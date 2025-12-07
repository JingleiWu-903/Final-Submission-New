using UnityEngine;
using UnityEngine.Video; 

[CreateAssetMenu(fileName = "NewItem", menuName = "Package/Item Data")]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Trash,       // garbage
        Coral,       // coral
        LargeTrash,  // Can
        Net          // Net
    }

    public ItemType itemType = ItemType.Trash;

    [Header("Basic Info")]
    public string itemName;
    public Sprite icon;        
    public string description;    

    [Header("Media")]
    public VideoClip videoClip;   // Drag and drop video directly
    public Sprite imagePreview;   // can also drag in pictures (optional)

    [Header("World Object")]
    public GameObject worldPrefab; // objects in scene
}
