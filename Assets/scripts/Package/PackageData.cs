using System.Collections.Generic;
using UnityEngine;

public class PackageData : MonoBehaviour
{
    public static PackageData Instance;

    public List<ItemData> items = new List<ItemData>();

    private void Awake()
    {
        // 单例逻辑：如果场景里已有 Instance，就销毁自己
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
}
