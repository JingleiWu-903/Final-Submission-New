using UnityEngine;

public class CoralPlantSlot : MonoBehaviour
{
    [Header("ItemData Configuration")]
    public ItemData coralItem;          //Just like the coralItem in Area

    [Header("Two sub-objects")]
    public GameObject greyCoral;        // Grayish-white placeholder
    public GameObject realCoral;        // True coral

    [HideInInspector]
    public bool isPlanted = false;      // Has it been planted yet

    private CoralPlantArea area;

    private void Start()
    {
        RefreshVisual();
    }

    public void SetArea(CoralPlantArea a)
    {
        area = a;
    }

    // Called by CoralPlantArea: The actual execution of "planting"
    public void Plant()
    {
        if (isPlanted) return;

        isPlanted = true;
        RefreshVisual();

        if (area != null)
        {
            area.NotifyPlanted(this);
        }

        Debug.Log("A coral was planted in the grid " + name + ".");
    }

    // Refresh the visibility of the gray/real coral
    public void RefreshVisual()
    {
        if (greyCoral != null)
            greyCoral.SetActive(!isPlanted);

        if (realCoral != null)
            realCoral.SetActive(isPlanted);
    }
}