using UnityEngine;

public class CharecterAnimation : MonoBehaviour
{
    private Animator animator = null;
    private SpriteRenderer spriteRenderer = null;
    private CharacterHitBox charecterHitBox = null;
    private Collider2D triggerHitBox = null;
    [SerializeField] private int maxAttackIndex = 3;
    private int attackIndex = 0;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if(animator == null)
        {
            Debug.LogError("the GameObject dont have Animator!");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("the GameObject dont have Sprite Renderer , thats wired!");
        }

        charecterHitBox = GetComponentInChildren<CharacterHitBox>();
        if(charecterHitBox != null)
        {
            triggerHitBox = charecterHitBox.GetComponent<Collider2D>();
            if (triggerHitBox == null)
            {
                Debug.LogError("the GameObject children with charecterHitBox dont have Collider2D!");
            }
            else if (triggerHitBox.isTrigger == false)
            {
                Debug.LogError("the Collider2D of the children need to be trigger!");
            }
            else
            {
                Debug.Log("CharacterHitBox found on: " + charecterHitBox.gameObject.name);
                Debug.Log("Collider2D found on: " + triggerHitBox.gameObject.name);
            }
        }
        else
        {
            Debug.LogError("this GameObject dont have Children or CharecterHitBox in Children!");
        }
    }

    private void Start()
    {
        DisableHitBox();
    }
    private void EnableHitBox()
    {
        if (triggerHitBox == null) return;
        triggerHitBox.enabled = true;
    }

    private void DisableHitBox()
    {
        if (charecterHitBox == null || triggerHitBox == null) return;
        charecterHitBox.ClearMemory();
        triggerHitBox.enabled = false;
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

        animator.SetInteger(nameof(AnimationParameters.AttackIndex), attackIndex + 1);
        attackIndex = (attackIndex + 1) % maxAttackIndex;
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
    public void FlipSprite(float directionX)
    {
        if (Mathf.Approximately(directionX, 0f))return;

        bool facingLeft = directionX < 0;
        spriteRenderer.flipX = facingLeft;

        if (charecterHitBox == null) return;
        Vector3 hitBoxPosition = charecterHitBox.transform.localPosition;

        hitBoxPosition.x = Mathf.Abs(hitBoxPosition.x) * (facingLeft ? -1 : 1);

        charecterHitBox.transform.localPosition = hitBoxPosition;
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
