using System.Collections;
using UnityEngine;


public class CoralPickupF : MonoBehaviour
{
    public ItemData data;              // The ItemData corresponding to this coral

    [Header("Hint & Pickup Distance")]
    public float showDistance = 5f;    // The distance at which the F prompt is displayed
    public float pickupDistance = 5f;  // The distance at which items can be picked up by pressing F

    [Header("Suspended & Rotating Effects")]
    public float floatAmplitude = 0.2f; // Fluctuating height up and down
    public float floatSpeed = 2f;       // floating speed
    public float rotateSpeed = 60f;     // rotational speed

    [Header("Inhalation effect")]
    public float absorbTime = 0.4f;     // Inhalation duration
    public float targetHeight = 1.2f;   // The height that sucks the player close.

    private Transform player;
    private float baseY;
    private bool isPickingUp = false;   // Whether inhaling or not, avoid repeated triggering.

    private void Start()
    {
        // Find players（Tag = Player）
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("CoralPickupF：The player object with Tag = Player cannot be found!");
        }

        // Record the initial height and let it fluctuate up and down around this height
        baseY = transform.position.y;

        //Detection prompt UI singleton
        if (PickupHintUI.Instance != null)
        {
            Debug.Log("CoralPickupF The prompt UI has been detected: " + PickupHintUI.Instance.name);
            PickupHintUI.Instance.SetVisible(false);
        }
        else
        {
            Debug.LogWarning("CoralPickupF：There is no PickupHintUI prompt panel hung in the scene!");
        }
    }

    private void Update()
    {
        if (player == null) return;

        //Hover + Rotation Effects
        float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        Vector3 pos = transform.position;
        pos.y = baseY + offsetY;
        transform.position = pos;

        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);

        // During the inhalation process, ignore the UI and buttons
        if (isPickingUp) return;

        //Calculating distance, controlling UI & picking up
        float dist = Vector3.Distance(transform.position, player.position);
        bool inShowRange = dist <= showDistance;     // The F prompt can be displayed
        bool inPickupRange = dist <= pickupDistance; // Press F to pick up

        // Control prompt UI
        if (PickupHintUI.Instance != null)
        {
            // Tell the UI to follow this coral now
            PickupHintUI.Instance.Follow(transform);
            // Whether to display or not
            PickupHintUI.Instance.SetVisible(inShowRange);
        }

        // Press F within the pick-up range
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

        // Disable collisions to prevent triggering other collisions during the process
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Vector3 startPos = transform.position;
        Vector3 endPos = player.position + Vector3.up * targetHeight;

        float t = 0f;
        while (t < absorbTime)
        {
            t += Time.deltaTime;
            float p = t / absorbTime;
            p = p * p;  // Interpolation with a slightly increased speed

            transform.position = Vector3.Lerp(startPos, endPos, p);
            yield return null;
        }

        // Formally added to the backpack
        PackageData.Instance.AddItem(data);

        PackagePanel panel = FindObjectOfType<PackagePanel>();
        if (panel != null)
        {
            panel.RefreshScroll();
        }

        Debug.Log("Press F to pick up the coral and put it in your backpack:" + data.itemName);

        Destroy(gameObject);
    }
}