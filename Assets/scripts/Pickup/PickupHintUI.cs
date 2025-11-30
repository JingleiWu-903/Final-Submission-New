using UnityEngine;

public class PickupHintUI : MonoBehaviour
{
    public static PickupHintUI Instance;

    private void Awake()
    {
        // 做一个单例，方便别的脚本来拿这个 UI
        Instance = this;
        gameObject.SetActive(false);    // 一开始就隐藏
    }

    // 让别的脚本来控制显示/隐藏
    public void SetVisible(bool visible)
    {
        if (gameObject.activeSelf != visible)
        {
            gameObject.SetActive(visible);
            Debug.Log("提示 UI 显示状态改为: " + visible);
        }
    }
}