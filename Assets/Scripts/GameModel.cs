using System.Collections.Generic;

namespace DefaultNamespace
{
    public class GameModel
    {
        public const int TrackCount = 4;


        private readonly Dictionary<int, Queue<NoteView>> _currentNotes = new ();

        public GameModel()
        {
            for (int i = 0; i < TrackCount; i++)
            {
                _currentNotes[i] = new Queue<NoteView>();
            }
        }
    }
}