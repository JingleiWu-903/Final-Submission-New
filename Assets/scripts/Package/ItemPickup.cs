using System.Collections;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData data;  

    private bool canBePicked = false;
    public GameObject netTrapArea;

    private void Awake()
    {
        StartCoroutine(EnablePickupAfterDelay(0.3f));
    }

    private IEnumerator EnablePickupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canBePicked = true;
    }

    // Open up callable pickup functions to the public
    public void Pickup()
    {
        if (!canBePicked) return;

        Debug.Log("<color=yellow>Pickup() is called: " + gameObject.name + "</color>");
        Debug.Log("The Net instance being clicked is：" + gameObject.name, gameObject);

        // If it's the garbage plus energy section
        if (data.itemType == ItemData.ItemType.Trash)
        {
            var energySystem = FindFirstObjectByType<EnergySystem>();
            if (energySystem != null)
            {
                energySystem.AddEnergy(1);
            }
        }

        // Add to backpack
        PackageData.Instance.AddItem(data);

        // refresh UI
        var panel = FindFirstObjectByType<PackagePanel>();
        if (panel != null)
        {
            panel.RefreshScroll();
        }

        // move NetTrapArea firstly （Trigger OnTriggerExit）
        if (netTrapArea != null)
        {
            Debug.Log("Before move Trigger：" + netTrapArea.transform.position);
            netTrapArea.transform.position += Vector3.up * 10f;   // Raise the trigger area so that the fish triggers Exit
            Debug.Log("After move Trigger：" + netTrapArea.transform.position);
        }

        // Delayed hidden net (if hidden immediately, Trigger will disappear and Exit will not trigger)
        StartCoroutine(HideNetAfterDelay(0.1f));

        Debug.Log("<color=green>Picked up successfully：" + data.itemName + "</color>");
    }

    private IEnumerator HideNetAfterDelay(float t)
    {
        yield return new WaitForSeconds(t);
        gameObject.SetActive(false);
    }

    // Mouse click (automatically called by Unity)
    private void OnMouseDown()
    {
        Pickup(); // Click to call directly
    }
}
