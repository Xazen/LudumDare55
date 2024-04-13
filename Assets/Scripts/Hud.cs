using System;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private List<TimingObject> timingObjects;

    [SerializeField]
    private float scoreAnimDuration = 0.1f;
    [SerializeField]
    private float scaleDuration = 0.1f;
    [SerializeField]
    private float scaleMultiplier = 1.3f;
    [SerializeField]
    private int minComboForShow = 5;

    private TweenerCore<int, int, NoOptions> _scoreTween;

    void Start()
    {
        Init();

        Singletons.NoteController.OnNoteReachedEnd += OnNoteReachedEnd;
        Singletons.GameModel.OnScoreChanged += SetScore;
        Singletons.GameModel.OnComboChanged += SetCombo;
        Singletons.GameModel.OnNotePlayed += OnNotePlayed;
    }

    private void Init()
    {
        HideAllTimingObjects();
        scoreText.text = "0";
        comboText.text = "x0";
        comboText.gameObject.SetActive(false);
    }

    private void HideAllTimingObjects()
    {
        foreach (var timingObject in timingObjects)
        {
            timingObject.GameObject.SetActive(false);
        }
    }

    void OnDestroy()
    {
        Singletons.GameModel.OnScoreChanged -= SetScore;
        Singletons.GameModel.OnComboChanged -= SetCombo;
        Singletons.GameModel.OnNotePlayed -= OnNotePlayed;
    }

    private void OnNoteReachedEnd(NoteView noteView)
    {
        ShowTimingObject(TimingType.Miss);
    }

    private void OnNotePlayed(int _, NoteView noteView, ScoreType scoreType)
    {
        ShowTimingObject(scoreType.TimingType);
    }

    private void ShowTimingObject(TimingType timingType)
    {
        foreach (var timingObject in timingObjects)
        {
            timingObject.GameObject.SetActive(timingObject.TimingType == timingType);
            if (timingObject.TimingType == timingType)
            {
                PlayScaleAnimation(timingObject.GameObject.transform, () => HideObject(timingObject.GameObject.transform));
            }
        }
    }

    private void HideObject(Transform transform)
    {
        transform.DOScale(Vector3.zero, scaleDuration).OnComplete(() => transform.gameObject.SetActive(false));
    }

    private void SetCombo(int combo)
    {
        comboText.gameObject.SetActive(combo >= minComboForShow);
        if (combo < minComboForShow)
        {
            return;
        }

        comboText.text = $"x{combo.ToString()}";
        if (combo > 4)
        {
            PlayScaleAnimation(comboText.transform);
        }
    }

    private void SetScore(int score)
    {
        if (_scoreTween != null && _scoreTween.IsActive() && !_scoreTween.IsComplete())
        {
            _scoreTween.Kill(true);
        }

        int currentScore = int.Parse(scoreText.text);
        _scoreTween = DOTween.To(() => currentScore, x => scoreText.text = x.ToString(), score, scoreAnimDuration)
            .OnComplete(() =>
            {
                scoreText.text = score.ToString();
                _scoreTween = null;
            });
    }

    private void PlayScaleAnimation(Transform t, Action onComplete = null)
    {
        t.localScale = Vector3.one;
        t.DOScale(scaleMultiplier, scaleDuration).SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutQuint)
            .OnComplete(() => onComplete?.Invoke());
    }
}