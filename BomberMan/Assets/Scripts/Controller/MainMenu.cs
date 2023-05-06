using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playSandBox()
    {
        SceneManager.LoadScene("TestScene");
    }

}
