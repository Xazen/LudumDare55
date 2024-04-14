using System.Collections;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public class DynamicBackground : MonoBehaviour
{
    [SerializeField]
    private GameObject[] dragonBalls;

    [SerializeField] private GameObject dragonBallsContainer;

    [SerializeField] private SpriteRenderer skyImage;
    [SerializeField] private GameObject shengronObject;
    [SerializeField] private GameObject booObject;
    [SerializeField] private CanvasGroup shengronAppearShine;
    [SerializeField] private Sprite nightSprite;

    [SerializeField] private CanvasGroup ripCanvas;
    [SerializeField] private CanvasGroup ripCanvas2;

    [SerializeField]
    private float scaleDuration;


    // Start is called before the first frame update

    void Start()
    {
        var model = Singletons.GameModel;
        if (model != null) model.OnNotePlayed += OnNotePlayed;

        Singletons.AudioManager.WonTrigger += OnWonTrigger;


        // StartCoroutine(DebugSequence());
    }

    private IEnumerator DebugSequence()
    {
        OnWonTrigger(AudioManager.WonTriggers.MusicEnd);
        yield return new WaitForSeconds(4.8f);
        OnWonTrigger(AudioManager.WonTriggers.Shenlon);
        yield return new WaitForSeconds(4.8f);
        OnWonTrigger(AudioManager.WonTriggers.RIP);
        yield return new WaitForSeconds(9.8f);
        Debug.Log("End of debug sequence");
    }

    private void OnDestroy()
    {
        var model = Singletons.GameModel;
        if (model != null) model.OnNotePlayed -= OnNotePlayed;

        Singletons.AudioManager.WonTrigger -= OnWonTrigger;
    }

    private void OnWonTrigger(AudioManager.WonTriggers obj)
    {
        switch (obj)
        {
            // music end to shenlon: 0 - 4.8s : dragon balls pulsing
            case AudioManager.WonTriggers.MusicEnd:
                GlobalSettings.BlockInput = true;

                // 0
                var dragonballsOutSequence = DOTween.Sequence();
                dragonballsOutSequence.Append(dragonBallsContainer.transform.DOScale(1.2f,  1f)
                    .SetRelative(true)
                    .SetLoops(2, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine));

                // 2.8
                dragonballsOutSequence.Insert(1.8f, dragonBallsContainer.transform.DOMoveY(1, 1.2f).SetRelative(true).SetEase(Ease.InOutSine));

                // 3
                dragonballsOutSequence.Append(dragonBallsContainer.transform.DOMoveY(-30, 1.8f).SetEase(Ease.OutCubic));
                dragonballsOutSequence.Play();
                break;
            // shenlon to RIP: 4.8s - 9.6: shenlong appear
            case AudioManager.WonTriggers.Shenlon:

                // 4.8
                // Shengron appears 4.8 - 8.8
                var shenlonAppearSequence = DOTween.Sequence();
                shenlonAppearSequence.AppendCallback(() =>
                {
                    shengronObject.SetActive(true);
                    shengronAppearShine.alpha = 0;
                    shengronAppearShine.gameObject.SetActive(true);
                });
                shenlonAppearSequence.Insert(0, booObject.transform.DOMoveY(-30, 1.8f).SetEase(Ease.OutCubic));
                shenlonAppearSequence.AppendInterval(3.5f);

                // 8.3
                shenlonAppearSequence.Append(shengronAppearShine.DOFade(1, 0.4f).SetEase(Ease.InOutSine));

                // 8.7
                shenlonAppearSequence.AppendInterval(0.2f);
                shenlonAppearSequence.AppendCallback(() =>
                {
                    skyImage.sprite = nightSprite;
                });
                shenlonAppearSequence.AppendInterval(0.2f);

                // 9.1
                shenlonAppearSequence.Append(shengronAppearShine.DOFade(0, 0.2f).SetEase(Ease.InOutSine));

                shenlonAppearSequence.Play();

                break;
            // RIP to PointsScreen: 9.728s: show rip
            case AudioManager.WonTriggers.RIP:

                var ripSequence = DOTween.Sequence();
                ripSequence.AppendCallback(() =>
                {
                    ripCanvas.alpha = 0;
                    ripCanvas.gameObject.SetActive(true);
                });
                ripSequence.Append(ripCanvas.DOFade(1, 1f).SetEase(Ease.InOutSine));
                ripSequence.AppendInterval(3f);
                ripSequence.AppendCallback(() =>
                {
                    ripCanvas2.alpha = 0;
                    ripCanvas2.gameObject.SetActive(true);
                });
                ripSequence.Append(ripCanvas2.DOFade(1, 1f).SetEase(Ease.InOutSine));
                ripSequence.Play();

                break;
        }
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
}
