using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerAnimator playerAnimator;
    CharacterMovement characterMovement;
    void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        playerAnimator = GetComponent<PlayerAnimator>();

        InputEvents.Move += OnMove;
        InputEvents.Jump += OnJump;
        InputEvents.Attack += OnAttack;
        InputEvents.Death += OnDeath;
    }
    private void Update()
    {
        playerAnimator.SetVerticalVelocity(characterMovement.GetVerticalVelocity());
    }
    private void OnJump()
    {
        if(characterMovement.HandleJump())
        {
            playerAnimator.PlayJump();
        }
    }

    private void OnMove(Vector2 dir)
    {
        characterMovement.HandleMovement(dir.x);
        playerAnimator.SetMovement(dir);
    }

    private void OnAttack()
    {
        playerAnimator.PlayAttack();
    }
    private void OnDeath()
    {

    }
}
