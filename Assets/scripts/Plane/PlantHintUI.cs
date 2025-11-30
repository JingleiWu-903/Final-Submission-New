using UnityEngine;

public class PlantHintUI : MonoBehaviour
{
    public static PlantHintUI Instance;

    public Vector3 worldOffset = new Vector3(0.2f, 0.7f, 0f); // UI 相对种植位的偏移
    private RectTransform rt;
    private Camera cam;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        rt = GetComponent<RectTransform>();
        cam = Camera.main;
        gameObject.SetActive(false);
    }

    /// <summary>让 P 提示跟随某个世界坐标</summary>
    public void Follow(Transform target)
    {
        if (target == null || cam == null) return;

        Vector3 worldPos = target.position + worldOffset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);
        rt.position = screenPos;
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}