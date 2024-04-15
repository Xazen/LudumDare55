﻿using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public class GameModel
    {
        public int Score;
        public int Combo;
        public int Health;
        public int DragonBallFound;

        private readonly Dictionary<int, Queue<NoteView>> _currentNotes = new ();
        private readonly Dictionary<TimingType, int> timingNotesCount = new ();
        public Dictionary<TimingType, int> TimingNotesCount => timingNotesCount;

        public Action<int, NoteView, ScoreType> OnNotePlayed;
        public Action<int> OnComboChanged;
        public Action<int, int> OnScoreChanged;
        public Action<int> OnHealthChanged;
        public Action<int> OnDragonBallFound;
        public Action<int> OnHealthZero;

        public GameModel()
        {
            for (int i = 0; i < Balancing.TrackCount; i++)
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
            timingNotesCount[TimingType.Miss] = timingNotesCount.GetValueOrDefault(TimingType.Miss) + 1;
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

            timingNotesCount[scoreType.TimingType] = timingNotesCount.GetValueOrDefault(scoreType.TimingType) + 1;

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

            var diff = score - Score;
            Score = score;
            OnScoreChanged?.Invoke(score, diff);
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
            if (health <= 0)
            {
                OnHealthZero?.Invoke(0);
            }
        }

        public List<NoteView> GetPerfectNotes()
        {
            var perfectNotes = new List<NoteView>();
            foreach (var noteQueue in _currentNotes.Values)
            {
                if (noteQueue.Count == 0)
                {
                    continue;
                }

                var note = noteQueue.Peek();
                if (note.IsDragonBall)
                {
                    continue;
                }

                var score = Singletons.Balancing.GetScoreTypeByNote(note);
                if (score.TimingType == TimingType.Perfect)
                {
                    perfectNotes.Add(note);
                }
            }
            return perfectNotes;
        }

        public bool HasNote(int trackIndex)
        {
            if (_currentNotes[trackIndex].Count > 0)
            {
                var noteView = _currentNotes[trackIndex].Peek();
                return Singletons.Balancing.GetScoreTypeByNote(noteView).TimingType != TimingType.Miss;
            }
            return false;
        }
    }
}