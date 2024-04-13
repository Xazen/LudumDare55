using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Balancing : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private int dragonBallCount = 7;
        public int DragonBallCount => dragonBallCount;

        [Header("Health")]
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int healthDecrease = 1;
        [SerializeField] private int healthIncrease = 1;
        [SerializeField] private int minComboForHeal = 5;
        public int MaxHealth => maxHealth;
        public int HealthDecrease => healthDecrease;
        public int MinComboForHeal => minComboForHeal;
        public int HealthIncrease => healthIncrease;

        [Header("Note Settings")]
        [SerializeField] private float noteStartDistance = 30f;

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
        [SerializeField] private NoteCalculationType noteCalculationType;
        [SerializeField] private List<ScoreType> scoreTypes;

        private float _trackStepDistance;
        public float TrackTargetXStart => trackTargetXStart;
        public float TrackStepDistance => _trackStepDistance;
        public float NoteStartDistance => noteStartDistance;
        public float NoteEndDistance
        {
            get
            {
                return -GetMaxNoteDistance();
            }
        }

        private float GetMaxNoteDistance()
        {
            var badType = scoreTypes.Find(type => type.TimingType == TimingType.Bad);
            switch (noteCalculationType)
            {
                case NoteCalculationType.Distance:
                    return badType.Distance;
                case NoteCalculationType.MillisEasy:
                    return GetMaxNoteDistanceByMillis(badType.MillisEasy);
                case NoteCalculationType.MillisMid:
                    return GetMaxNoteDistanceByMillis(badType.MillisMid);
                case NoteCalculationType.MillisHard:
                    return GetMaxNoteDistanceByMillis(badType.MillisHard);
            }

            return badType.Distance;
        }

        private float GetMaxNoteDistanceByMillis(float millis)
        {
            return millis / 1000 * GetNoteSpeed();
        }

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

            LogBalancing();
        }

        private void LogBalancing()
        {
            foreach (var scoreType in scoreTypes)
            {
                switch (noteCalculationType)
                {
                    case NoteCalculationType.Distance:
                        Debug.Log($"ScoreType: {scoreType.TimingType}: {(scoreType.Distance / GetNoteSpeed() * 1000):F2}ms ({scoreType.Distance})");
                        break;
                    case NoteCalculationType.MillisEasy:
                        Debug.Log($"ScoreType: {scoreType.TimingType}: {GetMaxNoteDistanceByMillis(scoreType.MillisEasy):F2}u ({scoreType.MillisEasy})");
                        break;
                    case NoteCalculationType.MillisMid:
                        Debug.Log($"ScoreType: {scoreType.TimingType}: {GetMaxNoteDistanceByMillis(scoreType.MillisMid):F2}u ({scoreType.MillisMid})");
                        break;
                    case NoteCalculationType.MillisHard:
                        Debug.Log($"ScoreType: {scoreType.TimingType}: {GetMaxNoteDistanceByMillis(scoreType.MillisHard):F2}u ({scoreType.MillisHard})");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            Debug.Log($"NoteDistance: {NoteEndDistance}");
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

        public ScoreType GetScoreTypeByNote(NoteView note)
        {
            var distance = Math.Abs(note.transform.position.z);
            var noteSpeed = GetNoteSpeed();
            var timing = distance / noteSpeed;
            var millisOff = timing * 1000;
            Debug.Log($"{millisOff:F2}ms {distance:F2}u");
            switch (noteCalculationType)
            {
                case NoteCalculationType.Distance:

                    foreach (var scoreType in scoreTypes)
                    {
                        float distanceAsMillis = scoreType.Distance / GetNoteSpeed() * 1000;
                        if (distance < distanceAsMillis)
                        {
                            Debug.Log($"Distance: {scoreType.TimingType}");
                            return scoreType;
                        }
                    }
                    break;
                case NoteCalculationType.MillisEasy:
                    foreach (var scoreType in scoreTypes)
                    {
                        if (millisOff < scoreType.MillisEasy)
                        {
                            Debug.Log($"MillisEasy: {scoreType.TimingType}");
                            return scoreType;
                        }
                    }
                    break;
                case NoteCalculationType.MillisMid:
                    foreach (var scoreType in scoreTypes)
                    {
                        if (millisOff < scoreType.MillisMid)
                        {
                            Debug.Log($"MillisMid: {scoreType.TimingType}");
                            return scoreType;
                        }
                    }
                    break;
                case NoteCalculationType.MillisHard:
                    foreach (var scoreType in scoreTypes)
                    {
                        if (millisOff < scoreType.MillisHard)
                        {
                            Debug.Log($"MillisHard: {scoreType.TimingType}");
                            return scoreType;
                        }
                    }
                    break;
            }
            return scoreTypes.Find(type => type.TimingType == TimingType.Miss);
        }
    }

    public enum NoteCalculationType
    {
        Distance,
        MillisEasy,
        MillisMid,
        MillisHard
    }
}