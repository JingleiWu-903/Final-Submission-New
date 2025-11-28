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

    // 初始化格子内容
    public void Setup(ItemData data, PackagePanel parent)
    {
        myData = data;
        panel = parent;

        iconImage.sprite = data.icon;
        nameText.text = data.itemName;
    }
    // 点击格子：通知 PackagePanel 显示详情
    public void OnPointerClick(PointerEventData eventData)
    {
        panel.ShowDetail(myData);
    }
}
