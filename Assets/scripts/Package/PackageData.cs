using System.Collections.Generic;
using UnityEngine;

public class PackageData : MonoBehaviour
{
    public static PackageData Instance;

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

    // ✅ 是否有某个物品
    public bool HasItem(ItemData item)
    {
        foreach (var i in items)
        {
            if (i == item) return true;
        }
        return false;
    }

    // ✅ 消耗一个物品（种植用）
    public bool ConsumeItem(ItemData item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == item)
            {
                items.RemoveAt(i);
                Debug.Log("消耗物品：" + item.itemName);
                return true;
            }
        }
        return false;
    }
}