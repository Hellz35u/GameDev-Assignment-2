using UnityEngine;
using TMPro;

using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button infoButton;
    TextMeshProUGUI textMeshPro;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playButton.onClick.AddListener(()=>StartGame());
        exitButton.onClick.AddListener(()=>ExitGame());
        infoButton.onClick.AddListener(()=>InfoScene());
    }
    private void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    private void ExitGame()
    {
        Debug.Log("Exiting Game");
    }
    private void InfoScene()
    {
        Debug.Log("Opening Information Window");
    }

}
