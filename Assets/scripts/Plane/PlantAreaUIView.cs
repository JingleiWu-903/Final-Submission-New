using UnityEngine;
using TMPro;

public class PlantAreaUIView : MonoBehaviour
{
    public Transform target;                       // use CoralPlantArea
    public Vector3 worldOffset = new Vector3(0, 3f, 0);
    public Vector2 screenOffset = Vector2.zero;
    public TextMeshProUGUI progressText;           // 0/9 text

    [Header("Up and down floating effect")]
    public float floatAmplitude = 0.3f;            // flying height
    public float floatSpeed = 1.0f;                // flying speed

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    public void SetProgress(int planted, int total)
    {
        if (progressText != null)
        {
            progressText.text = $"{planted} / {total}";
        }
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    private void LateUpdate()
    {
        if (target == null || cam == null) return;

        // Calculate the offset of the upper and lower fluctuations
        float hover = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        Vector3 worldPos = target.position + worldOffset + new Vector3(0, hover, 0);
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        if (screenPos.z < 0f) return;

        transform.position = screenPos + (Vector3)screenOffset;
    }
}