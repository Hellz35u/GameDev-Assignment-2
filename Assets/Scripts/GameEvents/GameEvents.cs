using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public static class GameEvents
{
    public static event Action<GameObject, GameObject> CharacterDeath;
    public static event Action<GameObject, GameObject, int> CharacterTakeHit;
    public static event Action<GameObject, int, int> CharacterHealthChange;

    public static event Action<int> ScoreUpdated;
    public static event Action<int> AddScore;

    public static event Action<int,int> PlayerHealthChange;

    public static event Action TimerEnded;

    public static event Action<int, float> WaveStarted;
    public static event Action WavesReset;


    public static void OnCharacterHealthChange(GameObject character, int currentHealth, int fullHealth)
    {
        CharacterHealthChange?.Invoke(character, currentHealth, fullHealth);
    }

    public static void OnCharacterTakeHit(GameObject victim, GameObject attacker, int damage)
    {
        CharacterTakeHit?.Invoke(victim, attacker, damage);
    }

    public static void OnCharacterDeath(GameObject victim , GameObject killer)
    {
        CharacterDeath?.Invoke(victim , killer);
    }

    public static void OnScoreUpdated(int totalScore)
    {
        ScoreUpdated?.Invoke(totalScore);
    }

    public static void OnAddScore(int scoreAmount)
    {
        AddScore?.Invoke(scoreAmount);
    }

    public static void OnPlayerHealthChange(int currentHealth,int maxHealth)
    {
        PlayerHealthChange?.Invoke(currentHealth, maxHealth);
    }
}