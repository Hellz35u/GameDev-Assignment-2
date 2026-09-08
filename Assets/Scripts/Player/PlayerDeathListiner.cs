
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerDeathListiner : MonoBehaviour
{
    private GameObject playerGameObject;
    [SerializeField] private float afterDeathDurationSeconds = 1.5f;
    [SerializeField] private float afterTextDurationSeconds = 1.5f;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private TextMeshProUGUI text;
    private Coroutine currentCoroutine;
    private void Awake()
    {
        playerGameObject = FindAnyObjectByType<PlayerController>()?.gameObject;
        if(playerGameObject == null)
        {
            Debug.LogError("cant find Player in the Scene!");
        }
        GameEvents.PlayerHealthChange += OnPlayerHealthChange;
    }

    private IEnumerator OnDeathCoroutine()
    {
        yield return new WaitUntil(() => playerGameObject == null);

        yield return new WaitForSeconds(afterDeathDurationSeconds);
        if (text != null)
        {
            text.enabled = true;
            text.text = "Game Over";
        }

        yield return new WaitForSeconds(afterTextDurationSeconds);

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Scene to load is not specified!");
        }
    }

    private void OnPlayerHealthChange(int currentHealth, int fullHealth)
    {
        if (playerGameObject == null) return;
        if(currentHealth == 0)
        {
            currentCoroutine = StartCoroutine(OnDeathCoroutine());
        }
        
    }

    private void OnDestroy()
    {
        GameEvents.PlayerHealthChange -= OnPlayerHealthChange;
    }

}
