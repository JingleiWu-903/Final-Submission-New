using UnityEngine;

public class StartSceneCursor : MonoBehaviour
{
    public Texture2D cursorTexture; // Drag mouse pattern
    public Vector2 hotspot = Vector2.zero;

    void Start()
    {
        // Show mouse
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // mouse picture
        if (cursorTexture != null)
        {
            Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
        }

        // Let CursorManager not take over the mouse in the Start scene
        CursorManager.Instance?.UnlockCursor();
    }
}