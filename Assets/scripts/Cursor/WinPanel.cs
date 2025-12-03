using UnityEngine;

public class WinPanel : MonoBehaviour
{
    private CursorManager cursor;

    private void OnEnable()
    {
        if (cursor == null)
            cursor = FindObjectOfType<CursorManager>();

        if (cursor != null)
            cursor.UnlockCursor();   // 显示鼠标
    }

    private void OnDisable()
    {
        if (cursor == null)
            cursor = FindObjectOfType<CursorManager>();

        if (cursor != null)
            cursor.LockCursor();     // 恢复视角模式
    }
}