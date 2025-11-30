using UnityEngine;


public class EnergyBall : MonoBehaviour
{
    public float lifeTime = 2f;

    // 掉落的珊瑚 prefab（我们会在 Inspector 里拖 Batched Coral 9 Pickup）
    public GameObject coralPrefab;
    // 这个珊瑚在背包里的 ItemData（比如 CoralBlue）
    public ItemData coralItem;

    private void Start()
    {
        // 2 秒后自动销毁能量球
        Destroy(gameObject, lifeTime);
    }
    public Vector3 coralSpawnScale = new Vector3(0.25f, 0.25f, 0.25f); // 掉落珊瑚的缩放
    private void OnCollisionEnter(Collision collision)
    {
        // 只处理打到大垃圾的情况
        if (!collision.gameObject.CompareTag("LargeTrash")) return;

        Debug.Log("能量球击中大型垃圾：" + collision.gameObject.name);

        // 1. 记录碰撞点（更准确地贴在地上）
        Vector3 hitPos = collision.contacts.Length > 0
            ? collision.contacts[0].point
            : collision.transform.position;

        // 2. 让大垃圾消失
        Destroy(collision.gameObject);

        // 3. 掉落珊瑚
        if (coralPrefab != null)
        {
            Vector3 spawnPos = hitPos + Vector3.up * 0.5f; // 稍微离地面一点
            GameObject coral = Instantiate(coralPrefab, spawnPos, Quaternion.identity);

            // 关键：缩小
            coral.transform.localScale = coralSpawnScale;
        }

        // 4. 自己也消失
        Destroy(gameObject);
    }
}