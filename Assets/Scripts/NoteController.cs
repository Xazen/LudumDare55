using System;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    [SerializeField]
    private GameObjectPool impactPool;

    public Action<NoteView> OnNoteReachedEnd;
    private readonly Dictionary<NoteView, Tween> _noteTweens = new ();

    private int _dragonBallCount;

    private void Awake()
    {
        Singletons.RegisterNoteController(this);
    }

    private void Start()
    {
        _dragonBallCount = 0;
        Singletons.AudioManager.RhythmCallback += OnNoteReceived;
        var model = Singletons.GameModel;
        if (model != null) model.OnNotePlayed += OnNotePlayed;
    }

    private void OnDestroy()
    {
        Singletons.AudioManager.RhythmCallback -= OnNoteReceived;

        var model = Singletons.GameModel;
        if (model != null) model.OnNotePlayed -= OnNotePlayed;
    }

    private void OnNotePlayed(int trackIndex, NoteView note, ScoreType scoreType)
    {
        Debug.Log("Note is dragon ball? " + note.IsDragonBall);
        if (note.IsDragonBall)
        {
            Singletons.AudioManager.PlayDragonballSound();
        }
        else
        {
            Singletons.AudioManager.Play_Hit(trackIndex);
        }

        var impactObject = impactPool.GetObject();
        impactObject.transform.position = note.transform.position;
        impactObject.SetActive(true);

        Singletons.NotePool.ReturnNote(note);
        _noteTweens.Remove(note);
    }

    private void OnNoteReceived(int trackIndex, bool isDragonBall)
    {
        SpawnNote(trackIndex, isDragonBall);
    }

    private void SpawnNote(int trackIndex, bool isDragonBall)
    {
        var note = Singletons.NotePool.GetNote();

        note.SetTrack(trackIndex);

        note.SetIsDragonBall(isDragonBall, _dragonBallCount);
        if (isDragonBall)
        {
            _dragonBallCount++;
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
