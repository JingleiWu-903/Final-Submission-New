using UnityEngine;

public class EnergyFullPanel : MonoBehaviour
{
    private CursorManager cursor;

    private void OnEnable()
    {
        if (cursor == null)
            cursor = FindObjectOfType<CursorManager>();

        if (cursor != null)
            cursor.UnlockCursor();
    }

    private void OnDisable()
    {
        if (cursor == null)
            cursor = FindObjectOfType<CursorManager>();

        if (cursor != null)
            cursor.LockCursor();
    }
}
