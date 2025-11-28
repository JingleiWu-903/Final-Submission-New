using UnityEngine;
using UnityEngine.UI;  // 引入UI库，使用Slider

public class EnergySystem : MonoBehaviour
{
    public Slider energySlider;  // 能量条Slider
    public Text energyText;      // 能量文本（显示百分比）
    private float currentEnergy = 0f;  // 当前能量
    private float maxEnergy = 100f;   // 最大能量

    void Start()
    {
        currentEnergy = 0f;  // 初始能量为0
        energySlider.value = currentEnergy / maxEnergy;  // 设置Slider初始值
        energyText.text = "Energy: " + Mathf.RoundToInt(currentEnergy) + "%";  // 设置初始文本
    }

    // 增加能量
    public void AddEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0f, maxEnergy);  // 增加能量并确保不超过最大值
        energySlider.value = currentEnergy / maxEnergy;  // 更新Slider
        energyText.text = "Energy: " + Mathf.RoundToInt(currentEnergy) + "%";  // 更新文本
    }

    // 检查能量是否满
    public bool IsEnergyFull()
    {
        return currentEnergy >= maxEnergy;  // 如果能量满了，返回true
    }
}