using UnityEngine;
using UnityEngine.Video; // ← 必须加这个才能使用 VideoClip

[CreateAssetMenu(fileName = "NewItem", menuName = "Package/Item Data")]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Trash,       // 垃圾
        Coral,       // 珊瑚
        LargeTrash,  // 大型垃圾
        Net          // 渔网
    }

    public ItemType itemType = ItemType.Trash;

    [Header("Basic Info")]
    public string itemName;       // 物品名称
    public Sprite icon;           // 背包图标
    public string description;    // 描述文本

    [Header("Media")]
    public VideoClip videoClip;   // ← 直接拖入视频
    public Sprite imagePreview;   // ← 也可拖入图片（可选）

    [Header("World Object")]
    public GameObject worldPrefab; // 场景中的物体
}
