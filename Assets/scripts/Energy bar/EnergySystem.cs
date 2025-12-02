using UnityEngine;
using UnityEngine.UI;  // 使用 Slider


public class EnergySystem : MonoBehaviour
{
    [Header("能量设置")]
    public Slider energySlider;        // 能量条
    public int energy = 0;             // 当前能量（0~3）
    public int maxEnergy = 3;          // 最大能量 = 3（捡3次垃圾）

    [Header("UI 与预制体")]
    public GameObject energyFullPanel; // 能量满时的提示UI
    public GameObject energyBallPrefab;// 能量球预制体
    public Transform player;           // 玩家 Transform

    [Header("能量球发射参数")]
    public float shootForce = 7f;      // 初速度
    public float upwardFactor = 0.05f;  // 向上抬头的比例

    private void Start()
    {
        // 把 Slider 设置成 0~3 的整格
        if (energySlider != null)
        {
            energySlider.minValue = 0;
            energySlider.maxValue = maxEnergy;
            energySlider.wholeNumbers = true;
            energySlider.value = energy;
        }

        if (energyFullPanel != null)
            energyFullPanel.SetActive(false);

        Debug.Log("Initial Energy: " + energy);
    }

    // 每捡一次垃圾 +1 格能量
    public void AddEnergy(int amount)
    {
        // 已经满了就不要再加（保持 3 格）
        if (energy >= maxEnergy)
            return;

        energy += amount;
        if (energy > maxEnergy)
            energy = maxEnergy;

        if (energySlider != null)
            energySlider.value = energy;

        Debug.Log("Current Energy: " + energy);

        // 满电后弹出提示 UI
        if (energy >= maxEnergy && energyFullPanel != null)
        {
            energyFullPanel.SetActive(true);
        }
    }

    private void Update()
    {
        // ⚠ 不再把 energy 重置为 0，只要能量 >= maxEnergy 就可以无限按 E 发射
        if (energy >= maxEnergy && Input.GetKeyDown(KeyCode.E))
        {
            FireEnergyBall();
        }
    }

    // 关闭“能量已满”提示（给按钮用）
    public void CloseEnergyFullPanel()
    {
        if (energyFullPanel != null)
            energyFullPanel.SetActive(false);
    }

    // 发射能量球（带抛物线）
    private void FireEnergyBall()
    {
        if (energyBallPrefab == null || player == null)
        {
            Debug.LogWarning("EnergyBallPrefab 或 player 没有设置！");
            return;
        }

        // 在玩家前方一点 + 稍微抬高的位置生成
        Vector3 spawnPos = player.position + player.forward * 1.0f + Vector3.up * 0.5f;
        GameObject energyBall = Instantiate(energyBallPrefab, spawnPos, Quaternion.identity);

        Rigidbody rb = energyBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.useGravity = true; // 记得在预制体上也勾上 Use Gravity

            // 发射方向：前方 + 少量向上
            Vector3 dir = (player.forward + Vector3.up * upwardFactor).normalized;
            rb.AddForce(dir * shootForce, ForceMode.VelocityChange);
        }

        Debug.Log("Energy ball fired!");
    }
}