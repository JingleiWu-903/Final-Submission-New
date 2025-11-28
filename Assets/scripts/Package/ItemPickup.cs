using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
    public ItemData data;

    private bool canBePicked = false;

    private void Awake()
    {
        // 刚生成时不能立即被拾取
        StartCoroutine(EnablePickupAfterDelay(0.3f));
    }

    private IEnumerator EnablePickupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canBePicked = true;
    }

    private void OnMouseDown()
    {
        if (!canBePicked) return;

        PackageData.Instance.AddItem(data);

        // 使用新版 API（不再出现黄色警告）
        PackagePanel panel = FindFirstObjectByType<PackagePanel>();
        if (panel != null)
            panel.RefreshScroll();

        gameObject.SetActive(false);

        Debug.Log("<color=green>拾取成功: " + data.itemName + "</color>");
    }
}
