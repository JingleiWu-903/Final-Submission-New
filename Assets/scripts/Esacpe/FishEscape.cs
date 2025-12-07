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
        // Initial state: pinned (play struggling animation Attack)
        animator.SetBool("IsFree", false);
    }

    public void StartEscape()
    {
        Debug.Log("Fish StartEscape() is Uesd");
        if (hasEscaped) return;
        hasEscaped = true;
        animator.SetBool("IsFree", true);
        Debug.Log("Fish IsFree is set to true");
        StartCoroutine(EscapeRoutine());
    }


    private IEnumerator EscapeRoutine()
    {
        // Step 1：upstream
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + Vector3.up * escapeUpDistance;

        float t = 0;
        while (t < escapeUpDuration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, t / escapeUpDuration);
            yield return null;
        }

        // Step 2：Fast walking (random direction)
        Vector3 dir = Random.onUnitSphere;
        dir.y = Mathf.Abs(dir.y); // Make sure to move upwards or horizontally
        dir.Normalize();

        float timer = 0;
        while (timer < escapeTime)
        {
            timer += Time.deltaTime;
            transform.position += dir * escapeSpeed * Time.deltaTime;
            yield return null;
        }

        // Step 3：disappear after escaping
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
