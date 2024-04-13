using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            Singletons.RegisterGameManager(this);
        }

        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            Singletons.ResetGameModel(new GameModel());
            Singletons.AudioManager.StartGameMusic();
            Singletons.PauseMenu.OnPaused += OnPaused;
            Singletons.GameModel.OnHealthZero += OnHealthZero;
        }

        private void OnHealthZero(int _)
        {
            Pause();
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