using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;   // 单例（给其他脚本调用）

    [Header("自定义鼠标图片（PNG）")]
    public Texture2D cursorTexture;   // ★ 把你设计的鼠标图片拖进来即可

    private bool isLocked = true;

    private void Awake()
    {
        // 单例初始化
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LockCursor();  // 游戏开始默认锁定鼠标
    }

    // --------------------------------
    //         对外公开方法
    // --------------------------------

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isLocked = false;

        // ★ 设置自定义鼠标（无 hotspot）
        if (cursorTexture != null)
        {
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isLocked = true;

        // ★ 隐藏时重置为默认（避免残影）
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void ToggleCursor()
    {
        if (isLocked)
            UnlockCursor();
        else
            LockCursor();
    }
}
