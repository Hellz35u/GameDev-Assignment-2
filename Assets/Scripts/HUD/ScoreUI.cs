using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        if(scoreText == null)
        {
            Debug.LogError("can't find TextMeshProUGUI in this GameObject!");
        }
    }
    private void OnEnable()
    {
        GameEvents.ScoreUpdated += ScoreUpdater;
    }
    private void OnDisable()
    {
        GameEvents.ScoreUpdated -= ScoreUpdater;
    }
    private void ScoreUpdater(int score)
    {
        if (scoreText == null)
        {
            Debug.LogError("scoreText NULL EXCEPTION");
            return;
        }

        scoreText.text = $"Score: {score}";
    }
}