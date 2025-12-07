using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;  

    [Header("Custom mouse picture£¨PNG£©")]
    public Texture2D cursorTexture;   

    private bool isLocked = true;

    private void Awake()
    {
        // Singleton initialization
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
        LockCursor();  // locking mouse by default when the game starts
    }

  

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isLocked = false;

        //  Set up a custom mouse (no hotspot)
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

        // Reset to default when hidden (to avoid ghosting)
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
