using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void QuitGame() {
        Debug.Log("The game has quit!");
        Application.Quit();
    }

    public void ChangeSence(string name){
        SceneManager.LoadScene(name);
    }
}
