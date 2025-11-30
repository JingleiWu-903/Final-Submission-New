using UnityEngine;

public class PickupHintUI : MonoBehaviour
{
    public static PickupHintUI Instance;

    [Header("跟随目标（珊瑚等）")]
    public Transform followTarget;   // 要跟随的世界物体

    [Header("世界坐标偏移")]
    public Vector3 worldOffset = new Vector3(0f, 0.3f, 0f);
    // 决定在“物体的哪个世界位置”上挂 UI，一般稍微高一点

    [Header("屏幕 UI 偏移（像素）")]
    public Vector2 screenOffset = new Vector2(0f, -60f);
    // 决定 UI 在屏幕上往下 / 往旁边偏多少像素

    private Camera cam;
    private RectTransform rect;

    private void Awake()
    {
        // 单例模式
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        cam = Camera.main;
        rect = GetComponent<RectTransform>();

        gameObject.SetActive(false); // 初始隐藏
    }

    private void LateUpdate()
    {
        if (followTarget == null || cam == null)
            return;

        // 1. 世界坐标 + 世界偏移
        Vector3 worldPos = followTarget.position + worldOffset;

        // 2. 世界坐标转屏幕坐标
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        // 3. 再加上屏幕像素偏移（往下、往右偏一点）
        screenPos += new Vector3(screenOffset.x, screenOffset.y, 0f);

        rect.position = screenPos;
    }

    /// <summary>
    /// 设置要跟随的目标（例如某一个珊瑚）
    /// </summary>
    public void Follow(Transform target)
    {
        followTarget = target;
    }

    /// <summary>
    /// 控制 UI 显示 / 隐藏
    /// </summary>
    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}