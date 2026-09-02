using UnityEngine;

public class CharacterActions : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private CharecterAnimation charecterAnimation;

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        charecterAnimation = GetComponent<CharecterAnimation>();
    }

    private void Update()
    {
        charecterAnimation.SetVerticalVelocity(characterMovement.GetVerticalVelocity());
    }

    public void SetFacing(float directionX)
    {
        charecterAnimation.FlipSprite(directionX);
    }

    public void Move(Vector2 direction)
    {
        characterMovement.HandleMovement(direction.x);
        charecterAnimation.SetMovement(direction);
    }

    public void TryJump()
    {
        if (characterMovement.HandleJump())
        {
            charecterAnimation.PlayJump();
        }
    }

    public void Attack()
    {
        charecterAnimation.PlayAttack();
    }

    public void Die()
    {
        charecterAnimation.PlayDeath();
    }

    public void TakeHit()
    {
        charecterAnimation.PlayHit();
    }
}