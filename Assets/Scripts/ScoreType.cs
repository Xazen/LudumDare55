using System;

namespace DefaultNamespace
{
    [Serializable]
    public class ScoreType
    {
        public TimingType TimingType;
        public float Distance;
        public int Score;
        public bool IsCombo;
    }

    public enum TimingType
    {
        Perfect,
        Great,
        Good,
        Bad,
        Miss
    }
}