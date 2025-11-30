using UnityEngine;


public class EnergyBall : MonoBehaviour
{
    public float lifeTime = 2f;        // 能量球存活时间
    public GameObject coralPrefab;     // 掉落的“小珊瑚”预制体
    public ItemData coralItem;         // 对应的珊瑚 ItemData（比如 CoralBlue）

    private void Start()
    {
        // 2 秒后自动销毁能量球
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("LargeTrash"))
            return;

        // 1. 记录碰撞点
        Vector3 hitPos = collision.contacts[0].point;
        Debug.Log("击中大垃圾，碰撞点：" + hitPos);

        // 2. 生成可拾取的“小珊瑚”
        if (coralPrefab != null)
        {
            // 稍微浮在地面上
            Vector3 spawnPos = hitPos + Vector3.up * 0.5f;

            GameObject coral = Instantiate(coralPrefab, spawnPos, Quaternion.identity);
            Debug.Log("生成可拾取珊瑚：" + coral.name);

            // 缩小一点
            coral.transform.localScale *= 0.3f;

            // 确保有拾取脚本
            CoralPickupF pickup = coral.GetComponent<CoralPickupF>();
            if (pickup == null)
                pickup = coral.AddComponent<CoralPickupF>();

            pickup.data = coralItem;   // 告诉它这是哪种珊瑚
        }

        // 3. 大垃圾消失
        Destroy(collision.gameObject);

        // 4. 能量球也消失
        Destroy(gameObject);
    }
}