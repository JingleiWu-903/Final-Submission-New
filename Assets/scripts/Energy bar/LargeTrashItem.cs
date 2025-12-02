using UnityEngine;

/// <summary>
/// 挂在【大型垃圾】（比如油桶）身上，告诉它对应哪个 ItemData
/// </summary>
public class LargeTrashItem : MonoBehaviour
{
    [Header("这个大型垃圾对应的背包物品（油桶的 ItemData）")]
    public ItemData data;
}