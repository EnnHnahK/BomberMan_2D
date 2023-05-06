using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public interface IMapGenerator
{
    GameObject door { get; }
    GameObject[,] mapCells { get; set; }
    void InitializeLevelController(Vector2Int mapSize, GameObject _brick, GameObject _block, GameObject _doorPrefab, GameObject _floorPrefab);
    void GenerateMap();
}
public class MapGenerator : IMapGenerator
{
    private GameObject brickPrefab;
    private GameObject blockPrefab;
    private GameObject doorPrefab;
    private GameObject floorPrefab;
    private int mapWidth = 0;
    private int mapHeight = 0;
    private const int edgeValue = 2;
    private List<Vector2> emptyCells;
    private GameObject mapParent;
    public GameObject[,] mapCells { get; set; }
    public GameObject door { get; private set; }


    IPlayerSpawner playerSpawner;
    IEnemySpawner enemySpawner;

    ~MapGenerator()
    {
        GameManager.instance.restartGame -= resetMap;
    }
    public MapGenerator()
    {
        GameManager.instance.restartGame += resetMap;
    }

    public void GenerateMap()
    {
        mapParent = new GameObject();
        mapParent.transform.position = Vector3.zero;
        mapParent.name = "MapParent";
        InitializeMap();
        GenerateBorder();
        GenerateFloor();
        GenerateBlocks();
        SpawnPlayer();
        GenerateBricks();
        SpawnEnemies();
        ServiceLocator.GetService<IMapProcessing>().InitializeGridHandler();
    }
    public void InitializeLevelController(Vector2Int mapSize, GameObject _brickPrefab, GameObject _blockPrefab, GameObject _doorPrefab, GameObject _floorPrefab)
    {
        mapWidth = mapSize.x;
        mapHeight = mapSize.y;
        brickPrefab = _brickPrefab;
        blockPrefab = _blockPrefab;
        floorPrefab = _floorPrefab;
        playerSpawner = ServiceLocator.GetService<IPlayerSpawner>();
        enemySpawner = ServiceLocator.GetService<IEnemySpawner>();
        doorPrefab = _doorPrefab;
    }

    public void resetMap()
    {
        for (int i = 0; i < mapWidth + edgeValue; i++)
        {
            for (int j = 0; j < mapHeight + edgeValue; j++)
            {
                if (mapCells[i, j])
                {
                    GameObject gameObject = mapCells[i, j];
                    MonoBehaviour.DestroyImmediate(gameObject);
                    mapCells[i, j] = null;
                }
            }
        }
        mapCells = null;
        MonoBehaviour.Destroy(mapParent);
    }

    private void InitializeMap()
    {
        mapCells = new GameObject[mapWidth + edgeValue, mapHeight + edgeValue];
        emptyCells = new List<Vector2>();
        for (int i = 0; i < mapWidth + edgeValue; i++)
        {
            for (int j = 0; j < mapHeight + edgeValue; j++)
            {
                Vector2Int cellposition = new Vector2Int(i, j);
                emptyCells.Add(cellposition);
            }
        }
    }

    private void GenerateBorder()
    {
        for (int i = 0; i < mapWidth + edgeValue; i++)
        {
            DrawEdge(new Vector2Int(i, 0));
            DrawEdge(new Vector2Int(i, mapHeight + edgeValue - 1));
        }
        for (int j = 0; j < mapHeight + edgeValue; j++)
        {
            DrawEdge(new Vector2Int(0, j));
            DrawEdge(new Vector2Int(mapWidth + edgeValue - 1, j));
        }
    }

    private void DrawEdge(Vector2Int pos)
    {
        mapCells[pos.x, pos.y] = MonoBehaviour.Instantiate(blockPrefab, new Vector3(pos.x, pos.y), Quaternion.identity, mapParent.transform);
        emptyCells.Remove(pos);
    }

    private void GenerateBlocks()
    {
        for (int i = 2; i < mapWidth; i += 2)
        {
            for (int j = 2; j < mapHeight; j += 2)
            {
                Vector2 cellPosition = new Vector2(i, j);
                mapCells[i, j] = MonoBehaviour.Instantiate(blockPrefab, cellPosition, Quaternion.identity, mapParent.transform);
                emptyCells.Remove(cellPosition);
            }
        }
    }
    private void GenerateFloor(){ 
        for(int i = 1; i <= mapWidth; i++)
        {
            for(int j = 1; j <= mapHeight; j++)
            {
                Vector2 cellPosition = new Vector2(i, j);
                mapCells[i, j] = MonoBehaviour.Instantiate(floorPrefab, cellPosition, Quaternion.identity, mapParent.transform);
                //emptyCells.Remove(cellPosition);
            }
        }
    }
    private void SpawnPlayer()
    {
        Vector2 playerSpawnPos = new Vector2(1, mapHeight);
        ServiceLocator.GetService<IPlayerSpawner>().SpawnPlayer(playerSpawnPos);
        emptyCells.Remove(playerSpawnPos);
        emptyCells.Remove(new Vector2(1, mapHeight - 1));
        emptyCells.Remove(new Vector2(2, mapHeight));
    }

    private void GenerateBricks()
    {
        int minValue = (int)((emptyCells.Count / 100f) * 30);
        int maxValue = (int)((emptyCells.Count / 100f) * 40);
        int brickCount = Random.Range(minValue, maxValue);
        //int num = Random.Range(0, emptyCells.Count);
        for (int i = 0; i < brickCount; i++)
        {
            int cellNumber = Random.Range(0, emptyCells.Count);
            Vector2Int vectorIndex = Vector2Int.FloorToInt(emptyCells[cellNumber]);
            Debug.Log(" " + vectorIndex);
            GameObject brickGameobject = mapCells[vectorIndex.x, vectorIndex.y] = MonoBehaviour.Instantiate(brickPrefab, emptyCells[cellNumber], Quaternion.identity, mapParent.transform);
            if (i == brickCount - 1)
                door = MonoBehaviour.Instantiate(doorPrefab, brickGameobject.transform.position, Quaternion.identity, mapParent.transform);
            emptyCells.Remove(vectorIndex);
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < GameManager.instance.enemyCount; i++)
        {
            int num = Random.Range(0, emptyCells.Count);
            Vector2 spawnPos = emptyCells[num];
            enemySpawner.SpawnEnemy(spawnPos);
            emptyCells.Remove(spawnPos);
        }
    }
}
