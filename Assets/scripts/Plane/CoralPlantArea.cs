using System.Collections.Generic;
using UnityEngine;

public class CoralPlantArea : MonoBehaviour
{
    [Header("需要消耗的珊瑚 ItemData")]
    public ItemData coralItem;          // CoralBlue (ItemData)

    [Header("范围 & 按键")]
    public float plantDistance = 6f;    // 玩家与区域中心距离
    public KeyCode plantKey = KeyCode.P;

    [Header("UI 引用")]
    public PlantAreaUIView plantAreaUI; // 说明 + 箭头 + 进度
    public PlantHintUI pressPHint;      // P 提示
    public GameObject winPanel;         // 胜利面板

    [Header("种植位（按顺序 PlantCoral_1 ~ PlantCoral_9）")]
    public List<CoralPlantSlot> slots = new List<CoralPlantSlot>();

    private Transform player;
    private int plantedCount = 0;

    private void Start()
    {
        // 找玩家
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("CoralPlantArea：找不到 Tag = Player 的玩家对象！");
        }

        // 若 slots 为空，则自动从子物体里找
        if (slots.Count == 0)
        {
            slots.AddRange(GetComponentsInChildren<CoralPlantSlot>());
        }

        // 初始化 P 提示（先跟随区域，之后会跟随具体 slot）
        if (pressPHint != null)
        {
            pressPHint.Follow(transform);
            pressPHint.SetVisible(false);
        }

        // 初始化 PlantAreaUI：一直显示在区域上方
        if (plantAreaUI != null)
        {
            plantAreaUI.Follow(transform);                     // 固定在区域中心上方
            plantAreaUI.Show(true);                            // 一直可见
            plantAreaUI.SetProgress(0, slots.Count);
        }

        // 胜利面板隐藏
        if (winPanel != null)
            winPanel.SetActive(false);
    }

    private void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        bool inRange = dist <= plantDistance;

        bool allPlanted = plantedCount >= slots.Count;
        CoralPlantSlot nextSlot = GetNextEmptySlot();

        // 处理 P 提示 —— 只要在范围内且还有位置，就让 P 挂到“下一棵灰珊瑚”旁边
        if (pressPHint != null)
        {
            bool showHint = inRange && !allPlanted && nextSlot != null;
            pressPHint.SetVisible(showHint);

            if (showHint)
            {
                pressPHint.Follow(nextSlot.transform);
            }
        }

        if (!inRange || allPlanted) return;

        // 在范围内按 P 尝试种植一棵
        if (Input.GetKeyDown(plantKey))
        {
            TryPlantOne();
        }
    }

    /// <summary>尝试种下一棵珊瑚</summary>
    private void TryPlantOne()
    {
        if (PackageData.Instance == null)
        {
            Debug.LogWarning("CoralPlantArea：没有 PackageData.Instance！");
            return;
        }

        // 消耗 1 个 CoralBlue
        if (!PackageData.Instance.ConsumeItem(coralItem))
        {
            Debug.Log("CoralPlantArea：背包里没有可种的珊瑚。");
            return;
        }

        CoralPlantSlot targetSlot = GetNextEmptySlot();
        if (targetSlot == null)
        {
            Debug.Log("CoralPlantArea：已经没有空的种植位了。");
            return;
        }

        targetSlot.SetPlanted(true);
        plantedCount++;

        Debug.Log($"在种植位 {targetSlot.name} 种下了一颗珊瑚，现在已种 {plantedCount}/{slots.Count}");

        if (plantAreaUI != null)
        {
            plantAreaUI.SetProgress(plantedCount, slots.Count);
        }

        if (plantedCount >= slots.Count)
        {
            // 种满 -> 关掉提示 & 显示胜利
            if (pressPHint != null) pressPHint.SetVisible(false);
            if (plantAreaUI != null) plantAreaUI.Show(false);
            if (winPanel != null) winPanel.SetActive(true);
        }
    }

    /// <summary>找到下一个“还没种”的 slot（按列表顺序）</summary>
    private CoralPlantSlot GetNextEmptySlot()
    {
        foreach (var slot in slots)
        {
            if (slot != null && !slot.isPlanted)
                return slot;
            Debug.Log("Slot index: " + slots.IndexOf(slot));
        }
       
        return null;
    }
}