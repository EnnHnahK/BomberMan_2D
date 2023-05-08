using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIController : MonoBehaviour
{
    private int score = 0;

    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI bombText;
    private int bombAmount;


    void Start()
    {
        GameManager.instance.scoreUpdate += UpdateScore;
    }

    void UpdateScore()
    {
        score += 100;
        scoreText.text = "Score:" + score.ToString();
    }

    void UpdateBomb()
    {
        GameObject gameObject = GameObject.FindWithTag("Player");
        if(gameObject != null) { 
            bombAmount = gameObject.GetComponent<BombController>().bombsRemaining;
            bombText.text = "" + bombAmount;
        }
    }
    private void Update()
    {
        UpdateBomb();
    }
    public void backMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
