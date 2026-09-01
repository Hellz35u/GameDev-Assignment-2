using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private TextMeshProUGUI healthPointsText;
    private Slider slider;

    private float maxHealthBarPoints = 100f;

    private void OnEnable()
    {
        GameEvents.OnHealthChanged += HealthBarUpdater;
    }
    private void OnDisable()
    {
        GameEvents.OnHealthChanged -= HealthBarUpdater;
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

        HealthBarUpdater(maxHealthBarPoints);
    }
    private void HealthBarUpdater(float currentHealth)
    {
        slider.value = currentHealth / maxHealthBarPoints;
        healthPointsText.text = $"{(int)currentHealth}";
    }
}