using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{

    [SerializeField] int fullHealth = 0;
    [SerializeField] private int currentHealth;//[SerializeField] it is for DEBUG
    bool isAlive = true;

    private void Awake()
    {
        currentHealth = fullHealth;
    }

    void OnEnable()
    {
        GameEvents.CharacterTakeHit += OnCharacterTakeHit;
        GameEvents.CharacterDeath += OnCharacterDeath;  
    }

    void OnDisable()
    {
        GameEvents.CharacterTakeHit -= OnCharacterTakeHit;
        GameEvents.CharacterDeath -= OnCharacterDeath;
    }

    private void OnCharacterDeath(GameObject deathGameObject)
    {
        if (deathGameObject != this.gameObject) return;
        isAlive = false;
    }

    private void OnCharacterTakeHit(GameObject hittenGameObject, int damage)
    {
        if (hittenGameObject != this.gameObject || !isAlive) return;
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            GameEvents.OnCharacterDeath(hittenGameObject);
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public int GetFullHealth()
    {
        return fullHealth;
    }

    public bool IsAlive()
    {
        return currentHealth > 0 && isAlive;
    }
}
