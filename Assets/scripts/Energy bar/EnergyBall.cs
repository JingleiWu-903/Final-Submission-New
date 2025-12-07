using UnityEngine;


public class EnergyBall : MonoBehaviour
{
    public float lifeTime = 2f;

    // Dropped coral prefab
    public GameObject coralPrefab;
    // The ItemData of this coral in the backpack
    public ItemData coralItem;

    private void Start()
    {
        //The energy ball will self-destruct in 2 seconds
        Destroy(gameObject, lifeTime);
    }
    public Vector3 coralSpawnScale = new Vector3(0.25f, 0.25f, 0.25f); //Zooming in on the fallen coral
    private void OnCollisionEnter(Collision collision)
    {
        // Only handle LargeTrash, directly return for other collisions
        if (!collision.gameObject.CompareTag("LargeTrash"))
            return;

        Debug.Log("The energy ball hit the large piece of garbage：" + collision.gameObject.name);

        // Large garbage can be directly added to the backpack
        LargeTrashItem largeItem = collision.gameObject.GetComponent<LargeTrashItem>();
        if (largeItem != null && largeItem.data != null)
        {
            //Add to backpack
            PackageData.Instance.AddItem(largeItem.data);

            //Refresh the backpack UI
            var panel = FindObjectOfType<PackagePanel>();
            if (panel != null)
            {
                panel.RefreshScroll();
            }

            Debug.Log("Large garbage has been added to the backpack：" + largeItem.data.itemName);
        }

        //Dropped coral
        if (coralPrefab != null)
        {
            //A little above the collision point of the drop position.
            Vector3 hitPos;

            if (collision.contacts != null && collision.contacts.Length > 0)
            {
                hitPos = collision.contacts[0].point;
            }
            else
            {
                hitPos = collision.transform.position;
            }

            GameObject coral = Instantiate(
                coralPrefab,
                hitPos + Vector3.up * 0.5f,
                Quaternion.identity
            );

            Debug.Log("Drop collectible coral.：" + coral.name);
        }

        //Destroy the large garbage and energy balls
        Destroy(collision.gameObject);  // The huge garbage has vanished
        Destroy(gameObject);            // The energy ball vanished by itself
    }
}