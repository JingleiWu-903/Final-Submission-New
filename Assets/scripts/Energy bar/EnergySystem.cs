using UnityEngine;
using UnityEngine.UI;  // 使用 Slider

public class EnergySystem : MonoBehaviour
{
    public Slider energySlider;        // 能量条
    public int energy = 0;             // 当前能量（0~3）
    public int maxEnergy = 3;          // 最大能量 = 3（捡3次垃圾）

    public GameObject energyFullPanel; // 能量满时的提示UI
    public GameObject energyBallPrefab;// 能量球预制体
    public Transform player;           // 玩家 Transform

    private void Start()
    {
        // 把 Slider 设置成 0~3 的整格
        energySlider.minValue = 0;
        energySlider.maxValue = maxEnergy;
        energySlider.wholeNumbers = true;
        energySlider.value = energy;

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
        // ✅ 注意：这里不再把 energy 重置为 0，
        // 只要能量 >= max，就可以无限按 E 发射
        if (energy >= maxEnergy && Input.GetKeyDown(KeyCode.E))
        {
            FireEnergyBall();
        }
    }

    // 关闭“能量已满”提示的按钮用
    public void CloseEnergyFullPanel()
    {
        if (energyFullPanel != null)
            energyFullPanel.SetActive(false);
    }

    // 发射能量球
    private void FireEnergyBall()
    {
        Debug.Log("FireEnergyBall 来自对象: " + gameObject.name);

        if (energyBallPrefab == null || player == null)
        {
            Debug.LogWarning("EnergyBallPrefab 或 player 没有设置！");
            return;
        }

        // 在玩家前方生成能量球
        GameObject ball = Instantiate(
            energyBallPrefab,
            player.position + player.forward * 2f,
            Quaternion.identity
        );

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(player.forward * 15f, ForceMode.VelocityChange);
        }
    }
}