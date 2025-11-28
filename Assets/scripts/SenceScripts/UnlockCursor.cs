using UnityEngine;

public class UnlockCursor : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;   // 解锁鼠标
        Cursor.visible = true;                   // 让鼠标可见
    }
}
