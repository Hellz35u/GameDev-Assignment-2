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
    string firstLevelName = "Level1";
    string infoLevelName = "InfoScene";
 
    void Start()
    {
        playButton.onClick.AddListener(()=>StartGame());
        exitButton.onClick.AddListener(()=>ExitGame());
        infoButton.onClick.AddListener(()=>InfoScene());
    }
    public void StartGame()
    {
        SceneManager.LoadScene(firstLevelName);
    }
    public void ExitGame()
    {
        Debug.Log("Exiting Game");
    }
    public void InfoScene()
    {
        SceneManager.LoadScene(infoLevelName);
    }

}
