using Cinemachine;
using UnityEngine;
public interface IPlayerSpawner
{
    void InitializePlayerSpawner(GameObject _playerPrefab);
    void SpawnPlayer(Vector3 _position);
    void ResetPlayer();
}
public class PlayerSpawner : IPlayerSpawner
{
    private GameObject playerPrefab;
    private GameObject playerRef;
    public CinemachineVirtualCamera myCinemachine;

    ~PlayerSpawner()
    {
        GameManager.instance.restartGame -= ResetPlayer;
    }
    public PlayerSpawner()
    {
        GameManager.instance.restartGame += ResetPlayer;
    }
    public void InitializePlayerSpawner(GameObject _playerPrefab)
    {
        playerPrefab = _playerPrefab;
    }

    public void ResetPlayer()
    {
        MonoBehaviour.Destroy(playerRef);
    }

    public void SpawnPlayer(Vector3 _position)
    {
        playerRef = MonoBehaviour.Instantiate(playerPrefab, _position, Quaternion.identity);
    }
}
