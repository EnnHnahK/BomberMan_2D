using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("Grid size Should odd order")]
    //[SerializeField]
    //private Vector2Int _gridSize = new Vector2Int(Random.Range(10,25), Random.Range(10, 25));
    //private Vector2Int _gridSize = new Vector2Int(25, 25);
    private Vector2Int _gridSize;

    public GameObject gameObject;

    [SerializeField]
    private GameObject brickPrefab;
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private GameObject doorPrefab;

    [SerializeField]
    private CinemachineVirtualCamera camFollowPlayer;

    private int camCount = 0;
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
        _gridSize = new Vector2Int(Random.Range(10, 25), Random.Range(10, 25));
        if (_instance != null)
            Destroy(gameObject);
        else
        {
            _instance = this; 
        }
        camFollowPlayer = GetComponent<CinemachineVirtualCamera>();
    }
    void Start()
    {
        ServiceLocator.InitializeContainer();
        GameReady();
        ServiceLocator.GetService<IMapGenerator>().InitializeLevelController(_gridSize, brickPrefab, blockPrefab, doorPrefab);
        ServiceLocator.GetService<IPlayerSpawner>().InitializePlayerSpawner(playerPrefab);
        ServiceLocator.GetService<IEnemySpawner>().InitializeEnemySpawner(enemyPrefab, enemyCount);
        
    }
    public void Update()
    {
        gameObject = GameObject.FindWithTag("Player");
        if(gameObject != null && camCount == 0)
        {
            camCount = 1;
            Debug.Log("run");
            camFollowPlayer.ResolveFollow(gameObject.transform);
        }
    }
    public void GameReady()
    {
        StartCoroutine(gameController.Level(ServiceLocator.GetService<IMapGenerator>().GenerateMap));
    }
    public void RestartGame()
    {
        StartCoroutine(gameController.Level(() =>
        {
            restartGame?.Invoke();
            ServiceLocator.GetService<IMapGenerator>().GenerateMap();
        }));
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
