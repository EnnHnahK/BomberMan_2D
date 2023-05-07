using TMPro;
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
                    character.GetComponent<BombController>().AddBomb();
                    break;
                }
            case itemType.IncreaseExplosion:
                {
                    character.GetComponent<BombController>().explosionRadius++;
                    break;
                }
            case itemType.SpeedUp:
                {
                    character.GetComponent<PlayerController>().moveSpeed++;   
                    break;
                }
        }
        Destroy(gameObject);
    }
}
