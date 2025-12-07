using UnityEngine;

public class UnlockCursor : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;   // unlock mouse
        Cursor.visible = true;                   // Make mouse visible
    }
}
