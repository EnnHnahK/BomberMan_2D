using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playSandBox()
    {
        SceneManager.LoadScene("TestScene");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Delete()
    {
        Debug.Log(PlayerPrefs.GetFloat("levelReached"));
        PlayerPrefs.DeleteAll();
        Debug.Log(PlayerPrefs.GetFloat("levelReached"));
    }
    public void devNoti()
    {
        SceneManager.LoadScene("Dev");
    }
    public void backMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
