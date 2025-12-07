using UnityEngine;

public class NetTrap : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit is triggered：" + other.name);

        FishEscape fish = other.GetComponent<FishEscape>();
        if (fish != null)
        {
            Debug.Log("FishEscape is found and starts to escape");
            fish.StartEscape();
        }
        else
        {
            Debug.Log("OnTriggerExit triggered, but the object was not a fish：" + other.name);
        }
    }

}
