using UnityEngine;

public class GlowPulse : MonoBehaviour
{
    public float speed = 2f;
    public float intensity = 0.3f;
    private Renderer rend;
    private Color baseColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        baseColor = rend.material.GetColor("_EmissionColor");
    }

    void Update()
    {
        float pulse = Mathf.Sin(Time.time * speed) * intensity + 1f;
        rend.material.SetColor("_EmissionColor", baseColor * pulse);
    }
}