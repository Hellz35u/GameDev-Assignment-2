using System;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterAttachedHealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthPointsText;
    private CharacterHealth characterHealth;

    private void Awake()
    {
        characterHealth = GetComponent<CharacterHealth>();
        if (characterHealth == null)
        {
            Debug.LogError("can't find CharacterHealth on this GameObject!");
        }
    }
    private void Start()
    {
        if (characterHealth == null) return;
        OnHealthChange(this.gameObject, characterHealth.GetHealth(), characterHealth.GetFullHealth());
    }
    private void OnEnable()
    {
        
        GameEvents.CharacterHealthChange += OnHealthChange;
        
    }

    private void OnDisable()
    {
        GameEvents.CharacterHealthChange -= OnHealthChange;
    }

    private void OnHealthChange(GameObject character, int currentHealth, int fullHealth)
    {
        if (character != this.gameObject) return;
        UpdateHealthBar(currentHealth, fullHealth);
        UpdateHealthText($"{currentHealth}");
    }

    private void UpdateHealthText(string text)
    {
        if (healthPointsText == null) return;
        healthPointsText.text = text;
    }

    private void UpdateHealthBar(int current, int full)
    {
        if (healthBar == null) return;
        healthBar.maxValue = full;
        healthBar.value = current;
    }
}