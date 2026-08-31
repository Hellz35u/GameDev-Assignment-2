using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private SwordHitBox swordHitBox = null;
    private int attackIndex = 0;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        swordHitBox = GetComponentInChildren<SwordHitBox>();
    }
    private void Start()
    {
        DisableSwordHitBox();
    }
    private void EnableSwordHitBox()
    {
        Collider2D swordTrigger = swordHitBox.GetComponent<Collider2D>();
        swordTrigger.enabled = true;
    }

    private void DisableSwordHitBox()
    {
        Collider2D swordTrigger = swordHitBox.GetComponent<Collider2D>();
        swordHitBox.ClearMemory();
        swordTrigger.enabled = false;
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

        bool facingLeft = directionX < 0;

        spriteRenderer.flipX = facingLeft;

        Vector3 hitBoxPosition = swordHitBox.transform.localPosition;

        hitBoxPosition.x = Mathf.Abs(hitBoxPosition.x) * (facingLeft ? -1 : 1);

        swordHitBox.transform.localPosition = hitBoxPosition;
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
