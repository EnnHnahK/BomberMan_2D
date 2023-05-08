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
    [SerializeField]
    private TextMeshProUGUI loadingText;
    [SerializeField]
    private TextMeshProUGUI pauseText;
    [SerializeField]
    private GameObject loading;
    [SerializeField]
    private GameObject loseButton;
    [SerializeField]
    private GameObject pauseLable;
    private bool pause = false;

    private int bombAmount;
    private float time = 5f;

    void Start()
    {
        GameManager.instance.scoreUpdate += UpdateScore;
        GameManager.instance.gameStatus += UpdateGameStatus;
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
    void UpdateGameStatus(bool isWon)
    {
        if (!isWon)
        {
            loseButton.SetActive(true);
        }
    }
    public void restartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Update()
    {
        UpdateBomb();
        if (time > 4f)
        {
            loadingText.text = "GameStatus: Map Initialization!";
        }
        else if (time > 3f && time < 4f)
        {
            loadingText.text = "GameStatus: Generator Map!";
        }
        else if (time > 2f && time < 3f)
        {
            loadingText.text = "GameStatus: Camera Initialization!";
        }
        else if (time > 2f && time < 3f)
        {
            loadingText.text = "GameStatus: Searching Player!";
        }
        else if (time > 1f && time < 2f)
        {
            loadingText.text = "GameStatus: Done!";
        }
        else if (time > 0f && time < 1f)
        {
            loadingText.text = "GameStatus: Thanks!";
        }
        else if (time < 0f)
        {
            loading.SetActive(false);
        }
        time -= Time.deltaTime;
    }
    public void backMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void checkPause()
    {
        if (pause)
        {
            Resume();
            pause =! pause;
        }
        else
        {
            pause = !pause;
            Pause();
        }
    }
    private void Pause()
    {
        pauseText.text = "Resume";
        pauseLable.SetActive(true);
        Time.timeScale = 0;
    }
    private void Resume()
    {
        pauseText.text = "Pause";
        pauseLable.SetActive(false);
        Time.timeScale = 1;
    }
}
