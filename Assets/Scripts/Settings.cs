using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private GameObject settingsContainer;
    [SerializeField] private SettingsControl noteTimingControl;
    [SerializeField] private SettingsControl difficultyControl;
    [SerializeField] private SettingSliderControl noteOffsetControl;
    [SerializeField] private Playfield trackObject;
    [SerializeField] private GameObject notePreview;

    [SerializeField]
    private float scaleDuration = 0.1f;
    [SerializeField]
    private float scaleMultiplier = 1.3f;

    private void Start()
    {
        Singletons.GameModel.OnNotePlayed += OnNotePlayed;

        difficultyControl.SetData(
            newValue => GlobalSettings.Difficulty = newValue,
            () => GlobalSettings.Difficulty,
            (Enum.GetValues(typeof(SongDifficulty)) as SongDifficulty[])!.ToList());

        var noteCalculationTypes = new List<NoteCalculationType>
        {
            NoteCalculationType.Easy,
            NoteCalculationType.Normal,
            NoteCalculationType.Hard
        };
        noteTimingControl.SetData(
            newValue => GlobalSettings.NoteTiming = newValue,
            () => GlobalSettings.NoteTiming,
            noteCalculationTypes);

        noteOffsetControl.SetData(
            newValue =>
            {
                trackObject.MoveTrackByMillis(newValue);
                GlobalSettings.NoteOffsetMillis = newValue;
            },
            () => GlobalSettings.NoteOffsetMillis,
            -250,
            250);

        menuButton.onClick.AddListener(OpenMenu);
        EventSystem.current.SetSelectedGameObject(difficultyControl.GetLeftButton());
    }

    private void Update()
    {
        if (!EventSystem.current.currentSelectedGameObject)
        {
            EventSystem.current.SetSelectedGameObject(difficultyControl.GetLeftButton());
        }
    }

    private void OnNotePlayed(int arg1, NoteView arg2, ScoreType arg3)
    {
        PlayScaleAnimation(notePreview.transform);
    }

    private void PlayScaleAnimation(Transform t, Action onComplete = null)
    {
        t.localScale = Vector3.one;
        t.DOScale(scaleMultiplier, scaleDuration).SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutQuint)
            .OnComplete(() => onComplete?.Invoke());
    }

    private void OpenMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}