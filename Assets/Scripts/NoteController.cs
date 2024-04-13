using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    [SerializeField]
    private AudioManager audioManager;

    [SerializeField] private float noteStartDistance = 30f;
    [SerializeField] private float noteEndDistance = -10f;
    [SerializeField] private float trackTargetXStart = -1.5f;
    [SerializeField] private float trackTargetXEnd = 1.5f;
    [SerializeField] private NotePool notePool;

    private float _trackStepDistance;
    private GameModel _gameModel;


    // Start is called before the first frame update
    void Start()
    {
        var trackDistance = trackTargetXEnd - trackTargetXStart;
        _trackStepDistance = trackDistance / (GameModel.TrackCount - 1);

        audioManager.RhythmCallback += OnNoteReceived;

        _gameModel = new GameModel();
    }

    private void OnNoteReceived(int trackIndex)
    {
        SpawnNote(trackIndex);
    }

    private void SpawnNote(int trackIndex)
    {
        var note = notePool.GetNote();

        note.SetTrack(trackIndex);
        var noteXPosition = trackTargetXStart + _trackStepDistance * trackIndex;
        note.transform.position = new Vector3(noteXPosition, 0, noteStartDistance);
        note.transform.localScale = Vector3.one;

        note.transform.DOMoveZ(noteEndDistance, noteStartDistance / AudioManager.TimeOffset)
            .SetSpeedBased(true)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                if (note.transform.position.z < 0)
                {
                    note.transform.localScale = Vector3.one * 0.5f;
                }
            })
            .OnComplete(() => { notePool.ReturnNote(note); });

        note.gameObject.SetActive(true);
    }
}
