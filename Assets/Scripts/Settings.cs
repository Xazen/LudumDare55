using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject settingsContainer;
    [SerializeField] private SettingsControl noteTimingControl;
    [SerializeField] private SettingsControl difficultyControl;
    [SerializeField] private Button menuButton;

    private void Start()
    {
        difficultyControl.SetData(
            diff => GlobalSettings.Difficulty = diff,
            () => GlobalSettings.Difficulty,
            (Enum.GetValues(typeof(SongDifficulty)) as SongDifficulty[])!.ToList());

        var noteCalculationTypes = new List<NoteCalculationType>
        {
            NoteCalculationType.Easy,
            NoteCalculationType.Normal,
            NoteCalculationType.Hard
        };
        noteTimingControl.SetData(
            diff => GlobalSettings.NoteTiming = diff,
            () => GlobalSettings.NoteTiming,
            noteCalculationTypes);

        menuButton.onClick.AddListener(OpenMenu);

        EventSystem.current.SetSelectedGameObject(difficultyControl.GetLeftButton());
    }

    private void OpenMenu()
    {
        SceneManager.LoadScene("Boot");
    }
}