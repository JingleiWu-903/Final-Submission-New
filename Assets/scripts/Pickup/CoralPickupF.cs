using System.Collections;
using UnityEngine;


public class CoralPickupF : MonoBehaviour
{
    public ItemData data;              // 这个珊瑚对应的 ItemData（如 CoralBlue）

    [Header("提示 & 拾取距离")]
    public float showDistance = 5f;    // 显示 F 提示的距离
    public float pickupDistance = 5f;  // 能按 F 拾取的距离

    [Header("悬浮 & 旋转效果")]
    public float floatAmplitude = 0.2f; // 上下浮动高度
    public float floatSpeed = 2f;       // 浮动速度
    public float rotateSpeed = 60f;     // 旋转速度（度/秒）

    [Header("吸入效果")]
    public float absorbTime = 0.4f;     // 吸入持续时间
    public float targetHeight = 1.2f;   // 吸到玩家身边的高度（相对玩家）

    private Transform player;
    private float baseY;
    private bool isPickingUp = false;   // 是否正在吸入中，避免重复触发

    private void Start()
    {
        // 找玩家（Tag = Player）
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("CoralPickupF：找不到 Tag = Player 的玩家对象！");
        }

        // 记录初始高度，让它在这个高度附近上下浮动
        baseY = transform.position.y;

        // 检测提示 UI 单例
        if (PickupHintUI.Instance != null)
        {
            Debug.Log("CoralPickupF 已检测到提示 UI: " + PickupHintUI.Instance.name);
            PickupHintUI.Instance.SetVisible(false);
        }
        else
        {
            Debug.LogWarning("CoralPickupF：场景中没有挂 PickupHintUI 的提示面板！");
        }
    }

    private void Update()
    {
        if (player == null) return;

        // -------- 悬浮 + 旋转特效 --------
        float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        Vector3 pos = transform.position;
        pos.y = baseY + offsetY;
        transform.position = pos;

        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);

        // 吸入过程中，不再理会 UI 和按键
        if (isPickingUp) return;

        // -------- 计算距离，控制 UI & 拾取 --------
        float dist = Vector3.Distance(transform.position, player.position);
        bool inShowRange = dist <= showDistance;     // 可以显示 F 提示
        bool inPickupRange = dist <= pickupDistance; // 可以按 F 拾取

        // 控制提示 UI
        if (PickupHintUI.Instance != null)
        {
            // 告诉 UI 现在要跟随的是这个珊瑚
            PickupHintUI.Instance.Follow(transform);
            // 是否显示
            PickupHintUI.Instance.SetVisible(inShowRange);
        }

        // 在可拾取范围内按 F
        if (inPickupRange && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(PickupCoroutine());
        }
    }

    private IEnumerator PickupCoroutine()
    {
        isPickingUp = true;

        if (PickupHintUI.Instance != null)
            PickupHintUI.Instance.SetVisible(false);

        // 禁用碰撞，避免过程中再触发其它碰撞
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Vector3 startPos = transform.position;
        Vector3 endPos = player.position + Vector3.up * targetHeight;

        float t = 0f;
        while (t < absorbTime)
        {
            t += Time.deltaTime;
            float p = t / absorbTime;
            p = p * p;  // 稍微加速一点的插值

            transform.position = Vector3.Lerp(startPos, endPos, p);
            yield return null;
        }

        // -------- 正式加入背包 --------
        PackageData.Instance.AddItem(data);

        PackagePanel panel = FindObjectOfType<PackagePanel>();
        if (panel != null)
        {
            panel.RefreshScroll();
        }

        Debug.Log("按 F 拾取珊瑚进入背包：" + data.itemName);

        Destroy(gameObject);
    }
}