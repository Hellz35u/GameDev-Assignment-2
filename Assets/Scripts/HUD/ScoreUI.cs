using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        GameEvents.ScoreChanged += ScoreUpdater;
    }
    private void OnDisable()
    {
        GameEvents.ScoreChanged -= ScoreUpdater;
    }
    private void Start()
    {
        ScoreUpdater(0);
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