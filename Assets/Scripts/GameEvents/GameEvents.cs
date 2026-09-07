using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<GameObject> CharacterDeath;
    public static event Action<GameObject,int> CharacterTakeHit;

    public static event Action PlayerDeath;
    public static event Action<int> PlayerTakeHit;

    public static event Action<int> ScoreChanged;

    public static event Action<int,int> PlayerHealthRatioUpdated;

    public static event Action TimerEnded;

    public static event Action<int, float> WaveStarted;
    public static event Action WavesReset;

    // Game
    public static event Action GameWon;
    
    public static void OnCharacterTakeHit(GameObject gameObject,int damage)
    {
        CharacterTakeHit?.Invoke(gameObject, damage);
    }

    public static void OnCharacterDeath(GameObject gameObject)
    {
        CharacterDeath?.Invoke(gameObject);
    }

    public static void OnScoreChanged(int score)
    {
        ScoreChanged?.Invoke(score);
    }
    public static void OnPlayerHealthUpdated(int currentHealth,int maxHealth)
    {
        PlayerHealthRatioUpdated?.Invoke(currentHealth, maxHealth);
    }
    public static void OnTimerEnded()
    {
        TimerEnded?.Invoke();
    }
    public static void OnWaveStarted(int waveNumber, float waveDuration)
    {
        WaveStarted?.Invoke(waveNumber, waveDuration);
    }
    public static void OnWavesReset()
    {
        WavesReset?.Invoke();
    }
    public static void OnGameWon()
    {
        GameWon?.Invoke();
    }
}