using UnityEngine;
using TMPro;
using System;
public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float waveDuration = 60f;
    private Action timerEnd;
    private float timeRemaining;
    private bool isRuning = true;

    private void Start()
    {
        timeRemaining = waveDuration;
    }
    private void Update()
    {
        if (!isRuning)
            return;

        timeRemaining -= Time.deltaTime;

        if(timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRuning = false;
            timerEnd?.Invoke();
            Debug.Log("Timer finished");
        }

        UpdateTimerText();
    }
    public void ListenToTimerEnd(Action func)
    {
        timerEnd += func;
    }
    public void StartTimer(float duration)
    {
        timeRemaining = duration;
        isRuning = true;

        UpdateTimerText();
    }

    public void StopTimer()
    {
        isRuning = false;
    }
    public void ResetTimer(float duration)
    {
        timeRemaining = duration;

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
    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
