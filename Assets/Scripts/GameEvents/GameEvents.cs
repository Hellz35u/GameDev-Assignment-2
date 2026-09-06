using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<GameObject> OnCharacterDeath;
    public static event Action<GameObject,int> OnCharacterTakeHit;

    public static event Action OnPlayerDeath;
    public static event Action<int> OnPlayerTakeHit;

    public static event Action<int> OnScoreChanged;

    public static event Action<float> OnHealthChanged;

    public static event Action OnTimerEnded;

    public static event Action<int, float> OnWaveStarted;
    public static event Action OnWavesReset;

    // Game
    public static event Action OnGameWon;
    
    public static void ScoreChanged(int score)
    {
        OnScoreChanged?.Invoke(score);
    }
    public static void HealthChanged(float currentHealth)
    {
        OnHealthChanged?.Invoke(currentHealth);
    }
    public static void TimerEnded()
    {
        OnTimerEnded?.Invoke();
    }
    public static void WaveStarted(int waveNumber, float waveDuration)
    {
        OnWaveStarted?.Invoke(waveNumber, waveDuration);
    }
    public static void WavesReset()
    {
        OnWavesReset?.Invoke();
    }
    public static void GameWon()
    {
        OnGameWon?.Invoke();
    }
}