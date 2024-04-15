using System;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text scoreDiffText;
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private List<TimingObject> timingObjects;
    [SerializeField] private Image healthImage;
    [SerializeField] private Image dragonBallImage;

    [SerializeField]
    private float scoreAnimDuration = 0.1f;
    [SerializeField]
    private float scaleDuration = 0.1f;
    [SerializeField]
    private float scaleMultiplier = 1.3f;

    private TweenerCore<int, int, NoOptions> _scoreTween;

    void Start()
    {
        Init();

        Singletons.NoteController.OnNoteReachedEnd += OnNoteReachedEnd;
        Singletons.GameModel.OnScoreChanged += SetScore;
        Singletons.GameModel.OnComboChanged += SetCombo;
        Singletons.GameModel.OnNotePlayed += OnNotePlayed;
        Singletons.GameModel.OnHealthChanged += OnHealthChanged;
        Singletons.GameModel.OnDragonBallFound += OnDragonBallFound;
    }

    private void OnDragonBallFound(int dragonBallFound)
    {
        dragonBallImage.fillAmount = (float) dragonBallFound / Singletons.Balancing.DragonBallCount;
    }

    private void OnHealthChanged(int health)
    {
        healthImage.fillAmount = (float) health / Singletons.Balancing.MaxHealth;
    }

    private void Init()
    {
        HideAllTimingObjects();
        dragonBallImage.fillAmount = 0;
        healthImage.fillAmount = 1;
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
        var minCombo = Singletons.Balancing.MinComboForHeal;
        comboText.gameObject.SetActive(combo >= minCombo);
        if (combo < minCombo)
        {
            return;
        }

        comboText.text = $"x{combo.ToString()}";
        if (combo > 4)
        {
            PlayScaleAnimation(comboText.transform);
        }
    }

    private void SetScore(int score, int diff)
    {
        if (_scoreTween != null && _scoreTween.IsActive() && !_scoreTween.IsComplete())
        {
            _scoreTween.Kill(true);
        }

        int currentScore = Math.Max(score - diff, 0);
        _scoreTween = DOTween.To(() => currentScore, x => scoreText.text = x.ToString("N0"), score, scoreAnimDuration)
            .OnComplete(() =>
            {
                scoreText.text = score.ToString("N0");
                _scoreTween = null;
            });

        scoreDiffText.gameObject.SetActive(true);
        scoreDiffText.text = "+" + diff.ToString("N0");
        PlayScaleAnimation(scoreDiffText.transform, () => scoreDiffText.gameObject.SetActive(false));
    }

    private void PlayScaleAnimation(Transform t, Action onComplete = null)
    {
        t.localScale = Vector3.one;
        t.DOScale(scaleMultiplier, scaleDuration).SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutQuint)
            .OnComplete(() => onComplete?.Invoke());
    }
}