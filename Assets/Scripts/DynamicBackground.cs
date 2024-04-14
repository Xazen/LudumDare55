using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public class DynamicBackground : MonoBehaviour
{
    [SerializeField]
    private GameObject[] dragonBalls;

    [SerializeField]
    private float scaleDuration;

    // Start is called before the first frame update
    void Start()
    {
        var model = Singletons.GameModel;
        if (model != null) model.OnNotePlayed += OnNotePlayed;

    }

    private void OnNotePlayed(int trackIndex, NoteView note, ScoreType scoreType)
    {
        Debug.Log("OnNotePlayed in DynamicBackground");
        if (note.IsDragonBall)
        {
            var dragonBall = dragonBalls[note.DragonBallIndex];
            dragonBall.transform.localScale = Vector3.zero;
            dragonBall.SetActive(true);
            dragonBall.transform.DOScale(Vector3.one, scaleDuration).SetEase(Ease.OutBounce);
        }
    }

    private void OnDestroy()
    {
        var model = Singletons.GameModel;
        if (model != null) model.OnNotePlayed -= OnNotePlayed;
    }
}
