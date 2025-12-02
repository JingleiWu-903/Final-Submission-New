using UnityEngine;

public class CoralPlantArea : MonoBehaviour
{
    [Header("需要消耗的珊瑚 ItemData")]
    public ItemData coralItem;          // CoralBlue

    [Header("按键 & 距离")]
    public float plantDistance = 4f;    // 玩家离“某个格子”多近可以种
    public float areaRadius = 10f;      // 玩家离整个区域多近算“在区域内”
    public KeyCode plantKey = KeyCode.P;

    [Header("UI 引用")]
    public PlantAreaUIView areaUI;      // 进度 + 箭头 的大面板（PlantAreaUI）
    public PlantHintUI plantHintUI;     // P 提示小面板（PlantHintPanel）
    public PlantMessagePanel messageUI; // 顶部“没有珊瑚”那块（PlantMessagePanel）
    public GameObject winPanel;         // WinPanel

    [Header("所有种植位（按顺序）")]
    public CoralPlantSlot[] slots;

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
            Debug.LogWarning("[CoralPlantArea] 找不到 Tag = Player 的玩家对象！");
        }

        // 初始化格子
        plantedCount = 0;
        if (slots != null)
        {
            foreach (var s in slots)
            {
                if (s == null) continue;
                s.SetArea(this);
                if (s.isPlanted) plantedCount++;
            }
        }

        // 初始化进度 UI
        if (areaUI != null)
        {
            areaUI.SetTarget(transform);                   // 让它跟随整个区域中心
            areaUI.SetProgress(plantedCount, slots.Length);
            areaUI.SetVisible(true);
        }

        // P 提示先隐藏
        if (plantHintUI != null)
        {
            plantHintUI.SetVisible(false);
        }

        // 顶部信息面板先隐藏
        if (messageUI != null)
        {
            messageUI.HideInstant();
        }

        // WinPanel 先关
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (player == null) return;

        // ① 判断玩家是否在“区域附近”
        float distToArea = Vector3.Distance(player.position, transform.position);
        bool nearArea = distToArea <= areaRadius;

        // ② 找最近的“未种植” Slot（不限制距离，用于决定下一棵种在哪里）
        CoralPlantSlot closest = null;
        float closestDist = float.MaxValue;

        foreach (var s in slots)
        {
            if (s == null || s.isPlanted) continue;

            float d = Vector3.Distance(player.position, s.transform.position);
            if (d < closestDist)
            {
                closestDist = d;
                closest = s;
            }
        }

        // ③ 更新 P 提示 UI：必须在区域内 + 最近格子距离玩家不超过 plantDistance
        bool canShowHint = nearArea && closest != null && closestDist <= plantDistance;

        if (plantHintUI != null)
        {
            if (canShowHint)
            {
                plantHintUI.SetTarget(closest.transform);
                plantHintUI.SetVisible(true);
            }
            else
            {
                plantHintUI.SetVisible(false);
            }
        }

        // ④ 处理按键 P
        if (Input.GetKeyDown(plantKey))
        {
            // 玩家离区域太远：啥也不干
            if (!nearArea)
                return;

            // 背包里没有这种珊瑚
            if (!PackageData.Instance.HasItem(coralItem))
            {
                if (messageUI != null)
                {
                    string msg = (plantedCount == 0)
                        ? "There are no corals in your backpack!"
                        : "There are no corals in your backpack anymore!";

                    messageUI.ShowMessage(msg);
                }
                return;
            }

            // 消耗 1 个珊瑚并种在最近的格子
            if (PackageData.Instance.ConsumeItem(coralItem))
            {
                closest.Plant();
            }
        }
    }

    //  被 CoralPlantSlot 调用：有一棵种好了
    public void NotifyPlanted(CoralPlantSlot slot)
    {
        plantedCount++;

        if (areaUI != null)
        {
            areaUI.SetProgress(plantedCount, slots.Length);
        }

        Debug.Log($"[CoralPlantArea] 已种植数量: {plantedCount}/{slots.Length}");

        // 全部种完
        if (plantedCount >= slots.Length)
        {
            // 关闭 P 提示和进度 UI
            if (plantHintUI != null)
                plantHintUI.SetVisible(false);

            if (areaUI != null)
                areaUI.SetVisible(false);

            // 弹出胜利面板
            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }
            else
            {
                Debug.LogWarning("[CoralPlantArea] WinPanel 没有在 Inspector 里拖引用！");
            }

            // 顺便给一条提示文字
            if (messageUI != null)
            {
                messageUI.ShowMessage("You have repaired this coral reef!");
            }

            Debug.Log("[CoralPlantArea] 已种完所有珊瑚，胜利！");
        }
    }
}