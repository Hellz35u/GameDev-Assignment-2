using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private SystemInput systemInput;
    private void Awake()
    {
        systemInput = new();
    }

    private void OnEnable()
    {
        systemInput.Player.Enable();
        systemInput.Player.Move.performed += OnMovePerformed;
        systemInput.Player.Move.canceled += OnMoveCanceled;
        systemInput.Player.Jump.performed += OnJumpPerformed;
        systemInput.Player.Pause.performed += OnPausePerformed;
        systemInput.Player.Attack.performed += OnAttackPerformed;
        systemInput.Player.Attack.canceled += OnAttackCanceled;
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
}