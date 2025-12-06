using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject tip_I;      // “按 I” 提示
    public GameObject tip_Tab;    // “按 Tab” 提示

    [Header("References")]
    public InstructionPanelController instructionPanel;
    public PackageInput packageInput;

    private bool hasOpenedInstruction = false;   // 是否第一次按过 I
    private bool hasFinishedInstruction = false; // 是否已经关闭过介绍 panel
    private bool hasOpenedBag = false;           // 是否按过 Tab

    void Start()
    {
        tip_I.SetActive(true);
        tip_Tab.SetActive(false);
    }

    void Update()
    {
        // ----------- 玩家按 I 键：打开/关闭介绍面板（无限次可触发） -----------
        if (Input.GetKeyDown(KeyCode.I))
        {
            instructionPanel.ToggleInstruction();

            // 第一次按 I 才隐藏提示
            if (!hasOpenedInstruction)
            {
                tip_I.SetActive(false);
                hasOpenedInstruction = true;
            }

            return;
        }

        // ----------- 玩家关闭介绍面板后：显示 Tab 提示（只触发一次） ----------
        if (hasOpenedInstruction && !instructionPanel.IsOpen && !hasFinishedInstruction)
        {
            hasFinishedInstruction = true;
            tip_Tab.SetActive(true);
        }

        // ----------- 玩家按 Tab：打开背包 ----------- 
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
