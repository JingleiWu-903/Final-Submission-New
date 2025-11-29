using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    public float lifeTime = 2f;      // 能量球存活时间
    public GameObject coralPrefab;   // 掉落在场景里的珊瑚预制体
    public ItemData coralItem;       // 珊瑚对应的 ItemData（进背包用）

    private void Start()
    {
        // 2 秒后自动销毁能量球（防止一直存在）
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("EnergyBall 碰到了：" + collision.gameObject.name);

        // 只处理打到「大型垃圾 LargeTrash」的情况
        if (collision.gameObject.CompareTag("LargeTrash"))
        {
            Debug.Log("能量球击中大型垃圾：" + collision.gameObject.name);

            // 1. 删除大型垃圾
            Destroy(collision.gameObject);

            // 2. 在场景中生成一块珊瑚
            if (coralPrefab != null)
            {
                Instantiate(
                    coralPrefab,
                    collision.transform.position + Vector3.up * 0.5f,
                    Quaternion.identity
                );
            }
            else
            {
                Debug.LogWarning("EnergyBall 没有设置 coralPrefab！");
            }

            // 3. 珊瑚进入背包（使用对应的 ItemData）
            if (coralItem != null && PackageData.Instance != null)
            {
                PackageData.Instance.AddItem(coralItem);
                Debug.Log("珊瑚已进入背包：" + coralItem.itemName);
            }
            else
            {
                Debug.LogWarning("coralItem 或 PackageData.Instance 为空，无法加到背包！");
            }

            // 4. 删除能量球本体
            Destroy(gameObject);
        }
    }
}