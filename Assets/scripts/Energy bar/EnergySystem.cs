using UnityEngine;
using UnityEngine.UI;  // 引入UI库，使用Slider

public class EnergySystem : MonoBehaviour
{
    public Slider energySlider;    // 能量条滑块
    public float energy = 0;       // 当前能量
    public float maxEnergy = 100;  // 最大能量
    public int trashCount = 0;     // 已捡到的垃圾数量
    public int maxTrashCount = 3;  // 充能所需的垃圾数量
    public GameObject energyFullPanel;  // 能量充满时的UI提示

    public GameObject energyBallPrefab;  // 能量球的预制体
    public Transform player;  // 玩家的位置

    private void Start()
    {
        energySlider.value = energy / maxEnergy;
        energyFullPanel.SetActive(false);
        Debug.Log("Initial Energy: " + energy);   // 初始能量日志
    }

    // 增加能量
    public void AddEnergy(float amount)
    {
        if (energySlider == null)
        {
            Debug.LogError("❌ energySlider 没有在 Inspector 里赋值！");
            return;
        }

        if (energyFullPanel == null)
        {
            Debug.LogError("❌ energyFullPanel 没有在 Inspector 里赋值！");
            return;
        }

        // 增加能量
        energy += amount;
        if (energy > maxEnergy) energy = maxEnergy;

        Debug.Log("当前能量：" + energy);

        // ⭐ 设置 Slider（不会超出 0~1）
        energySlider.value = Mathf.Clamp(energy / maxEnergy, 0f, 1f);

        // 如果满了，显示提示 UI
        if (energy >= maxEnergy)
        {
            energyFullPanel.SetActive(true);
        }
    }
    // 按 E 发射能量球
    private void Update()
    {
        if (energy >= maxEnergy && Input.GetKeyDown(KeyCode.E))  // 按 E 发射能量球
        {
            FireEnergyBall();
            energy = 0;  // 发射后重置能量
            energySlider.value = energy / maxEnergy;  // 更新UI
            energyFullPanel.SetActive(false);  // 隐藏提示
        }
    }

    // 发射能量球
    private void FireEnergyBall()
    {
        if (energyBallPrefab != null)
        {
            GameObject energyBall = Instantiate(energyBallPrefab, player.position + player.forward * 2, Quaternion.identity);
            Rigidbody rb = energyBall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(player.forward * 15f, ForceMode.VelocityChange);  // 给能量球加速度
            }
            Debug.Log("Energy ball fired!");
        }
    }
}
