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
        PlayerEvents.Death += OnDeath;
        PlayerEvents.TakeHit += OnTakeHit;
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
        playerAnimator.PlayDeath();
    }
    private void OnTakeHit()
    {
        playerAnimator.PlayHit();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            //here we call the event on hit
        }
    }
}
