using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMangerBack : MonoBehaviour
{
       public void ChangeSence(string name){
        SceneManager.LoadScene(name);
}
}