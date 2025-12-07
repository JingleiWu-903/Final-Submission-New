using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject tip_I;      // TIP-"press I” 
    public GameObject tip_Tab;    // TIP-“press Tab”

    [Header("References")]
    public InstructionPanelController instructionPanel;
    public PackageInput packageInput;

    private bool hasOpenedInstruction = false;   // Whether pressed I for the first time
    private bool hasFinishedInstruction = false; // Has the introduction panel been closed yet
    private bool hasOpenedBag = false;           // Whether press Tab

    void Start()
    {
        tip_I.SetActive(true);
        tip_Tab.SetActive(false);
    }

    void Update()
    {
        // presses "I" Open/Close the introduction panel // can be triggered unlimited times
        if (Input.GetKeyDown(KeyCode.I))
        {
            instructionPanel.ToggleInstruction();

            // Press " I " for the first time to hide the prompt
            if (!hasOpenedInstruction)
            {
                tip_I.SetActive(false);
                hasOpenedInstruction = true;
            }

            return;
        }

        // Show "press tab " After the intro panel is closed
        if (hasOpenedInstruction && !instructionPanel.IsOpen && !hasFinishedInstruction)
        {
            hasFinishedInstruction = true;
            tip_Tab.SetActive(true);
        }

        // "press'tab：opens backpack
        if (!hasOpenedBag && Input.GetKeyDown(KeyCode.Tab))
        {
            if (packageInput != null)
            {
                hasOpenedBag = true;
                tip_Tab.SetActive(false);
            }
        }
    }
}
