using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PackagePanel : MonoBehaviour
{
    [Header("Detail Pages")]
    public GameObject infoPage;
    public GameObject videoPage;

    public UnityEngine.Video.VideoPlayer videoPlayer;
    public UnityEngine.UI.RawImage videoRawImage;

    [Header("Scroll Area")]
    public Transform content;
    public GameObject itemPrefab;

    [Header("Detail Area")]
    public GameObject detailPanel;
    public Image detailIcon;
    public TextMeshProUGUI detailName;
    public TextMeshProUGUI detailDescription;

    [Header("Detail Buttons")]
    public Button infoButton;
    public Button videoButton;
    public Button deleteButton;
    public Button closeButton;

    private ItemData currentItem;

    private void OnEnable()
    {
        RefreshScroll();
    }

    private void Start()
    {
        RefreshScroll();
        detailPanel.SetActive(false);

        infoButton.onClick.AddListener(OnClickInfo);
        videoButton.onClick.AddListener(OnClickVideo);
        deleteButton.onClick.AddListener(DeleteCurrentItem);

        if (closeButton != null)
            closeButton.onClick.AddListener(() => detailPanel.SetActive(false));
    }

    public void RefreshScroll()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        foreach (ItemData item in PackageData.Instance.items)
        {
            GameObject cell = Instantiate(itemPrefab, content);
            cell.GetComponent<PackageCell>().Setup(item, this);
        }
    }

    public void ShowDetail(ItemData item)
    {
        currentItem = item;

        detailIcon.sprite = item.icon;
        detailName.text = item.itemName;
        detailDescription.text = item.description;

        detailPanel.SetActive(true);

        infoPage.SetActive(true);
        videoPage.SetActive(false);

        if (videoPlayer != null)
            videoPlayer.Stop();
    }

    // --------------------------
    // 删除（丢弃）物品
    // --------------------------
    public void DeleteCurrentItem()
    {
        if (currentItem == null) return;

        PackageData.Instance.RemoveItem(currentItem);

        Transform player = GameObject.FindWithTag("Player").transform;
        Vector3 spawn = player.position + player.forward * 2 + Vector3.up * 2f;

        Debug.Log("<color=yellow>丢弃生成位置：" + spawn + "</color>");

        if (currentItem.worldPrefab != null)
        {
            GameObject obj = Instantiate(currentItem.worldPrefab, spawn, Quaternion.identity);

            Debug.Log("<color=green>生成了物体：" + obj.name + "</color>");

            if (obj.GetComponent<ItemPickup>() == null)
                obj.AddComponent<ItemPickup>().data = currentItem;

            if (obj.GetComponent<Collider>() == null)
                obj.AddComponent<BoxCollider>();

            if (obj.GetComponent<Rigidbody>() == null)
                obj.AddComponent<Rigidbody>().useGravity = true;
        }

        RefreshScroll();
        detailPanel.SetActive(false);
    }

    public void TogglePanel()
    {
        bool newState = !gameObject.activeSelf;
        gameObject.SetActive(newState);

        if (newState)
            CursorManager.Instance.UnlockCursor();

        else
            CursorManager.Instance.LockCursor();
    }

    private void OnClickInfo()
    {
        infoPage.SetActive(true);
        videoPage.SetActive(false);

        if (videoPlayer != null)
            videoPlayer.Stop();
    }

    private void OnClickVideo()
    {
        infoPage.SetActive(false);
        videoPage.SetActive(true);

        if (currentItem != null && videoPlayer != null)
        {
            videoPlayer.url = currentItem.videoPath;
            videoPlayer.Play();
        }
    }
}
