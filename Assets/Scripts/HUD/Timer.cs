using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float timeRemaining;
    private bool isRunning;
    private Action onTimerEnd;

    private void Awake()
    {
        if (timerText == null)
        {
            Debug.LogError("Timer: TextMeshProUGUI component not found!");
            enabled = false;
            return;
        }

        timeRemaining = 0f;
        isRunning = false;
        UpdateTimerText();
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
            onTimerEnd = endTimerEvent;
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public bool IsFinished()
    {
        return timeRemaining <= 0f;
    }
    //private void OnEnable()
    //{
    //    GameEvents.OnWaveStarted += HandleWaveStarted;
    //}
    //private void OnDisable()
    //{
    //    GameEvents.OnWaveStarted -= HandleWaveStarted;
    //}
    //private void HandleWaveStarted(int waveNumber, float duration)
    //{
    //    StartTimer(duration);
    //}
    private void UpdateTimerText()
    {
        if (timerText == null)
            return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}