using UnityEngine;

public class PlayerHealthBroadcaster : MonoBehaviour
{
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
        GameEvents.OnPlayerHealthChange(currentHealth, fullHealth);
    }
}