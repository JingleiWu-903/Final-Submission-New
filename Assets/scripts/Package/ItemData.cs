using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Package/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;         // 物品名称
    public Sprite icon;             // 图标（背包格子里的图片）

    
    public string description;      // 介绍文本

    public string videoPath;        // 视频文件名（不写 .mp4）
   
    public GameObject worldPrefab;
}
