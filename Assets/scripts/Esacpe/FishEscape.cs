using UnityEngine;
using System.Collections;

public class FishEscape : MonoBehaviour
{
    public Animator animator;

    public float escapeUpDistance = 1.2f;
    public float escapeUpDuration = 0.6f;
    public float escapeSpeed = 3f;
    public float escapeTime = 2f;

    private bool hasEscaped = false;

    void Start()
    {
        // 初始状态 = 被压住（播放挣扎动画 Attack）
        animator.SetBool("IsFree", false);
    }

    public void StartEscape()
    {
        Debug.Log("🐟 StartEscape() 被调用！！");

        if (hasEscaped) return;

        hasEscaped = true;
        animator.SetBool("IsFree", true);

        Debug.Log("🐟 IsFree 已设置为 true");

        StartCoroutine(EscapeRoutine());
    }


    private IEnumerator EscapeRoutine()
    {
        // Step 1：向上游
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + Vector3.up * escapeUpDistance;

        float t = 0;
        while (t < escapeUpDuration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, t / escapeUpDuration);
            yield return null;
        }

        // Step 2：快速游走（随机方向）
        Vector3 dir = Random.onUnitSphere;
        dir.y = Mathf.Abs(dir.y); // 确保向上或平移
        dir.Normalize();

        float timer = 0;
        while (timer < escapeTime)
        {
            timer += Time.deltaTime;
            transform.position += dir * escapeSpeed * Time.deltaTime;
            yield return null;
        }

        // Step 3：逃跑后消失
        Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exit: " + other.name);

        FishEscape fish = other.GetComponent<FishEscape>();
        if (fish != null)
        {
            Debug.Log("Fish Escape triggered!");
            fish.StartEscape();
        }
    }

}
