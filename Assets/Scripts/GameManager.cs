using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private AudioManager.Difficulty difficulty;
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
            Singletons.AudioManager.StartGameMusic(difficulty);
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