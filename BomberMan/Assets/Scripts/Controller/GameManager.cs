using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("Grid size Should odd order")]
    [SerializeField]
    private Vector2Int _gridSize;

    public Transform transformPlayer;
    public GameObject playerObject;
    private float timeLoading = 5f;
    public GameObject levelCanvas;

    [SerializeField]
    private GameObject brickPrefab;
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private GameObject doorPrefab;

    public CinemachineVirtualCamera camFollowPlayer;

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
        if (_gridSize == Vector2Int.zero)
        {
            _gridSize = new Vector2Int(Random.Range(10, 25), Random.Range(10, 25));
        }
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
       // camFollowPlayer = GetComponent<CinemachineVirtualCamera>();
    }
    public void Update()
    {
        if (timeLoading <= 0)
        {
            playerObject = GameObject.FindWithTag("Player");
            transformPlayer = playerObject.transform;
            if (transformPlayer != null && camCount == 0)
            {
                camCount = 1;
                Debug.Log("run");
                camFollowPlayer.m_Follow = transformPlayer;
                levelCanvas.SetActive(false);
            }

        }
        timeLoading -= Time.deltaTime;
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
