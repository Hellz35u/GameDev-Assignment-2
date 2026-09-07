using System;
using UnityEngine;

public class NewPlayerController : MonoBehaviour
{
    private CharacterController characterActions;
    private CharacterHealth characterHealth;
    void Awake()
    {
        characterActions = GetComponent<CharacterController>();
        if (characterActions == null)
        {
            Debug.LogError("can't find CharacterActions script in this GameObject!");
        }
        characterHealth = GetComponent<CharacterHealth>();
        if (characterHealth == null)
        {
            Debug.LogError("can't find CharacterHealth script in this GameObject!");
        }
    }

    private void OnEnable()
    {
        InputEvents.Move += characterActions.Move;
        InputEvents.Jump += characterActions.TryJump;
        InputEvents.Attack += characterActions.Attack;
        GameEvents.CharacterDeath += OnCharacterDeath;
        GameEvents.CharacterTakeHit += OnChracterTakeHit;
    }


    private void OnCharacterDeath(GameObject deathGameObject)
    {
        if (deathGameObject != this.gameObject) return;

        GameEvents.OnPlayerHealthUpdated(0, characterHealth.GetFullHealth());
        
    }
    private void OnChracterTakeHit(GameObject hittedGameObject, int arg2)
    {
        if (hittedGameObject != this.gameObject) return;

        GameEvents.OnPlayerHealthUpdated(characterHealth.GetHealth(), characterHealth.GetFullHealth());
    }



    private void OnDisable()
    {
        InputEvents.Move -= characterActions.Move;
        InputEvents.Jump -= characterActions.TryJump;
        InputEvents.Attack -= characterActions.Attack;
        GameEvents.CharacterDeath -= OnCharacterDeath;
        GameEvents.CharacterTakeHit -= OnChracterTakeHit;
    }
}