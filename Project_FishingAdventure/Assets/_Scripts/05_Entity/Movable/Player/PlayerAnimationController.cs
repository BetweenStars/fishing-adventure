using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Player player;
    public Animator animator { get; private set; }
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        player = GetComponentInParent<Player>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (animator.GetFloat("xDir") < 0) { spriteRenderer.flipX = true; }
        else{ spriteRenderer.flipX = false; }
    }
}
