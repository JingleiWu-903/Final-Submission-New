using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class FirstPersonController : MonoBehaviour
    {
        [Header("移动参数")]
        [Tooltip("正常游动速度")]
        public float MoveSpeed = 2.0f;

        [Tooltip("按住 Shift 时的加速游动速度")]
        public float SprintSpeed = 4.0f;

        [Tooltip("按住空格时向上浮的速度")]
        public float UpSpeed = 3.0f;

        [Header("视角 & 旋转")]
        public GameObject CinemachineCameraTarget;
        [Tooltip("鼠标视角转动速度")]
        public float RotationSpeed = 1.0f;
        public float TopClamp = 90.0f;
        public float BottomClamp = -90.0f;

        [Header("水下镜头轻微漂浮（只动相机，不动角色）")]
        [Tooltip("上下漂浮幅度（越大越晃）")]
        public float BobAmplitude = 0.12f;    // 比之前大一点，更明显
        [Tooltip("漂浮频率（越大晃动越快）")]
        public float BobFrequency = 2.0f;

        // 视角 pitch
        private float _cinemachineTargetPitch;
        // 记录相机初始 localPosition
        private Vector3 _camTargetInitialLocalPos;

#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput != null &&
                       _playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
            }
        }

        private void Awake()
        {
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#endif

            if (CinemachineCameraTarget != null)
            {
                _camTargetInitialLocalPos = CinemachineCameraTarget.transform.localPosition;
            }
        }

        private void Update()
        {
            Move();  // ✅ 只有移动，没有重力和跳跃
        }

        private void LateUpdate()
        {
            CameraRotation();
            CameraBob();
        }

        /// <summary>
        /// 只做相机上下轻微漂浮，不动玩家
        /// </summary>
        private void CameraBob()
        {
            if (CinemachineCameraTarget == null) return;

            float offset = Mathf.Sin(Time.time * BobFrequency) * BobAmplitude;
            Vector3 lp = _camTargetInitialLocalPos;
            lp.y += offset;

            CinemachineCameraTarget.transform.localPosition = lp;
        }

        /// <summary>
        /// 视角旋转
        /// </summary>
        private void CameraRotation()
        {
            if (_input == null) return;

            if (_input.look.sqrMagnitude >= _threshold)
            {
                float dt = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetPitch += _input.look.y * RotationSpeed * dt;
                float yaw = _input.look.x * RotationSpeed * dt;

                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                if (CinemachineCameraTarget != null)
                {
                    CinemachineCameraTarget.transform.localRotation =
                        Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
                }

                transform.Rotate(Vector3.up * yaw);
            }
        }

        /// <summary>
        /// 水下移动核心逻辑：
        /// - WASD：沿摄像机方向的三维向量移动（可以上下潜）
        /// - Space：只在按住时向上浮，松手立刻停（没有惯性）
        /// </summary>
        private void Move()
        {
            if (_controller == null || _mainCamera == null || _input == null) return;

            // 1. 根据是否按住 Shift 选择速度
            float speed = _input.sprint ? SprintSpeed : MoveSpeed;

            Vector3 move = Vector3.zero;

            // 2. 处理 WASD 输入 -> 三维方向（跟随相机）
            Vector2 moveInput = _input.move;  // x = A/D, y = W/S

            if (moveInput.sqrMagnitude > 0.0001f)
            {
                Vector3 camForward = _mainCamera.transform.forward;
                Vector3 camRight = _mainCamera.transform.right;

                // 注意：这里不去掉 y 分量，所以能沿视线方向上下潜
                Vector3 dir = camRight * moveInput.x + camForward * moveInput.y;

                if (dir.sqrMagnitude > 0.0001f)
                {
                    dir.Normalize();
                    move += dir * speed;
                }
            }

            // 3. 空格：仅在按住时，向世界坐标 Y+ 方向上浮
            //    ❗ 这里只看 Input.GetKey，每一帧实时判断，不会“记住”状态
            if (Input.GetKey(KeyCode.Space))
            {
                move += Vector3.up * UpSpeed;
            }

            // 4. 真正移动：没有 _verticalVelocity，没有 Gravity，松开键就不会再动
            _controller.Move(move * Time.deltaTime);
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) angle += 360f;
            if (angle > 360f) angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }

   
    }
}