using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PackageCell : MonoBehaviour, IPointerClickHandler
{
    public Image iconImage;
    public TextMeshProUGUI nameText;

    private ItemData myData;
    private PackagePanel panel;

    // Initialize grid content
    public void Setup(ItemData data, PackagePanel parent)
    {
        myData = data;
        panel = parent;

        iconImage.sprite = data.icon;
        nameText.text = data.itemName;
    }
    // Click on the grid: Notify PackagePanel to display details
    public void OnPointerClick(PointerEventData eventData)
    {
        panel.ShowDetail(myData);
    }
}
