using UnityEngine;

public class PackageInput : MonoBehaviour
{
    public PackagePanel panel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            panel.TogglePanel();
        }
    }
}
