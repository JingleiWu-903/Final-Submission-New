using UnityEngine;

public class CoralPlantSlot : MonoBehaviour
{
    [Header("两个子物体（灰色 + 正常珊瑚）")]
    public GameObject greyCoral;        // 占位灰珊瑚
    public GameObject realCoral;        // 种植后的正常珊瑚

    [HideInInspector] public bool isPlanted = false;

    private void Awake()
    {
        if (greyCoral == null)
        {
            Transform t = transform.Find("GreyCoral");
            if (t != null) greyCoral = t.gameObject;
        }

        if (realCoral == null)
        {
            Transform t = transform.Find("RealCoral");
            if (t != null) realCoral = t.gameObject;
        }
    }

    private void Start()
    {
        RefreshVisual();
    }

    public void SetPlanted(bool planted)
    {
        isPlanted = planted;
        RefreshVisual();
    }

    private void RefreshVisual()
    {
        if (greyCoral != null) greyCoral.SetActive(!isPlanted);
        if (realCoral != null) realCoral.SetActive(isPlanted);
    }
}