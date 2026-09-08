using DG.Tweening;
using System;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    [Header("buttons")]
    private Button continueButton;
    private Button restartButton;
    private Button backMenuButton;
    private Button HelpButton;
    private Tween tween;
    private Image pauseMenuBackGroundImage;
    private Color pauseMenuBackGroundImageOriginalColor;
    private Button[] pauseMenuButtons;
    private Color pauseMenuButtonOriginalColor;
    private GameObject pauseMenuCanvas;
    private GameObject playerGameObject;
    [SerializeField] private GameObject allyPrefab;
    [Header("Animation Settings")]
    [SerializeField] private float transitionDuration = 0.5f;

    private void Awake()
    {
        playerGameObject = FindAnyObjectByType<PlayerController>()?.gameObject;
        InputEvents.Pause += PausePressed;
        pauseMenuBackGroundImage = GetComponentInChildren<Image>(true);
        if(pauseMenuBackGroundImage == null)
        {
            Debug.LogError("can't locate background image of the pause menu");
        }
        else
        {
            pauseMenuBackGroundImageOriginalColor = pauseMenuBackGroundImage.color;
            pauseMenuCanvas = pauseMenuBackGroundImage.gameObject;
            pauseMenuButtons = pauseMenuBackGroundImage.GetComponentsInChildren<Button>(true);
            if(pauseMenuButtons.Length == 0)
            {
                Debug.LogError("not found any button in the pause menu");
            }
            else if (pauseMenuButtons[0].image != null)
            {
                pauseMenuButtonOriginalColor = pauseMenuButtons[0].image.color;
            }
            else
            {
                Debug.LogError("The first pause menu button is missing an Image component , make sure to use the prefab");
            }
        }
    }
    public void SpawnAlly()
    {
        Instantiate(allyPrefab,playerGameObject.transform.position, Quaternion.identity);
    }

    void Start()
    {  
        HidePauseMenu();
    }

    private void OnDestroy()
    {
        InputEvents.Pause -= PausePressed;
    }
    public void Resume()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
        SceneManager.GetActiveScene().buildIndex);
    }

    public void PausePressed()
    {
        if (pauseMenuCanvas.activeInHierarchy)
        {
            HidePauseMenu();
        }
        else
        {
            AnimatePauseMenu();
        }
    }

    private void HidePauseMenu()
    {
        tween?.Kill();
        Time.timeScale = 1f;
        pauseMenuCanvas?.SetActive(false);
        if(pauseMenuBackGroundImage != null)
        {
            pauseMenuBackGroundImage.color = pauseMenuBackGroundImageOriginalColor;
        }
        if (pauseMenuButtons != null)
        {
            foreach (Button button in pauseMenuButtons)
            {
                button.enabled = false;
                button.image.color = pauseMenuButtonOriginalColor;
            }
        }
    }

    private void AnimatePauseMenu()
    {
        Ease menuEaseIn = Ease.InQuint;
        tween?.Kill();

        DG.Tweening.Sequence pauseSequence = DOTween.Sequence();

        pauseSequence.Append(DOVirtual.Float(1f, 0f, transitionDuration, val => Time.timeScale = val));
        pauseSequence.SetUpdate(true);
        if (pauseMenuBackGroundImage != null)
        {
            Color transparentColor = pauseMenuBackGroundImageOriginalColor;
            transparentColor.a = 0f;
            pauseMenuBackGroundImage.color = transparentColor;
            pauseMenuCanvas.SetActive(true);
            pauseSequence.Join(pauseMenuBackGroundImage.DOColor(pauseMenuBackGroundImageOriginalColor, transitionDuration).SetEase(menuEaseIn));
        }
        if (pauseMenuButtons != null && pauseMenuButtons.Length != 0)
        {
            Color transparentColor = pauseMenuButtonOriginalColor;
            transparentColor.a = 0f;
            foreach (Button button in pauseMenuButtons)
            {
                button.image.color = transparentColor;
                button.enabled = false;
                pauseSequence.Join(button.image.DOColor(pauseMenuButtonOriginalColor, transitionDuration).SetEase(menuEaseIn));
            }
        }

        pauseSequence.OnComplete(() => ShowPauseMenu());
        tween = pauseSequence;

    }

    private void ShowPauseMenu()
    {
        if(pauseMenuButtons != null)
        {
            foreach (Button button in pauseMenuButtons)
            {
                button.enabled = true;
            }
        }
        pauseMenuCanvas?.SetActive(true);
        Time.timeScale = 0f;
    }
}
