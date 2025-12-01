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

    /// <summary>
    /// 从背包里“消耗”1 个指定 ItemData，成功返回 true。
    /// </summary>
    public bool ConsumeItem(ItemData item)
    {
        int index = items.FindIndex(i => i == item);
        if (index >= 0)
        {
            items.RemoveAt(index);
            Debug.Log("消耗 1 个：" + item.itemName);
            return true;
        }

        Debug.Log("背包里没有可以消耗的：" + item.itemName);
        return false;
    }
}