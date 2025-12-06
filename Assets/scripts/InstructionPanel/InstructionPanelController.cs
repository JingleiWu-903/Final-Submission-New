using UnityEngine;
using UnityEngine.UI;

public class InstructionPanelController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject instructionPanel;

    [Header("Paging")]
    public Image imageHolder;
    public Sprite[] pages;

    [Header("Buttons")]
    public Button leftButton;
    public Button rightButton;

    private int currentPage = 0;
    private bool isOpen = false;

    public bool IsOpen => isOpen;


    void Awake()
    {
        // 面板默认关闭
        if (instructionPanel != null)
            instructionPanel.SetActive(false);
    }

    void OnEnable()
    {
        // 保证按钮监听永远绑定成功
        if (leftButton != null)
        {
            leftButton.onClick.RemoveAllListeners();
            leftButton.onClick.AddListener(PreviousPage);
        }

        if (rightButton != null)
        {
            rightButton.onClick.RemoveAllListeners();
            rightButton.onClick.AddListener(NextPage);
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            ToggleInstruction();
        }
    }

    public void ToggleInstruction()
    {

        isOpen = !isOpen;
        instructionPanel.SetActive(isOpen);

        if (isOpen)
        {
            CursorManager.Instance.UnlockCursor();

            // 每次打开时回到第一页
            currentPage = 0;
            ShowPage(currentPage);
            instructionPanel.SetActive(true);
        }
        else
        {
            CursorManager.Instance.LockCursor();
        }
    }

    public void NextPage()
    {
        if (pages == null || pages.Length == 0) return;

        currentPage++;
        if (currentPage >= pages.Length)
            currentPage = 0;

        ShowPage(currentPage);
    }

    public void PreviousPage()
    {
        if (pages == null || pages.Length == 0) return;

        currentPage--;
        if (currentPage < 0)
            currentPage = pages.Length - 1;

        ShowPage(currentPage);
    }

    private void ShowPage(int index)
    {
        if (pages == null || pages.Length == 0) return;

        imageHolder.sprite = pages[index];
        imageHolder.color = Color.white;

        Debug.Log("ShowPage " + index);
    }
}
