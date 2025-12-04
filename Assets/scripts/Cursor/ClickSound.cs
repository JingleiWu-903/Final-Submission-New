using UnityEngine;

public class ClickSound : MonoBehaviour
{
    public AudioSource audioSource;

    void Update()
    {
        // 鼠标左键按下时播放点击音效
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.Play();
        }
    }
}