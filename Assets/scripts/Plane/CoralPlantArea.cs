using UnityEngine;

public class CoralPlantArea : MonoBehaviour
{
    [Header("Coral required for consumption ItemData")]
    public ItemData coralItem;          // CoralBlue

    [Header("Buttons & Distance")]
    public float plantDistance = 5f;    // How close to a certain square can a player be to plant
    public float areaRadius = 12f;      // How close to the entire area does a player need to be to be considered "within the area"
    public KeyCode plantKey = KeyCode.P;

    [Header("UI reference")]
    public PlantAreaUIView areaUI;      // A large panel (PlantAreaUI) with progress and arrows
    public PlantHintUI plantHintUI;     // Plant Hint Panel (PlantHintPanel)
    public PlantMessagePanel messageUI; // The "No Coral" section at the top (PlantMessagePanel)
    public GameObject winPanel;         // WinPanel

    [Header("All planting positions (in sequence)")]
    public CoralPlantSlot[] slots;

    private Transform player;
    private int plantedCount = 0;

    private void Start()
    {
        // Find players
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("[CoralPlantArea] The player object with Tag = Player cannot be found!");
        }

        // Initialize the grid
        plantedCount = 0;
        if (slots != null)
        {
            foreach (var s in slots)
            {
                if (s == null) continue;
                s.SetArea(this);
                if (s.isPlanted) plantedCount++;
            }
        }

        // Initialize progress UI
        if (areaUI != null)
        {
            areaUI.SetTarget(transform);                   // Let it follow the center of the entire area
            areaUI.SetProgress(plantedCount, slots.Length);
            areaUI.SetVisible(true);
        }

        // P Prompt to hide first
        if (plantHintUI != null)
        {
            plantHintUI.SetVisible(false);
        }

        // The top information panel is hidden first
        if (messageUI != null)
        {
            messageUI.HideInstant();
        }

        //WinPanel is closed
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Determine whether the player is "near the area"
        float distToArea = Vector3.Distance(player.position, transform.position);
        bool nearArea = distToArea <= areaRadius;

        //Find the nearest "unplanted" slot (without distance restrictions, to determine where the next one should be planted)
        CoralPlantSlot closest = null;
        float closestDist = float.MaxValue;

        foreach (var s in slots)
        {
            if (s == null || s.isPlanted) continue;

            float d = Vector3.Distance(player.position, s.transform.position);
            if (d < closestDist)
            {
                closestDist = d;
                closest = s;
            }
        }

        // ③Update the P prompt UI: It must be within the area and the nearest grid's distance to the player should not exceed plantDistance
        bool canShowHint = nearArea && closest != null && closestDist <= plantDistance;

        if (plantHintUI != null)
        {
            if (canShowHint)
            {
                plantHintUI.SetTarget(closest.transform);
                plantHintUI.SetVisible(true);
            }
            else
            {
                plantHintUI.SetVisible(false);
            }
        }

        // ④ Press key P to handle
        if (Input.GetKeyDown(plantKey))
        {
            //Player is too far from the area: Do nothing
            if (!nearArea)
                return;

            // There is no coral in the backpack
            if (!PackageData.Instance.HasItem(coralItem))
            {
                if (messageUI != null)
                {
                    string msg = (plantedCount == 0)
                        ? "There are no corals in your backpack!"
                        : "There are no corals in your backpack anymore!";

                    messageUI.ShowMessage(msg);
                }
                return;
            }

            // Consume 1 coral and plant it in the nearest cell
            if (PackageData.Instance.ConsumeItem(coralItem))
            {
                closest.Plant();
            }
        }
    }

    //  Called by CoralPlantSlot: One has been planted
    public void NotifyPlanted(CoralPlantSlot slot)
    {
        plantedCount++;

        if (areaUI != null)
        {
            areaUI.SetProgress(plantedCount, slots.Length);
        }

        Debug.Log($"[CoralPlantArea] Planted quantity: {plantedCount}/{slots.Length}");

        // All planted
        if (plantedCount >= slots.Length)
        {
            // Turn off P prompt and progress UI
            if (plantHintUI != null)
                plantHintUI.SetVisible(false);

            if (areaUI != null)
                areaUI.SetVisible(false);

            // Pop up the victory panel
            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }
            else
            {
                Debug.LogWarning("[CoralPlantArea]The WinPanel has no reference dragged in the Inspector!");
            }

            // Please provide the text you would like translated
            if (messageUI != null)
            {
                messageUI.ShowMessage("You have repaired this coral reef!");
            }

            Debug.Log("[CoralPlantArea] All corals have been planted. Victory!");
        }
    }
}