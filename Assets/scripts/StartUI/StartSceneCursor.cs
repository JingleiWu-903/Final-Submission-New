using UnityEngine;

public class StartSceneCursor : MonoBehaviour
{
    public Texture2D cursorTexture; // 拖你自定义的鼠标图案
    public Vector2 hotspot = Vector2.zero;

    void Start()
    {
        // 显示鼠标
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // 使用你的自定义鼠标
        if (cursorTexture != null)
        {
            Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
        }

        // 让 CursorManager 在 Start 场景不接管鼠标
        CursorManager.Instance?.UnlockCursor();
    }
}