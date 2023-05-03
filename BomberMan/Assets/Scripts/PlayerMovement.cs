using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Vector2 direction = Vector2.down;

    public PlayerAnimator spriderRenderUp;
    public PlayerAnimator spriderRenderRight;
    public PlayerAnimator spriderRenderDown;
    public PlayerAnimator spriderRenderLeft;
    private PlayerAnimator activesprider;
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
    private void setDirection(Vector2 newDirection, PlayerAnimator spriteRender)
    {
        direction = newDirection;
        spriderRenderUp.enabled = spriteRender == spriderRenderUp;
        spriderRenderDown.enabled = spriteRender == spriderRenderDown;
        spriderRenderRight.enabled = spriteRender == spriderRenderRight;
        spriderRenderLeft.enabled = spriteRender == spriderRenderLeft;
        activesprider = spriteRender;
        activesprider.idle = direction == Vector2.zero;
    }
}
