using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterMovement characterMovement;
    void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        InputEvents.Move += OnMove;
        InputEvents.Jump += OnJump;
    }

    private void OnJump()
    {
        characterMovement.HandleJump();
    }

    private void OnMove(Vector2 dir)
    {
        characterMovement.HandleMovement(dir.x);
    }
}
