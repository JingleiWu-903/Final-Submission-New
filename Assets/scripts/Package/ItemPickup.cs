using System.Collections;
using UnityEngine;
using static ItemData;


public class ItemPickup : MonoBehaviour
{
    public ItemData data;  // 物品数据

    private bool canBePicked = false;

    private void Awake()
    {
        // 物品生成后延迟一段时间才可以拾取
        StartCoroutine(EnablePickupAfterDelay(0.3f));
    }

    private IEnumerator EnablePickupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canBePicked = true;
    }

    private void OnMouseDown()
    {
        if (!canBePicked) return;

        // 打印调试日志以确保进入了 OnMouseDown
        Debug.Log("Mouse clicked on: " + gameObject.name);

        // 只有捡到垃圾时才增加能量
        if (data.itemType == ItemData.ItemType.Trash)
        {
            EnergySystem energySystem = FindObjectOfType<EnergySystem>();
            if (energySystem != null)
            {
                energySystem.AddEnergy(10f);  // 每次捡到垃圾增加10能量
            }
        }

        // 将物品加入背包
        PackageData.Instance.AddItem(data);

        // 刷新背包界面
        PackagePanel panel = FindObjectOfType<PackagePanel>();
        if (panel != null)
        {
            panel.RefreshScroll();
        }

        // 销毁物体（拾取后消失）
        gameObject.SetActive(false);

        Debug.Log("<color=green>拾取成功: " + data.itemName + "</color>");
    }
}