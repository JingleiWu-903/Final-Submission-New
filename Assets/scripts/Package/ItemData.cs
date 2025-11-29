using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Package/Item Data")]
public class ItemData : ScriptableObject
{
    // 物品类型枚举
    public enum ItemType
    {
        Trash,     // 垃圾
        Coral,     // 珊瑚
        LargeTrash,// 大型垃圾
        Net        // 渔网
    }

    public ItemType itemType = ItemType.Trash; // 设置为垃圾类型
    public string itemName;        // 物品名称
    public Sprite icon;            // 图标（背包格子里的图片）
    public string description;     // 介绍文本
    public string videoPath;       // 视频文件名（不写 .mp4）
    public GameObject worldPrefab; // 物品在场景中的表现（如渔网、垃圾）
}
