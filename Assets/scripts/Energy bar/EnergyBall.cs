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
        // 只处理 LargeTrash（大型垃圾），其他碰撞直接返回
        if (!collision.gameObject.CompareTag("LargeTrash"))
            return;

        Debug.Log("能量球击中大型垃圾：" + collision.gameObject.name);

        // ------------ 1. 先把大型垃圾本身加入背包 ------------
        LargeTrashItem largeItem = collision.gameObject.GetComponent<LargeTrashItem>();
        if (largeItem != null && largeItem.data != null)
        {
            // 加入背包
            PackageData.Instance.AddItem(largeItem.data);

            // 刷新背包 UI
            var panel = FindObjectOfType<PackagePanel>();
            if (panel != null)
            {
                panel.RefreshScroll();
            }

            Debug.Log("<color=yellow>大型垃圾已加入背包：" + largeItem.data.itemName + "</color>");
        }
        else
        {
            Debug.LogWarning("击中 LargeTrash，但没有挂 LargeTrashItem 或 data 为空：" + collision.gameObject.name);
        }

        // ------------ 2. 掉落珊瑚（保持你原来的逻辑） ------------
        if (coralPrefab != null)
        {
            // 计算一个比较合理的掉落位置（碰撞点上方一点）
            Vector3 hitPos;

            if (collision.contacts != null && collision.contacts.Length > 0)
            {
                hitPos = collision.contacts[0].point;
            }
            else
            {
                hitPos = collision.transform.position;
            }

            GameObject coral = Instantiate(
                coralPrefab,
                hitPos + Vector3.up * 0.5f,
                Quaternion.identity
            );

            Debug.Log("掉落可拾取珊瑚：" + coral.name);
        }

        // ------------ 3. 销毁大垃圾和能量球 ------------
        Destroy(collision.gameObject);  // 大垃圾消失
        Destroy(gameObject);            // 能量球自己也消失
    }
}