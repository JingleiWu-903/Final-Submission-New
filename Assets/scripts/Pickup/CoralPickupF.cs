using UnityEngine;

public class CoralPickupF : MonoBehaviour
{
    public ItemData data;           // 对应的珊瑚 ItemData
    public Transform hintUI;        // 提示用的 F UI（世界空间 Canvas）
    public float showDistance = 3f; // 多远开始显示提示
    public float pickupDistance = 2f; // 多远内可以按 F 拾取

    private Transform player;
    private bool isInRange = false;

    private void Start()
    {
        // 找玩家
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // 如果 Inspector 里没手动拖 hintUI，就尝试在子物体里找 “HintCanvas”
        if (hintUI == null)
        {
            Transform child = transform.Find("HintCanvas");
            if (child != null)
            {
                hintUI = child;
            }
        }

        if (hintUI != null)
        {
            hintUI.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("CoralPickupF：没有找到提示 UI（HintCanvas）子物体！", this);
        }
    }

    private void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        bool shouldShow = dist <= showDistance;
        bool canPickup = dist <= pickupDistance;

        // 控制提示 UI 显隐
        if (hintUI != null)
        {
            if (shouldShow != hintUI.gameObject.activeSelf)
            {
                hintUI.gameObject.SetActive(shouldShow);
                Debug.Log("提示 UI 显示状态为: " + shouldShow + ", 距离: " + dist.ToString("F2"));
            }
        }

        // 按 F 拾取
        if (canPickup && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("按 F 拾取珊瑚：" + data.itemName);

            // 加入背包
            PackageData.Instance.AddItem(data);

            PackagePanel panel = FindObjectOfType<PackagePanel>();
            if (panel != null) panel.RefreshScroll();

            Destroy(gameObject);
        }
    }
}