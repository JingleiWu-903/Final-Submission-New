using System.Collections;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData data;  // 物品数据

    private bool canBePicked = false;

    private void Awake()
    {
        StartCoroutine(EnablePickupAfterDelay(0.3f));
    }

    private IEnumerator EnablePickupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canBePicked = true;
    }

    // ----------- 对外开放可调用的拾取函数 -----------
    public void Pickup()
    {
        if (!canBePicked) return;

        Debug.Log("<color=yellow>Pickup() 被调用: " + gameObject.name + "</color>");

        // 如果是垃圾 → 加能量段
        if (data.itemType == ItemData.ItemType.Trash)
        {
            var energySystem = FindFirstObjectByType<EnergySystem>();
            if (energySystem != null)
            {
                energySystem.AddEnergy(1);
            }
        }

        // 加入背包
        PackageData.Instance.AddItem(data);

        // 刷新UI
        var panel = FindFirstObjectByType<PackagePanel>();
        if (panel != null)
        {
            panel.RefreshScroll();
        }

        // 隐藏物体
        gameObject.SetActive(false);

        Debug.Log("<color=green>拾取成功：" + data.itemName + "</color>");
    }

    // ----------- 鼠标点击（Unity 自动调用）------------
    private void OnMouseDown()
    {
        Pickup(); // 点击直接调用
    }
}
