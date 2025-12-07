using UnityEngine;
using UnityEngine.UI;

public class PickupHintUI : MonoBehaviour
{
    public static PickupHintUI Instance;

    [Header("Follow the target (object)")]
    public Transform followTarget;

    [Header("World coordinate offset")]
    public Vector3 worldOffset = new Vector3(0f, 0.3f, 0f);

    [Header("Screen pixel offset")]
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

        gameObject.SetActive(false); //Initially hidden
    }

    private void LateUpdate()
    {
        if (followTarget == null || cam == null)
            return;

        //World coordinates + offset
        Vector3 worldPos = followTarget.position + worldOffset;

        // World coordinate to screen coordinate transformation
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        //The background information is not displayed
        if (screenPos.z < 0)
        {
            gameObject.SetActive(false);
            return;
        }

        // Add screen offset
        screenPos += new Vector3(screenOffset.x, screenOffset.y, 0f);

        rect.position = screenPos;
    }

    /// <summary>
    /// Set the target to follow
    /// </summary>
    public void Follow(Transform target)
    {
        followTarget = target;
    }

    /// <summary>
    /// Show or hide the UI
    /// </summary>
    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
