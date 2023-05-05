using UnityEngine;

public class Brick : MonoBehaviour , IDamage
{
    private IMapProcessing mapProcessing;
    private void Start()
    {
        mapProcessing = ServiceLocator.GetService<IMapProcessing>();
    }
    public void Damage()
    {
        mapProcessing.EmptyGrid(new Vector2Int((int)transform.position.x, (int)transform.position.y));
        Destroy(gameObject);
    }
}
