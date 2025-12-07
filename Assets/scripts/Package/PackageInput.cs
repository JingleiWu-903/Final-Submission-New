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
                cursor.LockCursor();     // Turn off the backpack //view restoration
            }
            else
            {
                panel.gameObject.SetActive(true);
                CursorManager.Instance.UnlockCursor();
                // Open the backpack // the mouse freely
            }
        }
    }

    public bool IsPanelOpen()
    {
        if (panel == null) return false;
        return panel.gameObject.activeSelf;
    }

    public void OpenPanel()
    {
        panel.gameObject.SetActive(true);
        cursor.UnlockCursor();
    }

}
