using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("Grid size Should odd order")]
    [SerializeField]
    private Vector2Int _gridSize;

    [SerializeField]
    private GameObject brickPrefab;
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private GameObject doorPrefab;

    public uint enemyCount;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject playerPrefab;

    public System.Action restartGame;
    public System.Action scoreUpdate;
    public System.Action<bool> gameStatus;
    public GameController gameController;

    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }


    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
        {
            _instance = this; 
        }
    }
    void Start()
    {
        ServiceLocator.InitializeContainer();
        GameReady();
        ServiceLocator.GetService<IMapGenerator>().InitializeLevelController(_gridSize, brickPrefab, blockPrefab, doorPrefab);
        ServiceLocator.GetService<IPlayerSpawner>().InitializePlayerSpawner(playerPrefab);
        ServiceLocator.GetService<IEnemySpawner>().InitializeEnemySpawner(enemyPrefab, enemyCount);
    }
    public void GameReady()
    {
        StartCoroutine(gameController.Level(ServiceLocator.GetService<IMapGenerator>().GenerateMap));
        
    }
    public void RestartGame()
    {
        /*StartCoroutine(uIManager.LevelIntro(() =>
        {
            restartGame?.Invoke();
            ServiceLocator.GetService<IGridGenerator>().GenerateGrid();
        }));*/
    }
    public void GameStatus(bool isWon)
    {
        gameStatus?.Invoke(isWon);
    }
    public void UpdateScore()
    {
        scoreUpdate?.Invoke();
    }
}
