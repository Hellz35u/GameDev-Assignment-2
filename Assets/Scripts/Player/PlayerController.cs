using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("can't find CharacterController script in this GameObject!");
        }
    }

    private void OnEnable()
    {
        if (characterController == null) return;
        InputEvents.Move += characterController.Move;
        InputEvents.Jump += characterController.TryJump;
        InputEvents.Attack += characterController.Attack;
    }

    private void OnDisable()
    {
        if (characterController == null) return;
        InputEvents.Move -= characterController.Move;
        InputEvents.Jump -= characterController.TryJump;
        InputEvents.Attack -= characterController.Attack;
    }
}