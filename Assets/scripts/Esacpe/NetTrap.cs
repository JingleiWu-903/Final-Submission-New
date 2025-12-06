using UnityEngine;

public class NetTrap : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("🚨 OnTriggerExit 被触发：" + other.name);

        FishEscape fish = other.GetComponent<FishEscape>();
        if (fish != null)
        {
            Debug.Log("🐟 找到了 FishEscape，开始逃跑！");
            fish.StartEscape();
        }
        else
        {
            Debug.Log("⚠ OnTriggerExit 触发了，但对象不是鱼：" + other.name);
        }
    }

}
