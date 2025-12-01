using UnityEngine;

public class PlantHintUI : MonoBehaviour
{
    [Header("要跟随的目标（由 CoralPlantArea 设置）")]
    public Transform target;

    [Header("世界空间偏移（让 P 在珊瑚侧面或上方一点）")]
    public Vector3 worldOffset = new Vector3(0.4f, 1.0f, 0);

    [Header("屏幕空间微调")]
    public Vector3 screenOffset = Vector3.zero;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (!gameObject.activeSelf) return;
        if (target == null || cam == null) return;

        Vector3 worldPos = target.position + worldOffset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        transform.position = screenPos + screenOffset;
    }

    /// <summary>由 CoralPlantArea 调用，设置跟随目标</summary>
    public void Follow(Transform t)
    {
        target = t;
    }

    /// <summary>由 CoralPlantArea 调用，控制显示隐藏</summary>
    public void SetVisible(bool v)
    {
        gameObject.SetActive(v);
    }
}