using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class Playfield : MonoBehaviour
    {
        [SerializeField]
        private GameObject trackObject;

        [SerializeField]
        private GameObject[] trackers;
        [SerializeField]
        private float scaleDuration = 0.1f;
        [SerializeField]
        private float scaleMultiplier = 1.3f;

        private void Start()
        {
            Singletons.RegisterPlayfield(this);
            MoveTrackByMillis(GlobalSettings.NoteOffsetMillis);
        }

        public void AnimateTracker(int index)
        {
            PlayScaleAnimation(trackers[index].transform);
        }

        private void PlayScaleAnimation(Transform t)
        {
            t.localScale = Vector3.one;
            t.DOScale(scaleMultiplier, scaleDuration).SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.OutQuint)
                .OnComplete(() => t.localScale = Vector3.one);
        }

        public void MoveTrackByMillis(int millis)
        {
            var trackPosition = trackObject.transform.position;
            trackPosition.z = Singletons.Balancing.GetDistanceForMillis(millis);
            trackObject.transform.position = trackPosition;
        }
    }
}