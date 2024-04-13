using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameModel
    {
        public const int TrackCount = 4;
        public int Score;
        public int Combo;

        private readonly Dictionary<int, Queue<NoteView>> _currentNotes = new ();

        public Action<int, NoteView, ScoreType> OnNotePlayed;
        public Action<int> OnComboChanged;
        public Action<int> OnScoreChanged;

        public GameModel()
        {
            for (int i = 0; i < TrackCount; i++)
            {
                _currentNotes[i] = new Queue<NoteView>();
            }
        }

        public void RegisterNote(NoteView note, int trackIndex)
        {
            _currentNotes[trackIndex].Enqueue(note);
        }

        public void RemoveNoteFromTrack(int trackIndex)
        {
            _currentNotes[trackIndex].Dequeue();
        }

        public void PlayNote(int trackIndex)
        {
            if (_currentNotes[trackIndex].Count == 0)
            {
                return;
            }
            var balancing = Singletons.Balancing;
            var note = _currentNotes[trackIndex].Dequeue();
            var distance = Math.Abs(note.transform.position.z);
            var noteSpeed = balancing.GetNoteSpeed();
            var timing = distance / noteSpeed;
            var millisOff = timing * 100;
            Debug.Log($"{trackIndex}: {millisOff}ms {distance}u (miss: {balancing.NoteEndDistance / noteSpeed}s)");

            var scoreType = balancing.GetScoreType(distance);
            if (scoreType.IsCombo)
            {
                SetCombo(Combo + 1);
            }
            else
            {
                SetCombo(0);
            }

            int scoreMultiplier = Math.Max(1, Combo);
            SetScore(Score + scoreType.Score * scoreMultiplier);
            OnNotePlayed?.Invoke(trackIndex, note, scoreType);
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
            OnComboChanged?.Invoke(combo);
        }
    }
}