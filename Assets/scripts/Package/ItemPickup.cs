using System.Collections;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData data;  // 物品数据

    private bool canBePicked = false;
    public GameObject netTrapArea;

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
        Debug.Log("🎯 正在被点击的网实例是：" + gameObject.name, gameObject);

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

        // 必须先移动 NetTrapArea（触发 OnTriggerExit）
        if (netTrapArea != null)
        {
            Debug.Log("📌 移动 Trigger 前：" + netTrapArea.transform.position);
            netTrapArea.transform.position += Vector3.up * 10f;   // 抬高触发区，使鱼触发Exit
            Debug.Log("📌 移动 Trigger 后：" + netTrapArea.transform.position);
        }

        // 延迟隐藏网（如果立即隐藏，Trigger 会消失，Exit 不会触发）
        StartCoroutine(HideNetAfterDelay(0.1f));

        Debug.Log("<color=green>拾取成功：" + data.itemName + "</color>");
    }

    private IEnumerator HideNetAfterDelay(float t)
    {
        yield return new WaitForSeconds(t);
        gameObject.SetActive(false);
    }

    // ----------- 鼠标点击（Unity 自动调用）------------
    private void OnMouseDown()
    {
        Pickup(); // 点击直接调用
    }
}
