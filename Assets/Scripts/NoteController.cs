using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    [SerializeField]
    private AudioManager audioManager;

    private void Awake()
    {
        Singletons.RegisterNoteController(this);
    }

    private void Start()
    {
        audioManager.RhythmCallback += OnNoteReceived;
    }

    private void OnNoteReceived(int trackIndex)
    {
        SpawnNote(trackIndex);
    }

    private void SpawnNote(int trackIndex)
    {
        var note = Singletons.NotePool.GetNote();

        note.SetTrack(trackIndex);
        var noteXPosition = Singletons.Balancing.TrackTargetXStart + Singletons.Balancing.TrackStepDistance * trackIndex;
        note.transform.position = new Vector3(noteXPosition, 0, Singletons.Balancing.NoteStartDistance);
        note.transform.localScale = Vector3.one;

        note.transform.DOMoveZ(Singletons.Balancing.NoteEndDistance, Singletons.Balancing.NoteStartDistance / AudioManager.TimeOffset)
            .SetSpeedBased(true)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                if (note.transform.position.z < 0)
                {
                    note.transform.localScale = Vector3.one * 0.5f;
                }
            })
            .OnComplete(() => { Singletons.NotePool.ReturnNote(note); });

        note.gameObject.SetActive(true);
    }
}
