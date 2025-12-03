using UnityEngine;

public class PackageInput : MonoBehaviour
{
    public PackagePanel panel;
    private CursorManager cursor;

    void Start()
    {
        cursor = FindAnyObjectByType<CursorManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (panel.gameObject.activeSelf)
            {
                panel.gameObject.SetActive(false);
                cursor.LockCursor();     // 关闭背包 → 视角恢复
            }
            else
            {
                panel.gameObject.SetActive(true);
                CursorManager.Instance.UnlockCursor();
                // 打开背包 → 鼠标自由
            }
        }
    }
}
