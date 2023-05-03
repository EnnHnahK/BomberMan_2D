using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] animation;
    public Sprite IDsprite;

    public float animationTime = 0.25f;
    public int animationFrame;
    public bool loop = true;
    public bool idle = true;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }
    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }
    private void Start()
    {
        InvokeRepeating(nameof(NextFrame), animationTime, animationTime);
    }
    private void NextFrame()
    {
        animationFrame++;
        if (loop && animationFrame >= animation.Length)
        {

            animationFrame = 0;
        }
        if (idle)
        {
            spriteRenderer.sprite = IDsprite;
        }
        else if (animationFrame >= 0 && animationFrame < animation.Length)
        {
            spriteRenderer.sprite = animation[animationFrame];
        }
    }
}
