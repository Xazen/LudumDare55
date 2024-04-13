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
        note.transform.position = Singletons.Balancing.GetNoteSpawnPosition(trackIndex);
        note.transform.localScale = Vector3.one;

        note.transform.DOMoveZ(Singletons.Balancing.NoteEndDistance, Singletons.Balancing.GetNoteSpeed())
            .SetDelay(Singletons.Balancing.NoteSpeed)
            .SetSpeedBased(true)
            .SetEase(Ease.Linear)
            .OnStart(() => { note.gameObject.SetActive(true); })
            .OnUpdate(() =>
            {
                if (note.transform.position.z < 0)
                {
                    note.transform.localScale = Vector3.one * 0.5f;
                }
            })
            .OnComplete(() => { Singletons.NotePool.ReturnNote(note); });
    }
}
