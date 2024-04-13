using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public class GameModel
    {
        public const int TrackCount = 4;
        public int Score;
        public int Combo;
        public int Health;
        public int DragonBallFound;

        private readonly Dictionary<int, Queue<NoteView>> _currentNotes = new ();

        public Action<int, NoteView, ScoreType> OnNotePlayed;
        public Action<int> OnComboChanged;
        public Action<int> OnScoreChanged;
        public Action<int> OnHealthChanged;
        public Action<int> OnDragonBallFound;
        public Action<int> OnHealthZero;

        public GameModel()
        {
            for (int i = 0; i < TrackCount; i++)
            {
                _currentNotes[i] = new Queue<NoteView>();
            }
            DragonBallFound = 0;
            SetHealth(Singletons.Balancing.MaxHealth);
        }

        public bool HaveAllDragonBalls()
        {
            return DragonBallFound == Singletons.Balancing.DragonBallCount;
        }

        private void RegisterDragonBall()
        {
            DragonBallFound++;
            OnDragonBallFound?.Invoke(DragonBallFound);
        }

        public void RegisterNote(NoteView note, int trackIndex)
        {
            _currentNotes[trackIndex].Enqueue(note);
        }

        public void RegisterMiss(int trackIndex)
        {
            _currentNotes[trackIndex].Dequeue();
            Miss();
        }

        private void Miss()
        {
            SetHealth(Health - Singletons.Balancing.HealthDecrease);
            SetCombo(0);
        }

        public void PlayNote(int trackIndex)
        {
            if (_currentNotes[trackIndex].Count == 0)
            {
                return;
            }

            var balancing = Singletons.Balancing;
            var note = _currentNotes[trackIndex].Dequeue();
            var scoreType = balancing.GetScoreTypeByNote(note);
            if (scoreType.IsCombo)
            {
                SetCombo(Combo + 1);
            }
            else
            {
                Miss();
            }

            if (note.IsDragonBall && CanRegisterDragonBall(scoreType))
            {
                RegisterDragonBall();
            }

            int scoreMultiplier = Math.Max(1, Combo);
            SetScore(Score + scoreType.Score * scoreMultiplier);
            OnNotePlayed?.Invoke(trackIndex, note, scoreType);
        }

        private bool CanRegisterDragonBall(ScoreType scoreType)
        {
            return scoreType.TimingType is TimingType.Perfect or TimingType.Good or TimingType.Great;
        }

        private void SetScore(int score)
        {
            if (score == Score)
            {
                return;
            }
            Score = score;
            OnScoreChanged?.Invoke(score);
        }

        private void SetCombo(int combo)
        {
            if (combo == Combo)
            {
                return;
            }
            Combo = combo;
            if (combo >= Singletons.Balancing.MinComboForHeal)
            {
                SetHealth(Math.Min(Singletons.Balancing.MaxHealth, Health + Singletons.Balancing.HealthIncrease));
            }
            OnComboChanged?.Invoke(combo);
        }

        private void SetHealth(int health)
        {
            if (health == Health)
            {
                return;
            }
            Health = health;
            OnHealthChanged?.Invoke(health);
        }
    }
}