using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    private const int TrackCount = 4;
    private const int PoolCount = 100;

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private NoteView notePrefab;

    [SerializeField] private float noteStartDistance = 30f;
    [SerializeField] private float trackTargetXStart = -1.5f;
    [SerializeField] private float trackTargetXEnd = 1.5f;

    private float _trackStepDistance;

    private readonly Queue<NoteView> _notePrefabPool = new ();


    // Start is called before the first frame update
    void Start()
    {
        var trackDistance = trackTargetXEnd - trackTargetXStart;
        _trackStepDistance = trackDistance / (TrackCount - 1);
        audioManager.RhythmCallback += OnNoteReceived;

        for (int i = 0; i < PoolCount; i++)
        {
            var note = Instantiate(notePrefab, transform);
            note.gameObject.SetActive(false);
            _notePrefabPool.Enqueue(note);
        }
    }

    private void OnNoteReceived(int trackIndex)
    {
        if (_notePrefabPool.Count == 0)
        {
            // Optionally, create a new note if the pool is empty.
            // This depends on whether you want a fixed-size pool or a flexible one.
            var newNote = Instantiate(notePrefab, transform);
            newNote.gameObject.SetActive(false);
            _notePrefabPool.Enqueue(newNote);
        }

        var note = _notePrefabPool.Dequeue();

        note.SetTrack(trackIndex);
        var noteXPosition = trackTargetXStart + _trackStepDistance * trackIndex;
        note.transform.position = new Vector3(noteXPosition, 0, noteStartDistance);
        note.transform.DOMoveZ(0, AudioManager.TimeOffset).SetEase(Ease.Linear).OnComplete(() =>
        {
            note.gameObject.SetActive(false);
            _notePrefabPool.Enqueue(note);
        });

        note.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
