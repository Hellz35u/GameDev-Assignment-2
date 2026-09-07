using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private TextMeshProUGUI healthPointsText;
    private Slider slider;

    private void OnEnable()
    {
        GameEvents.PlayerHealthRatioUpdated += HealthBarUpdater;
    }
    private void OnDisable()
    {
        GameEvents.PlayerHealthRatioUpdated -= HealthBarUpdater;
    }
    private void Start()
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
        slider.value = currentHealth / (float)maxHealth;
        healthPointsText.text = $"{(int)currentHealth}";
    }
}