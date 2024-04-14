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
            float result = GlobalSettings.NoteTiming switch
            {
                NoteCalculationType.Easy => GetDistanceForMillis(badType.MillisEasy),
                NoteCalculationType.Normal => GetDistanceForMillis(badType.MillisMid),
                NoteCalculationType.Hard => GetDistanceForMillis(badType.MillisHard),
                NoteCalculationType.Distance => badType.Distance,
                _ => badType.Distance
            };

            return -result;
        }

        public float GetDistanceForMillis(float millis)
        {
            return millis / 1000 * GetNoteSpeed();
        }

        private void Awake()
        {
            Init();
        }

        private void LogBalancing()
        {
            Debug.Log($"Calculation Type: {GlobalSettings.NoteTiming}");
            foreach (var scoreType in scoreTypes)
            {
                switch (GlobalSettings.NoteTiming)
                {
                    case NoteCalculationType.Distance:
                        Debug.Log($"ScoreType: {scoreType.TimingType}: {(scoreType.Distance / GetNoteSpeed() * 1000):F2}ms ({scoreType.Distance})");
                        break;
                    case NoteCalculationType.Easy:
                        Debug.Log($"ScoreType: {scoreType.TimingType}: {GetDistanceForMillis(scoreType.MillisEasy):F2}u ({scoreType.MillisEasy})");
                        break;
                    case NoteCalculationType.Normal:
                        Debug.Log($"ScoreType: {scoreType.TimingType}: {GetDistanceForMillis(scoreType.MillisMid):F2}u ({scoreType.MillisMid})");
                        break;
                    case NoteCalculationType.Hard:
                        Debug.Log($"ScoreType: {scoreType.TimingType}: {GetDistanceForMillis(scoreType.MillisHard):F2}u ({scoreType.MillisHard})");
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

            Debug.Log($"Offset: {GlobalSettings.NoteOffsetMillis}");
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
            var distance = note.transform.position.z;
            var originalMillisOff = GetTimeMillisForDistance(distance);
            var millisOff = originalMillisOff - GlobalSettings.NoteOffsetMillis;
            var millisAbs = Math.Abs(millisOff);
            Debug.Log($"{millisOff:F2}ms (original: {originalMillisOff:F2}ms {distance:F2}u)");
            switch (GlobalSettings.NoteTiming)
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
                case NoteCalculationType.Easy:
                    foreach (var scoreType in scoreTypes)
                    {
                        if (millisAbs < scoreType.MillisEasy)
                        {
                            Debug.Log($"MillisEasy: {scoreType.TimingType}");
                            return scoreType;
                        }
                    }
                    break;
                case NoteCalculationType.Normal:
                    foreach (var scoreType in scoreTypes)
                    {
                        if (millisAbs < scoreType.MillisMid)
                        {
                            Debug.Log($"MillisMid: {scoreType.TimingType}");
                            return scoreType;
                        }
                    }
                    break;
                case NoteCalculationType.Hard:
                    foreach (var scoreType in scoreTypes)
                    {
                        if (millisAbs < scoreType.MillisHard)
                        {
                            Debug.Log($"MillisHard: {scoreType.TimingType}");
                            return scoreType;
                        }
                    }
                    break;
            }
            return scoreTypes.Find(type => type.TimingType == TimingType.Miss);
        }

        private double GetTimeMillisForDistance(float distance)
        {
            var noteSpeed = GetNoteSpeed();
            var timing = distance / noteSpeed;
            return timing * 1000;
        }
    }

    public enum NoteCalculationType
    {
        Distance,
        Easy,
        Normal,
        Hard
    }
}