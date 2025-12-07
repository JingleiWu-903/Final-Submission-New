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

        // Make sure the object is active and use alpha to control its visibility
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

        // Make sure the object is in an active state
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

        //fade in
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeTime);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // Stay for a while
        yield return new WaitForSeconds(showTime);

        // fade-out
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