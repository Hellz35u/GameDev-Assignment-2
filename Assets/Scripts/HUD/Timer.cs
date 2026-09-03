using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float waveDuration = 60f;

    private float timeRemaining;
    private bool isRunning;
    private Action onTimerEnd;

    private void Start()
    {
        if (timerText == null)
        {
            Debug.LogError("Timer: timerText is not assigned!");
            enabled = false;
            return;
        }

        StartTimer(waveDuration);
    }

    private void Update()
    {
        if (!isRunning)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning = false;
            onTimerEnd?.Invoke();
            onTimerEnd = null;
            UpdateTimerText();
            Debug.Log("Timer finished");
            return;
        }
        UpdateTimerText();
    }
    public void StartTimer(float duration, Action endTimerEvent = null)
    {
        timeRemaining = duration;
        isRunning = true;
        UpdateTimerText();
        if (endTimerEvent != null)
        {
            onTimerEnd += endTimerEvent;
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer(float duration)
    {
        timeRemaining = duration;
        isRunning = false;
        UpdateTimerText();
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public bool IsFinished()
    {
        return timeRemaining <= 0f;
    }
    private void OnEnable()
    {
        GameEvents.OnWaveStarted += HandleWaveStarted;
    }
    private void OnDisable()
    {
        GameEvents.OnWaveStarted -= HandleWaveStarted;
    }
    private void HandleWaveStarted(int waveNumber, float duration)
    {
        StartTimer(duration);
    }
    private void UpdateTimerText()
    {
        if (timerText == null)
            return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}