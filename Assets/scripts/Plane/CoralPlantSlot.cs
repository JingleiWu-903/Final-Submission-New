using UnityEngine;

public class CoralPlantSlot : MonoBehaviour
{
    [Header("需要消耗的珊瑚 ItemData")]
    public ItemData coralItem;          // 比如 CoralBlue

    [Header("两个子物体")]
    public GameObject greyCoral;        // 灰白占位
    public GameObject realCoral;        // 真正的珊瑚

    [HideInInspector] public bool isPlanted = false;

    private CoralPlantArea area;        // 所属区域

    private void Start()
    {
        RefreshVisual();
    }

    public void SetArea(CoralPlantArea a)
    {
        area = a;
    }

    public void RefreshVisual()
    {
        if (greyCoral != null) greyCoral.SetActive(!isPlanted);
        if (realCoral != null) realCoral.SetActive(isPlanted);
    }

    /// <summary>尝试在这个坑位种植一棵，如果成功返回 true</summary>
    public bool TryPlant()
    {
        if (isPlanted) return false;
        if (coralItem == null) return false;
        if (PackageData.Instance == null) return false;

        // 没有珊瑚，种不了
        if (!PackageData.Instance.HasItem(coralItem))
        {
            Debug.Log("背包里没有珊瑚，无法种植。");
            return false;
        }

        // 消耗 1 个珊瑚
        if (!PackageData.Instance.RemoveOne(coralItem))
            return false;

        // 标记为已种植
        isPlanted = true;
        RefreshVisual();

        // 通知区域刷新进度
        if (area != null)
        {
            area.NotifyPlanted();
        }

        Debug.Log("在坑位 " + name + " 种植了一棵珊瑚。");
        return true;
    }
}