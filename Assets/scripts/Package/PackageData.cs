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
        Debug.Log("Picked up successfully：" + item.itemName);
    }

    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
        Debug.Log("Delete items：" + item.itemName);
    }

    // Whether there is an item
    public bool HasItem(ItemData item)
    {
        foreach (var i in items)
        {
            if (i == item) return true;
        }
        return false;
    }

    // Consume an item (for planting)
    public bool ConsumeItem(ItemData item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == item)
            {
                items.RemoveAt(i);
                Debug.Log("Consume：" + item.itemName);
                return true;
            }
        }
        return false;
    }
}