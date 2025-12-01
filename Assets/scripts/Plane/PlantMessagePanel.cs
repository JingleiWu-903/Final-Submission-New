using System.Collections;
using UnityEngine;
using TMPro;

public class PlantMessagePanel : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI messageText;
    public float showTime = 1.5f;
    public float fadeTime = 0.5f;

    private Coroutine current;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        // 确保物体是激活的，用 alpha 控制可见性
        gameObject.SetActive(true);
        HideInstant();
    }

    public void HideInstant()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void ShowMessage(string msg)
    {
        if (canvasGroup == null) return;

        // 确保物体是激活状态
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        if (current != null)
            StopCoroutine(current);

        current = StartCoroutine(ShowRoutine(msg));
    }

    private IEnumerator ShowRoutine(string msg)
    {
        if (messageText != null)
            messageText.text = msg;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // 渐显
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeTime);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // 停留一段时间
        yield return new WaitForSeconds(showTime);

        // 渐隐
        t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeTime);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}