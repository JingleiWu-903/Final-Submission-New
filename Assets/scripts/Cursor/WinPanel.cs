using UnityEngine;

public class WinPanel : MonoBehaviour
{
    private CursorManager cursor;

    private void OnEnable()
    {
        if (cursor == null)
            cursor = FindObjectOfType<CursorManager>();

        if (cursor != null)
            cursor.UnlockCursor();   // Show mouse
    }

    private void OnDisable()
    {
        if (cursor == null)
            cursor = FindObjectOfType<CursorManager>();

        if (cursor != null)
            cursor.LockCursor();     // Restore perspective mode
    }
}