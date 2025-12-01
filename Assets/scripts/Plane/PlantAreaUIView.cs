using UnityEngine;
using TMPro;

public class PlantAreaUIView : MonoBehaviour
{
    [Header("显示进度的 Text（整块 UI 里那行带 0/9 的文本）")]
    public TMP_Text progressText;

    [Header("前缀文字（写你要的说明）")]
    [TextArea]
    public string prefix = "在这里种植珊瑚：";

    [Header("跟随设置")]
    public Transform target;                 // 一般就是 CoralPlantArea
    public Vector3 worldOffset = new Vector3(0, 2.5f, 0); // 在区域上方
    public Vector3 screenOffset = Vector3.zero;

    [Header("上下浮动")]
    public float floatAmplitude = 0.2f; // 浮动高度（世界坐标）
    public float floatSpeed = 1.5f;     // 浮动速度

    private Camera cam;
    private float baseOffsetY;

    private void Awake()
    {
        cam = Camera.main;
        baseOffsetY = worldOffset.y;
    }

    private void LateUpdate()
    {
        if (target == null || cam == null) return;

        // 在世界空间里上下浮动一点
        Vector3 wo = worldOffset;
        wo.y = baseOffsetY + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        Vector3 worldPos = target.position + wo;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        transform.position = screenPos + screenOffset;
    }

    /// <summary>更新进度显示：前缀 + 当前/总数</summary>
    public void SetProgress(int current, int total)
    {
        if (progressText != null)
        {
            progressText.text = $"{prefix}{current} / {total}";
        }
    }

    /// <summary>设置要跟随的目标（在 CoralPlantArea 里调用）</summary>
    public void Follow(Transform t)
    {
        target = t;
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }
}