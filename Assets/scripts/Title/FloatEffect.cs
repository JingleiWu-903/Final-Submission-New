using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatAmplitude = 0.1f; // 上下浮动幅度
    public float floatFrequency = 1f;   // 浮动速度

    [Header("Rotation Settings")]
    public float rotateAmplitude = 5f;  // 旋转角度幅度
    public float rotateFrequency = 1f;  // 旋转速度

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

        // 随机相位：每个漂浮物从不同位置开始
        randomOffsetPos = Random.Range(0f, Mathf.PI * 2f);
        randomOffsetRot = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        // --- 上下浮动 ---
        float floatOffset = Mathf.Sin(Time.time * floatFrequency + randomOffsetPos) * floatAmplitude;

        if (isUI)
            rectTransform.anchoredPosition = startPos + new Vector3(0, floatOffset, 0);
        else
            transform.localPosition = startPos + new Vector3(0, floatOffset, 0);

        // --- 轻微旋转（Z轴）---
        float rotateZ = Mathf.Sin(Time.time * rotateFrequency + randomOffsetRot) * rotateAmplitude;
        transform.localRotation = startRot * Quaternion.Euler(0, 0, rotateZ);
    }
}
