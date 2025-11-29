using UnityEngine;
using UnityEngine.UI;  // 引入UI库，使用Slider

public class EnergySystem : MonoBehaviour
{
    public Slider energySlider;         // 能量条
    public int energy = 0;              // 当前能量（0~3）
    public int maxEnergy = 3;           // 最大能量 = 3（捡 3 次垃圾）

    public GameObject energyFullPanel;  // 能量满时提示UI
    public GameObject energyBallPrefab; // 能量球预制体
    public Transform player;            // 玩家

    private void Start()
    {
        // 重要：让 Slider 变成 0~3 的整格
        energySlider.minValue = 0;
        energySlider.maxValue = maxEnergy;
        energySlider.wholeNumbers = true;   // 显示为整格
        energySlider.value = energy;

        if (energyFullPanel != null)
            energyFullPanel.SetActive(false);

        Debug.Log("Initial Energy: " + energy);
    }

    // 每捡一次垃圾 +1 格能量
    public void AddEnergy(int amount)
    {
        energy += amount;
        if (energy > maxEnergy) energy = maxEnergy;

        energySlider.value = energy;   // 更新 UI

        Debug.Log("Current Energy: " + energy);

        if (energy >= maxEnergy && energyFullPanel != null)
        {
            energyFullPanel.SetActive(true);   // 显示“按E发射能量球”
        }
    }

    private void Update()
    {
        // 能量满且按下 E
        if (energy >= maxEnergy && Input.GetKeyDown(KeyCode.E))
        {
            FireEnergyBall();

            // 发射后清空能量
            energy = 0;
            energySlider.value = energy;

            if (energyFullPanel != null)
                energyFullPanel.SetActive(false);
        }
    }

    // 发射能量球
    private void FireEnergyBall()
    {
        if (energyBallPrefab != null && player != null)
        {
            GameObject energyBall = Instantiate(
                energyBallPrefab,
                player.position + player.forward * 2f,
                Quaternion.identity);

            Rigidbody rb = energyBall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(player.forward * 15f, ForceMode.VelocityChange);
            }

            Debug.Log("Energy ball fired!");
        }
        else
        {
            Debug.LogWarning("EnergyBallPrefab 或 player 没有设置！");
        }
    }
}