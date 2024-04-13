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
    private GameObject notePrefab;


    [SerializeField] private float noteStartDistance = 30f;
    [SerializeField] private float trackTargetXStart = -1.5f;
    [SerializeField] private float trackTargetXEnd = 1.5f;

    private float _trackStepDistance;

    private Queue<GameObject> notePrefabPool = new ();


    // Start is called before the first frame update
    void Start()
    {
        var trackDistance = trackTargetXEnd - trackTargetXStart;
        _trackStepDistance = trackDistance / (TrackCount - 1);
        audioManager.RhythmCallback += OnNoteReceived;

        for (int i = 0; i < PoolCount; i++)
        {
            var note = Instantiate(notePrefab, transform);
            note.SetActive(false);
            notePrefabPool.Enqueue(note);
        }
    }

    private void OnNoteReceived(int trackIndex)
    {
        if (notePrefabPool.Count == 0)
        {
            // Optionally, create a new note if the pool is empty.
            // This depends on whether you want a fixed-size pool or a flexible one.
            var newNote = Instantiate(notePrefab, transform);
            newNote.SetActive(false);
            notePrefabPool.Enqueue(newNote);
        }

        var note = notePrefabPool.Dequeue();

        var noteXPosition = trackTargetXStart + _trackStepDistance * trackIndex;
        note.transform.position = new Vector3(noteXPosition, 0, noteStartDistance);
        note.transform.DOMoveZ(0, AudioManager.TimeOffset).SetEase(Ease.Linear).OnComplete(() =>
        {
            note.SetActive(false);
            notePrefabPool.Enqueue(note);
        });

        note.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
