using TMPro;
using UnityEngine;

public class NoteView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text debugTrackText;

    [SerializeField]
    private GameObject defaultObject;

    [SerializeField]
    private GameObject[] dragonBalls;

    private int _trackIndex;
    public int TrackIndex => _trackIndex;

    private int _dragonBallIndex;
    public int DragonBallIndex => _dragonBallIndex;

    public bool IsDragonBall { get; private set; } = false;

    private void OnDisable()
    {
        ResetView();
    }

    private void ResetView()
    {
        MarkKiBlast();
    }

    public void SetTrack(int trackIndex)
    {
        _trackIndex = trackIndex;
        debugTrackText.text = trackIndex.ToString();
    }

    public void SetIsDragonBall(bool isDragonBall, int dragonBallCount)
    {
        IsDragonBall = isDragonBall;

        if (isDragonBall)
        {
            MarkDragonBall(dragonBallCount);
        }
        else
        {
            MarkKiBlast();
        }
    }

    private void MarkDragonBall(int dragonBallCount)
    {
        _dragonBallIndex = dragonBallCount;
        int dragonBallIndex = Mathf.Min(dragonBallCount, dragonBalls.Length - 1);
        dragonBalls[dragonBallIndex].SetActive(true);
        defaultObject.SetActive(false);
        debugTrackText.text = $"{dragonBallCount}D";
    }

    private void MarkKiBlast()
    {
        IsDragonBall = false;
        defaultObject.SetActive(true);
        foreach (var dragonBall in dragonBalls)
        {
            dragonBall.SetActive(false);
        }
    }
}
