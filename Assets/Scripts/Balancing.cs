using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Balancing : MonoBehaviour
    {
        [SerializeField] private float noteStartDistance = 30f;
        [SerializeField] private float noteEndDistance = -1.5f;
        [SerializeField] private float trackTargetXStart = -1.5f;
        [SerializeField] private float trackTargetXEnd = 1.5f;

        private float _trackStepDistance;
        public float TrackTargetXStart => trackTargetXStart;
        public float TrackStepDistance => _trackStepDistance;
        public float NoteStartDistance => noteStartDistance;
        public float NoteEndDistance => noteEndDistance;

        private void Awake()
        {
            Singletons.RegisterBalancing(this);
        }

        void Start()
        {
            var trackDistance = trackTargetXEnd - trackTargetXStart;
            _trackStepDistance = trackDistance / (GameModel.TrackCount - 1);
        }
    }
}