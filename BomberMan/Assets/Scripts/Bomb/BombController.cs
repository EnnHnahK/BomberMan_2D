using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    public KeyCode inputKey = KeyCode.Space;
    public GameObject bombPrefab;
    public float bombTime = 3f;
    public int bombAmount = 1;
    public int bombsRemaining;

    public ExplosionController explosionPrefab;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    private IMapProcessing mapProcessing;

    public GameObject destructibleObject;

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }

    private void Update()
    {
        if (bombsRemaining > 0 && Input.GetKeyDown(inputKey))
        {
            StartCoroutine(PlaceBomb());
        }
    }

    private IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;

        yield return new WaitForSeconds(bombTime);

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        ExplosionController explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.activeRenderer(explosion.start);
        explosion.afterDestroy(explosionDuration);

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        Destroy(bomb.gameObject);
        bombsRemaining++;
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0)
        {
            return;
        }

        position += direction;

        ExplosionController explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.activeRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.setDirection(direction);
        explosion.afterDestroy(explosionDuration);

        Explode(position, direction, length - 1);
    }

    private void clearZone(Vector2 position)
    {
        GameObject cell = mapProcessing.GetCellAtPosition(Vector2Int.FloorToInt(position));
        if (cell != null)
        {
            if (cell.GetComponent<Block>() != null)
                return;
            else
            {
                MonoBehaviour.Instantiate(explosionPrefab, position, Quaternion.identity);
                mapProcessing.EmptyGrid(Vector2Int.FloorToInt(position));
            }
        }
        else
        {
            MonoBehaviour.Instantiate(explosionPrefab, position, Quaternion.identity);
        }
    }

    public void AddBomb()
    {
        bombAmount++;
        bombsRemaining++;
    }



}
