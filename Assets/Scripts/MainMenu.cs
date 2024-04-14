using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button settingsButton;

    private void Start()
    {
        Singletons.AudioManager.ToMenu();
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
        settingsButton.onClick.AddListener(OpenSettings);

        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }

    private void Update()
    {
        if (!EventSystem.current.currentSelectedGameObject)
        {
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        }
    }

    private void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
