using UnityEngine;
using UnityEngine.UI;

public class PickupHintUI : MonoBehaviour
{
    public static PickupHintUI Instance;

    [Header("跟随目标（物体）")]
    public Transform followTarget;

    [Header("世界坐标偏移")]
    public Vector3 worldOffset = new Vector3(0f, 0.3f, 0f);

    [Header("屏幕像素偏移")]
    public Vector2 screenOffset = new Vector2(0f, -60f);

    private Camera cam;
    private RectTransform rect;

    private void Awake()
    {
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

        // 世界坐标 + 偏移
        Vector3 worldPos = followTarget.position + worldOffset;

        // 世界坐标转屏幕
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        // 背后情况，不显示
        if (screenPos.z < 0)
        {
            gameObject.SetActive(false);
            return;
        }

        // 加屏幕偏移
        screenPos += new Vector3(screenOffset.x, screenOffset.y, 0f);

        rect.position = screenPos;
    }

    /// <summary>
    /// 设置跟随目标
    /// </summary>
    public void Follow(Transform target)
    {
        followTarget = target;
    }

    /// <summary>
    /// 显示或隐藏 UI
    /// </summary>
    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
