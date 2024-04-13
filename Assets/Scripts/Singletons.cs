namespace DefaultNamespace
{
    public static class Singletons
    {
        private static GameModel _gameModel;
        public static GameModel GameModel => _gameModel ??= new GameModel();

        private static NotePool _notePool;
        public static NotePool NotePool => _notePool;

        private static NoteController _noteController;
        public static NoteController NoteController => _noteController;

        private static Balancing _balancing;
        public static Balancing Balancing => _balancing;

        public static void RegisterNotePool(NotePool notePool)
        {
            _notePool = notePool;
        }

        public static void RegisterNoteController(NoteController noteController)
        {
            _noteController = noteController;
        }

        public static void RegisterBalancing(Balancing balancing)
        {
            _balancing = balancing;
        }
    }
}