using UnityEngine;

public class NewPlayerController : MonoBehaviour
{
    private CharacterActions characterActions;

    void Awake()
    {
        characterActions = GetComponent<CharacterActions>();
        if (characterActions == null)
        {
            Debug.LogError("can't find CharacterActions script in this GameObject!");
        }
    }

    private void OnEnable()
    {
        TargetManager.GetInstance()?.RegisterTarget(gameObject);
        InputEvents.Move += characterActions.Move;
        InputEvents.Jump += characterActions.TryJump;
        InputEvents.Attack += characterActions.Attack;
        PlayerEvents.Death += characterActions.Die;
        PlayerEvents.TakeHit += characterActions.TakeHit;
    }

    private void OnDisable()
    {
        InputEvents.Move -= characterActions.Move;
        InputEvents.Jump -= characterActions.TryJump;
        InputEvents.Attack -= characterActions.Attack;
        PlayerEvents.Death -= characterActions.Die;
        PlayerEvents.TakeHit -= characterActions.TakeHit;
        TargetManager.GetInstance()?.UnregisterTarget(gameObject);
    }
}