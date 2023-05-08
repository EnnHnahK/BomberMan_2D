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
        PlayerPrefs.DeleteAll();
    }

}
