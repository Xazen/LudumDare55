using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Balancing", menuName = "ScriptableObjects/Balancing", order = 1)]
    public class Balancing : ScriptableObject
    {
        public const int TrackCount = 4;

        [Header("General")]
        [SerializeField] private int dragonBallCount = 7;

        [Header("Health")]
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int healthDecrease = 10;
        [SerializeField] private int healthIncrease = 1;
        [SerializeField] private int minComboForHeal = 5;

        [Header("Note Settings")]
        [SerializeField] private float noteStartDistance = 13f;
        [SerializeField] private float trackWidth = 5.4f;
        [SerializeField]
        [Range(0.5f, AudioManager.TimeOffset)]
        private float targetNoteAnimationDuration = 2.5f;

        [Header("Timings")]
        [SerializeField] private NoteCalculationType noteCalculationType;
        [SerializeField] private List<ScoreType> scoreTypes;

        public int DragonBallCount => dragonBallCount;
        public int MaxHealth => maxHealth;
        public int HealthDecrease => healthDecrease;
        public int MinComboForHeal => minComboForHeal;
        public int HealthIncrease => healthIncrease;

        void Init()
        {
            scoreTypes.Sort((a, b) => a.Distance.CompareTo(b.Distance));

            LogBalancing();
        }

        public float GetMaxNoteDistance()
        {
            var badType = scoreTypes.Find(type => type.TimingType == TimingType.Bad);
            float result = noteCalculationType switch
            {
                NoteCalculationType.MillisEasy => GetMaxNoteDistanceByMillis(badType.MillisEasy),
                NoteCalculationType.MillisMid => GetMaxNoteDistanceByMillis(badType.MillisMid),
                NoteCalculationType.MillisHard => GetMaxNoteDistanceByMillis(badType.MillisHard),
                NoteCalculationType.Distance => badType.Distance,
                _ => badType.Distance
            };

            return -result;
        }

        private float GetMaxNoteDistanceByMillis(float millis)
        {
            return millis / 1000 * GetNoteSpeed();
        }

        private void Awake()
        {
            Init();
        }

        private void LogBalancing()
        {
            Debug.Log($"Calculation Type: {noteCalculationType}");
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

            Debug.Log($"NoteDistance: {GetMaxNoteDistance()}");
            for (int i = 0; i < TrackCount; i++)
            {
                var transformPosition = GetNoteSpawnPosition(i);
                Debug.Log($"SpawnPosition: {i}:{transformPosition}");
            }

        }

        public float GetNoteSpeed()
        {
            return noteStartDistance / targetNoteAnimationDuration;
        }

        public float GetNoteDelayForTargetDuration()
        {
            return AudioManager.TimeOffset - targetNoteAnimationDuration;
        }

        public Vector3 GetNoteSpawnPosition(int trackIndex)
        {
            var trackStepDistance = trackWidth / (TrackCount - 1);
            var noteXPosition = -(trackWidth / 2) + trackStepDistance * trackIndex;
            var transformPosition = new Vector3(noteXPosition, 0, noteStartDistance);
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