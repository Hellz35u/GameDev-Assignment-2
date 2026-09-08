using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private TextMeshProUGUI healthPointsText;
    private Slider slider;

    private void OnEnable()
    {
        GameEvents.PlayerHealthChange += HealthBarUpdater;
    }
    private void OnDisable()
    {
        GameEvents.PlayerHealthChange -= HealthBarUpdater;
    }
    private void Awake()
    {
        slider = GetComponent<Slider>();
        healthPointsText = GetComponentInChildren<TextMeshProUGUI>();

        if (slider == null)
        {
            Debug.LogError("Error: Slider is null");
            return;
        }

        if (healthPointsText == null)
        {
            Debug.LogError("Error: HealthPointsText is null");
            return;
        }

    }
    private void HealthBarUpdater(int currentHealth,int maxHealth)
    {
        if (slider == null || healthPointsText == null || maxHealth <= 0) return;
        slider.value = currentHealth / (float)maxHealth;
        healthPointsText.text = $"{(int)currentHealth}";
    }
}