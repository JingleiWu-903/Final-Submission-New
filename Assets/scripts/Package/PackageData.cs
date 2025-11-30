using System.Collections.Generic;
using UnityEngine;

public class PackageData : MonoBehaviour
{
    public static PackageData Instance;

    // 背包里所有物品
    public List<ItemData> items = new List<ItemData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(ItemData item)
    {
        items.Add(item);
        Debug.Log("拾取成功：" + item.itemName);
    }

    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
        Debug.Log("删除物品：" + item.itemName);
    }

    // ✅ 是否背包中至少有 1 个指定 ItemData
    public bool HasItem(ItemData target)
    {
        if (target == null) return false;
        return items.Contains(target);
    }

    // ✅ 消耗 1 个指定 ItemData，成功返回 true
    public bool RemoveOne(ItemData target)
    {
        if (target == null) return false;
        if (items.Contains(target))
        {
            items.Remove(target);
            Debug.Log("消耗 1 个物品：" + target.itemName);
            return true;
        }
        return false;
    }
}