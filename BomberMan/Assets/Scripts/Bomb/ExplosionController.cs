using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public AnimatorController start;
    public AnimatorController middle;
    public AnimatorController end;

    public void activeRenderer(AnimatorController renderer)
    {
        start.enabled = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled = renderer == end;
    }

    public void setDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void afterDestroy(float seconds)
    {
        Destroy(gameObject, seconds);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamage>() != null)
            collision.GetComponent<IDamage>().Damage();
    }

}

public interface IDamage
{
    void Damage();
}
