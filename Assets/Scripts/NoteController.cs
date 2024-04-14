using System;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    [SerializeField]
    private AudioManager audioManager;

    public Action<NoteView> OnNoteReachedEnd;
    private Dictionary<NoteView, Tween> _noteTweens = new ();

    private int dragonBallCount;

    private void Awake()
    {
        Singletons.RegisterNoteController(this);
    }

    private void Start()
    {
        dragonBallCount = 0;
        audioManager.RhythmCallback += OnNoteReceived;
        Singletons.GameModel.OnNotePlayed += OnNotePlayed;
    }

    private void OnDestroy()
    {
        Singletons.GameModel.OnNotePlayed -= OnNotePlayed;
    }

    private void OnNotePlayed(int trackIndex, NoteView note, ScoreType scoreType)
    {
        Singletons.NotePool.ReturnNote(note);
        _noteTweens.Remove(note);

        if (scoreType.TimingType != TimingType.Miss)
        {
            note.transform.localScale = Vector3.one * 0.5f;
        }
    }

    private void OnNoteReceived(int trackIndex, bool isDragonBall)
    {
        SpawnNote(trackIndex, isDragonBall);
    }

    private void SpawnNote(int trackIndex, bool isDragonBall)
    {
        var note = Singletons.NotePool.GetNote();

        note.SetTrack(trackIndex);

        note.SetIsDragonBall(isDragonBall, dragonBallCount);
        if (isDragonBall)
        {
            dragonBallCount++;
        }
        note.transform.position = Singletons.Balancing.GetNoteSpawnPosition(trackIndex);
        note.transform.localScale = Vector3.one;

        var tweenerCore = note.transform.DOMoveZ(Singletons.Balancing.GetMaxNoteDistance(), Singletons.Balancing.GetNoteSpeed())
            .SetDelay(Singletons.Balancing.GetNoteDelayForTargetDuration())
            .SetSpeedBased(true)
            .SetEase(Ease.Linear)
            .OnStart(() =>
            {
                note.gameObject.SetActive(true);
                Singletons.GameModel.RegisterNote(note, trackIndex);
            })
            .OnComplete(() =>
            {
                Singletons.GameModel.RegisterMiss(trackIndex);
                Singletons.NotePool.ReturnNote(note);
                OnNoteReachedEnd?.Invoke(note);
                _noteTweens.Remove(note);
            });

        _noteTweens.Add(note, tweenerCore);
    }

    public void Pause()
    {

        foreach (var noteTween in _noteTweens)
        {
            noteTween.Value.Pause();
        }
    }

    public void Resume()
    {
        foreach (var noteTween in _noteTweens)
        {
            noteTween.Value.Play();
        }
    }
}
