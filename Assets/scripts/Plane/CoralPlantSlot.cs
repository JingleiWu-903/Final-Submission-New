using UnityEngine;

public class CoralPlantSlot : MonoBehaviour
{
    [Header("ItemData 配置")]
    public ItemData coralItem;          // 一般和 Area 里的 coralItem 一样

    [Header("两个子物体")]
    public GameObject greyCoral;        // 灰白占位
    public GameObject realCoral;        // 真正的珊瑚

    [HideInInspector]
    public bool isPlanted = false;      // 是否已经种植

    private CoralPlantArea area;

    private void Start()
    {
        RefreshVisual();
    }

    public void SetArea(CoralPlantArea a)
    {
        area = a;
    }

    // ✅ 由 CoralPlantArea 调用：真正执行“种植”
    public void Plant()
    {
        if (isPlanted) return;

        isPlanted = true;
        RefreshVisual();

        if (area != null)
        {
            area.NotifyPlanted(this);
        }

        Debug.Log("在格子 " + name + " 种了一颗珊瑚");
    }

    // 刷新灰/真珊瑚的显隐
    public void RefreshVisual()
    {
        if (greyCoral != null)
            greyCoral.SetActive(!isPlanted);

        if (realCoral != null)
            realCoral.SetActive(isPlanted);
    }
}