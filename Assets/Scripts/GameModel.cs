using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameModel
    {
        public const int TrackCount = 4;
        public int Score = 0;
        public int Combo = 0;

        private readonly Dictionary<int, Queue<NoteView>> _currentNotes = new ();

        public GameModel()
        {
            for (int i = 0; i < TrackCount; i++)
            {
                _currentNotes[i] = new Queue<NoteView>();
            }
        }

        public Action<int, NoteView> OnNotePlayed;

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
            var balancing = Singletons.Balancing;
            var note = _currentNotes[trackIndex].Dequeue();
            var distance = Math.Abs(note.transform.position.z);
            var noteSpeed = balancing.GetNoteSpeed();
            var timing = distance / noteSpeed;
            OnNotePlayed?.Invoke(trackIndex, note);
            var millisOff = timing * 100;
            Debug.Log($"{trackIndex}: {millisOff}ms {distance}u (miss: {balancing.NoteEndDistance / noteSpeed}s)");

            var scoreType = balancing.GetScoreType(distance);
            if (scoreType == null)
            {
                Combo = 0;
                return;
            }

            if (scoreType.IsCombo)
            {
                Combo++;
            }

            int scoreMultiplier = Math.Max(1, Combo);
            Score += scoreType.Score * scoreMultiplier;
        }
    }
}