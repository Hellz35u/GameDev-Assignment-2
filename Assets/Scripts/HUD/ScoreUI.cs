using DG.Tweening.Core.Easing;
using System;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    //[SerializeField] private GameManager gameManager;
    private Action updateScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //gameManager.OnScoreChanged += ScoreUpdater;
        ScoreUpdater(0);
    }

    public void ScoreUpdater(int score)
    {
        if (scoreText != null)
            return;

        scoreText.text = $"Score: {score}";

    }
}
