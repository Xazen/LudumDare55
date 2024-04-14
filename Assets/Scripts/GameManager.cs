using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Balancing balancing;

        private void Awake()
        {
            Singletons.RegisterBalancing(balancing);
            Singletons.RegisterGameModel(new GameModel());
        }

        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            if (GlobalSettings.PendingRestart)
            {
                Singletons.AudioManager.Restart(GlobalSettings.Difficulty);
                GlobalSettings.PendingRestart = false;
            }
            else
            {
                Singletons.AudioManager.StartGameMusic(GlobalSettings.Difficulty);
            }
            Singletons.PauseMenu.OnPaused += OnPaused;
            Singletons.GameModel.OnHealthZero += OnHealthZero;
            Singletons.AudioManager.WonTrigger += OnWonTrigger;
            Singletons.AudioManager.LostTrigger += OnLostTrigger;
        }

        private void OnDestroy()
        {
            Singletons.PauseMenu.OnPaused -= OnPaused;
            Singletons.GameModel.OnHealthZero -= OnHealthZero;
            Singletons.AudioManager.WonTrigger -= OnWonTrigger;
            Singletons.AudioManager.LostTrigger -= OnLostTrigger;
        }

        private void OnLostTrigger()
        {
            if (!Singletons.GameOver.IsOpen)
            {
                Singletons.GameOver.OpenGameOver(false);
            }
        }

        private void OnWonTrigger(AudioManager.WonTriggers obj)
        {
            if (obj == AudioManager.WonTriggers.PointsScreen)
            {
                GlobalSettings.BlockInput = false;
                if (!Singletons.GameOver.IsOpen)
                {
                    Singletons.GameOver.OpenGameOver(true);
                }
            }
        }

        private void OnHealthZero(int _)
        {
            Pause();
            Singletons.AudioManager.GameOverSound();
            if (!Singletons.GameOver.IsOpen)
            {
                Singletons.GameOver.OpenGameOver(false);
            }
        }

        private void OnPaused(bool isPaused)
        {
            if (isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

        private static void Resume()
        {
            Singletons.AudioManager.ResumeMusic();
            Singletons.NoteController.Resume();
        }

        private static void Pause()
        {
            Singletons.AudioManager.PauseMusic();
            Singletons.NoteController.Pause();
        }
    }
}