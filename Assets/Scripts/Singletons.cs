﻿namespace DefaultNamespace
{
    public static class Singletons
    {
        private static GameModel _gameModel;
        public static GameModel GameModel => _gameModel;

        private static NotePool _notePool;
        public static NotePool NotePool => _notePool;

        private static NoteController _noteController;
        public static NoteController NoteController => _noteController;

        private static AudioManager _audioManager;
        public static AudioManager AudioManager => _audioManager;

        private static Balancing _balancing;
        public static Balancing Balancing => _balancing;

        private static PauseMenu _pauseMenu;
        public static PauseMenu PauseMenu => _pauseMenu;
        private static Playfield _playfield;
        public static Playfield Playfield => _playfield;

        private static GameOver _gameOver;
        public static GameOver GameOver => _gameOver;

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

        public static void RegisterAudioManager(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        public static void RegisterPauseMenu(PauseMenu pauseMenu)
        {
            _pauseMenu = pauseMenu;
        }

        public static void RegisterGameModel(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        public static void RegisterPlayfield(Playfield playfield)
        {
            _playfield = playfield;
        }

        public static void RegisterGameOverMenu(GameOver gameOver)
        {
            _gameOver = gameOver;
        }
    }
}