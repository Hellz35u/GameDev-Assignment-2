using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetMovement(Vector2 direction)
    {
        animator.SetFloat(nameof(AnimationParameters.Speed), Mathf.Abs(direction.x));
        FlipSprite(direction.x);
    }
    public void PlayJump()
    {
        animator.SetTrigger(nameof(AnimationParameters.Jump));
    }
    public void SetGrounded(bool isGrounded)
    {
        animator.SetBool(nameof(AnimationParameters.IsGrounded), isGrounded);
    }
    public void SetVerticalVelocity(float velocityY)
    {
        Debug.Log("VelocityY = " + nameof(AnimationParameters.VelocityY));
        animator.SetFloat(nameof(AnimationParameters.VelocityY), velocityY);
    }
    private void FlipSprite(float directionX)
    {
        if (Mathf.Approximately(directionX, 0f))
            return;
        spriteRenderer.flipX = directionX < 0;
    }
    enum AnimationParameters
    {
        Jump,
        VelocityY,
        Attack,
        Speed,
        IsGrounded
    }    
}
