using UnityEngine;
using TMPro;   // 用 TextMeshPro

public class CoralPlantArea : MonoBehaviour
{
    [Header("这一片区域的所有种植位")]
    public CoralPlantSlot[] slots;      // 可以不手动拖，会在 Start 里自动找

    [Header("UI：进度文本 和 胜利面板")]
    public TextMeshProUGUI progressText; // 显示 0/9
    public GameObject winPanel;         // 完成后弹出的 UI

    [Header("交互")]
    public KeyCode plantKey = KeyCode.P;
    public float interactDistance = 5f; // 玩家到坑位的最大距离

    private Transform player;
    private int plantedCount = 0;

    private void Start()
    {
        // 找玩家
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        // 自动收集子物体里的所有坑位
        if (slots == null || slots.Length == 0)
        {
            slots = GetComponentsInChildren<CoralPlantSlot>();
        }

        // 告诉每个坑位属于谁
        foreach (var slot in slots)
        {
            if (slot != null)
                slot.SetArea(this);

            if (slot != null && slot.isPlanted)
                plantedCount++;
        }

        if (winPanel != null) winPanel.SetActive(false);

        UpdateProgressUI();
    }

    private void Update()
    {
        if (player == null || slots == null || slots.Length == 0) return;

        // 找最近的、还没种植的坑位
        CoralPlantSlot nearest = null;
        float nearestDist = float.MaxValue;

        foreach (var slot in slots)
        {
            if (slot == null || slot.isPlanted) continue;

            float d = Vector3.Distance(player.position, slot.transform.position);
            if (d < nearestDist)
            {
                nearestDist = d;
                nearest = slot;
            }
        }

        bool canInteract = (nearest != null && nearestDist <= interactDistance);

        // 控制 P 提示 UI
        if (PlantHintUI.Instance != null)
        {
            PlantHintUI.Instance.SetVisible(canInteract);
            if (canInteract)
                PlantHintUI.Instance.Follow(nearest.transform);
        }

        // 按 P 种植
        if (canInteract && Input.GetKeyDown(plantKey))
        {
            if (nearest.TryPlant())
            {
                // 种植成功会回调 NotifyPlanted，这里不用管
            }
        }
    }

    public void NotifyPlanted()
    {
        plantedCount = 0;
        foreach (var slot in slots)
        {
            if (slot != null && slot.isPlanted)
                plantedCount++;
        }

        UpdateProgressUI();

        if (plantedCount >= slots.Length)
        {
            Debug.Log("所有珊瑚种植完成！");

            if (winPanel != null)
                winPanel.SetActive(true);

            if (PlantHintUI.Instance != null)
                PlantHintUI.Instance.SetVisible(false);
        }
    }

    private void UpdateProgressUI()
    {
        if (progressText != null)
        {
            progressText.text = $"{plantedCount}/{slots.Length}";
        }
    }
}