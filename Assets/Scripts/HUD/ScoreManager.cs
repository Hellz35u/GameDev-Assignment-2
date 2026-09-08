using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int currentScore = 0;
    private void Start()
    {
        GameEvents.OnScoreUpdated(currentScore);
    }

    private void OnEnable()
    {
        GameEvents.AddScore += AddScore;
    }

    private void OnDisable()
    {
        GameEvents.AddScore -= AddScore;
    }

    private void AddScore(int amount)
    {
        currentScore += amount;
        GameEvents.OnScoreUpdated(currentScore);
    }
}