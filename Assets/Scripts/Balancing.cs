using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class Balancing : MonoBehaviour
    {
        [Header("Note Settings")]
        [SerializeField] private float noteStartDistance = 30f;
        [SerializeField] private float noteEndDistance = -2.7f;
        [SerializeField] private float trackTargetXStart = -2.7f;
        [SerializeField] private float trackTargetXEnd = 1.5f;
        [SerializeField]
        [Range(0, AudioManager.TimeOffset - 0.5f)]
        private float noteSpeed = 4f;

        // speed: 4
        // 1.5: 200 ms (bad)
        // 0.75: 100 ms (good)
        // 0.375: 50 ms (great)
        // 0.1875: 25 ms (perfect)
        [Header("Timings")]
        [SerializeField] private List<ScoreType> scoreTypes;

        private float _trackStepDistance;
        public float TrackTargetXStart => trackTargetXStart;
        public float TrackStepDistance => _trackStepDistance;
        public float NoteStartDistance => noteStartDistance;
        public float NoteEndDistance => noteEndDistance;
        public float NoteSpeed => noteSpeed;

        private void Awake()
        {
            Singletons.RegisterBalancing(this);
        }

        void Start()
        {
            var trackDistance = trackTargetXEnd - trackTargetXStart;
            _trackStepDistance = trackDistance / (GameModel.TrackCount - 1);
            scoreTypes.Sort((a, b) => a.Distance.CompareTo(b.Distance));
        }

        public float GetNoteSpeed()
        {
            return NoteStartDistance / (AudioManager.TimeOffset - NoteSpeed);
        }

        public Vector3 GetNoteSpawnPosition(int trackIndex)
        {
            var noteXPosition = Singletons.Balancing.TrackTargetXStart + Singletons.Balancing.TrackStepDistance * trackIndex;
            var transformPosition = new Vector3(noteXPosition, 0, Singletons.Balancing.NoteStartDistance);
            return transformPosition;
        }

        public ScoreType GetScoreType(float distance)
        {
            foreach (var scoreType in scoreTypes)
            {
                if (distance < scoreType.Distance)
                {
                    return scoreType;
                }
            }

            return scoreTypes[^1];
        }
    }
}