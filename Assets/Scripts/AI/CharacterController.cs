

using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private CharacterAnimation characterAnimation;
    private bool enableControll = true;
    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        characterAnimation = GetComponent<CharacterAnimation>();
    }


    private void OnCharacterTakeHit(GameObject hittedGameObject, int damage)
    {
        if (hittedGameObject != this.gameObject) return;
        TakeHit();

    }

    private void OnCharacterDeath(GameObject deathGameObject)
    {
        if (deathGameObject != this.gameObject) return;
        Die();
    }
    private void OnEnable()
    {
        GameEvents.CharacterDeath += OnCharacterDeath;
        GameEvents.CharacterTakeHit += OnCharacterTakeHit;
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
        GameEvents.CharacterDeath -= OnCharacterDeath;
        GameEvents.CharacterTakeHit -= OnCharacterTakeHit;
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
        if (characterAnimation == null)
            return;
        characterAnimation.SetVerticalVelocity(characterMovement.GetVerticalVelocity());
    }

    public void EnableController()
    {
        enableControll = true;
    }

    public void DisableController()
    {
        enableControll = false;
        characterMovement.HandleMovement(0f);
    }

    public void SetFacing(float directionX)
    {
        if (Time.timeScale == 0f || !enableControll) return;
        characterAnimation.FlipSprite(directionX);
    }

    public void Move(Vector2 direction)
    {
        if (Time.timeScale == 0f || !enableControll) return;
        characterMovement.HandleMovement(direction.x);
        characterAnimation.SetMovement(direction);
    }

    public void TryJump()
    {
        if (Time.timeScale == 0f || !enableControll) return;
        if (characterMovement.HandleJump())
        {
            characterAnimation.PlayJump();
        }
    }

    public void Attack()
    {
        if (Time.timeScale == 0f || !enableControll) return;
        characterAnimation.PlayAttack();
    }

    public void Die()
    {
        DisableController();
        characterAnimation.PlayDeath();


        //for debug
        Destroy(this.gameObject);
    }

    public void TakeHit()
    {
        characterAnimation.PlayHit();
    }
}