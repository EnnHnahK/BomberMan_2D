using UnityEngine;

public class Item : MonoBehaviour
{
    public enum itemType
    {
        ExtraBomb,
        IncreaseExplosion,
        SpeedUp
    }

    public itemType type;
    public float despawnTime = 10f;

    void Update()
    {
        if(despawnTime <= 0)
        {
            Destroy(gameObject);
        }

        despawnTime -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("BOT"))
        {
            itemPickUp(collision.gameObject);
        }
    }

    private void itemPickUp(GameObject character)
    {
        switch (type)
        {
            case itemType.ExtraBomb:
                {
                    Debug.Log("Bombs placed by character is increased by 1");
                    break;
                }
            case itemType.IncreaseExplosion:
                {
                    Debug.Log("Bombs placed by character are given an explosion boost");
                    break;
                }
            case itemType.SpeedUp:
                {
                    Debug.Log("Character has been sped up");
                    break;
                }
        }
        Destroy(gameObject);
    }
}
