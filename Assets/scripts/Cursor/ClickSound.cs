using UnityEngine;

public class ClickSound : MonoBehaviour
{
    public AudioSource audioSource;

    void Update()
    {
        // Play a click sound when the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.Play();
        }
    }
}