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

        if (float.IsNaN(timeRemaining) || float.IsInfinity(timeRemaining))
        {
            Debug.LogError("Timer: timeRemaining contains an invalid value!");
            isRunning = false;
            return;
        }

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
        if (float.IsNaN(duration) || float.IsInfinity(duration))
        {
            Debug.LogError("Timer: duration contains an invalid value!");
            return;
        }

        if (duration < 0f)
        {
            Debug.LogError("Timer: duration cannot be negative!");
            return;
        }

        if (timerText == null)
        {
            Debug.LogError("Timer: timerText is not assigned!");
            return;
        }

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

    private void UpdateTimerText()
    {
        if (timerText == null)
            return;

        if (float.IsNaN(timeRemaining) || float.IsInfinity(timeRemaining))
        {
            Debug.LogError("Timer: Cannot update text because timeRemaining is invalid!");
            return;
        }
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}