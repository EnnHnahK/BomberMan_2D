using UnityEngine;

public class ItemsSpawner : MonoBehaviour
{
    public float despawmTime = 3f;

    [Range(0f, 1f)]
    public float itemSpawmRate = 0.3f;
    public GameObject[] itemSpawn;
    void Start()
    {
        //Destroy(gameObject, 1f);
    }
    private void OnDestroy()
    {
        if (gameObject.scene.isLoaded) { 
            if (itemSpawn.Length > 0 && Random.value < itemSpawmRate)
            {
                int randomIndex = Random.Range(0, itemSpawn.Length);
                Instantiate(itemSpawn[randomIndex], transform.position, Quaternion.identity);
            }
        }
    }
}
