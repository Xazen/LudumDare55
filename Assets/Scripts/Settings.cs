using System;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject settingsContainer;
    [SerializeField] private SettingsControl difficultyControl;
    [SerializeField] private Button menuButton;

    private void Start()
    {
        difficultyControl.SetData(
            diff => GlobalSettings.Difficulty = diff,
            () => GlobalSettings.Difficulty,
            (Enum.GetValues(typeof(SongDifficulty)) as SongDifficulty[])!.ToList());
        menuButton.onClick.AddListener(OpenMenu);

        EventSystem.current.SetSelectedGameObject(difficultyControl.GetLeftButton());
    }

    private void OpenMenu()
    {
        SceneManager.LoadScene("Boot");
    }
}