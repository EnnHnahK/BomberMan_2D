using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public Button[] levelButtons;
    public TextMeshProUGUI[] textButtons;
    void Start()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            textButtons[i].text = levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().text;
        }
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached) { 
                levelButtons[i].interactable = false;
                textButtons[i].text = "LOCKED";
                textButtons[i].color = Color.red;
            }

        }
    }
    public void Select(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

}
