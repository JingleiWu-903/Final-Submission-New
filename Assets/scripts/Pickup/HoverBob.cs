using UnityEngine;

public class HoverBob : MonoBehaviour
{
    public float amplitude = 0.1f;   // 上下浮动幅度
    public float frequency = 2f;     // 浮动速度
    public float rotateSpeed = 50f;  // 旋转速度

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }
}