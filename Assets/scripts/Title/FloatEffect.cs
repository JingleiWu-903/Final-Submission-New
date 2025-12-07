using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatAmplitude = 0.1f; // Upward and downward floating range
    public float floatFrequency = 1f;   //floating speed

    [Header("Rotation Settings")]
    public float rotateAmplitude = 5f;  // Rotation angle
    public float rotateFrequency = 1f;  // Rotation speed
    private Vector3 startPos;
    private Quaternion startRot;
    private float randomOffsetPos;
    private float randomOffsetRot;

    private RectTransform rectTransform;
    private bool isUI;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        isUI = rectTransform != null;

        if (isUI)
            startPos = rectTransform.anchoredPosition;
        else
            startPos = transform.localPosition;

        startRot = transform.localRotation;

        // starts from a different position
        randomOffsetPos = Random.Range(0f, Mathf.PI * 2f);
        randomOffsetRot = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        //Float up and down 
        float floatOffset = Mathf.Sin(Time.time * floatFrequency + randomOffsetPos) * floatAmplitude;

        if (isUI)
            rectTransform.anchoredPosition = startPos + new Vector3(0, floatOffset, 0);
        else
            transform.localPosition = startPos + new Vector3(0, floatOffset, 0);

        //Slight rotation (Z) 
        float rotateZ = Mathf.Sin(Time.time * rotateFrequency + randomOffsetRot) * rotateAmplitude;
        transform.localRotation = startRot * Quaternion.Euler(0, 0, rotateZ);
    }
}
