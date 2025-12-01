using UnityEngine;

public class PlantHintUI : MonoBehaviour
{
    public Transform target;
    public Vector3 worldOffset = new Vector3(0, 1.2f, 0);
    public Vector2 screenOffset = Vector2.zero;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        gameObject.SetActive(false);
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    private void LateUpdate()
    {
        if (target == null || cam == null) return;

        Vector3 worldPos = target.position + worldOffset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        if (screenPos.z < 0f)
        {
            SetVisible(false);
            return;
        }

        transform.position = screenPos + (Vector3)screenOffset;
    }
}