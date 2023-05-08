using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerController : MonoBehaviour,IDamage
{
    public float moveSpeed = 5.0f;
    public Vector2 direction = Vector2.down;
    public bool isAlive = true;

    public AnimatorController spriderRenderUp;
    public AnimatorController spriderRenderRight;
    public AnimatorController spriderRenderDown;
    public AnimatorController spriderRenderLeft;
    private AnimatorController activesprider;
    public new Rigidbody2D rigidbody { get; private set; }

    public void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activesprider = spriderRenderDown;  
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            setDirection(Vector2.left, spriderRenderLeft);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            setDirection(Vector2.right, spriderRenderRight);

        }
        else if (Input.GetKey(KeyCode.W))
        {
            setDirection(Vector2.up, spriderRenderUp);

        }
        else if (Input.GetKey(KeyCode.S))
        {
            setDirection(Vector2.down, spriderRenderDown);

        }
        else
        {
            setDirection(Vector2.zero, activesprider);
        }
    }
    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * moveSpeed * Time.fixedDeltaTime;
        rigidbody.MovePosition(position + translation);
    }
    // Huong
    private void setDirection(Vector2 newDirection, AnimatorController spriteRender)
    {
        direction = newDirection;
        spriderRenderUp.enabled = spriteRender == spriderRenderUp;
        spriderRenderDown.enabled = spriteRender == spriderRenderDown;
        spriderRenderRight.enabled = spriteRender == spriderRenderRight;
        spriderRenderLeft.enabled = spriteRender == spriderRenderLeft;
        activesprider = spriteRender;
        activesprider.idle = direction == Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyMovement>())
            Damage();
        else if (collision.gameObject.tag == "Door")
        {
            Debug.Log("Next Level");
            GameManager.instance.Win();
        }
    }
    public void Damage()
    {
        isAlive = false;
        GameManager.instance.GameStatus(false);
        Destroy(gameObject);
    }
  
}
