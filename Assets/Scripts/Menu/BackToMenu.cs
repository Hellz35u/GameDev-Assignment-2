using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
public class BackToMenu : MonoBehaviour
{
    [SerializeField] Button backToMenuButton;
    string mainMenuSceneName = "MainMenu";
    
    void Start()
    {
        backToMenuButton.onClick.AddListener(()=>ReturnToMenu());
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
