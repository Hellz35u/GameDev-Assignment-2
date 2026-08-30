using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private int attackIndex = 0;
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
    public void PlayAttack()
    {
        attackIndex++;
        if(attackIndex > 3)
        {
            attackIndex = 1;
        }
        animator.SetInteger(nameof(AnimationParameters.AttackIndex), attackIndex);
        animator.SetTrigger(nameof(AnimationParameters.Attack));
    }
    public void PlayDeath()
    {
        animator.SetTrigger(nameof(AnimationParameters.Death));
    }
    public void PlayHit()
    {
        animator.SetTrigger(nameof(AnimationParameters.TakeHit));
    }
    public void SetGrounded(bool isGrounded)
    {
        animator.SetBool(nameof(AnimationParameters.IsGrounded), isGrounded);
    }
    public void SetVerticalVelocity(float velocityY)
    {
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
        AttackIndex,
        Speed,
        IsGrounded,
        Death,
        TakeHit
    }    
}
