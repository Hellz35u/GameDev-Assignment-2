using UnityEngine;

public class CharacterActions : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private CharacterAnimation characterAnimation;

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        characterAnimation = GetComponent<CharacterAnimation>();
    }

    private void OnEnable()
    {
        if (TargetManager.GetInstance() == null)
        {
            Debug.LogError($"'{gameObject.name}' can't register to Target Manager");
        }
        else
        {
            TargetManager.GetInstance().RegisterTarget(gameObject);
        }
    }

    private void OnDisable()
    {
        if (TargetManager.GetInstance() == null)
        {
            Debug.LogWarning($"'{gameObject.name}' can't unregister from Target Manager ,\n if the game is ending ignore this message!");
        }
        else
        {
            TargetManager.GetInstance().UnregisterTarget(gameObject);
        }
    }

    private void Update()
    {
        characterAnimation.SetVerticalVelocity(characterMovement.GetVerticalVelocity());
    }

    public void SetFacing(float directionX)
    {
        if (Time.timeScale == 0f) return;
        characterAnimation.FlipSprite(directionX);
    }

    public void Move(Vector2 direction)
    {
        if (Time.timeScale == 0f) return;
        characterMovement.HandleMovement(direction.x);
        characterAnimation.SetMovement(direction);
    }

    public void TryJump()
    {
        if (Time.timeScale == 0f) return;
        if (characterMovement.HandleJump())
        {
            characterAnimation.PlayJump();
        }
    }

    public void Attack()
    {
        if (Time.timeScale == 0f) return;
        characterAnimation.PlayAttack();
    }

    public void Die()
    {
        characterAnimation.PlayDeath();
    }

    public void TakeHit()
    {
        characterAnimation.PlayHit();
    }
}