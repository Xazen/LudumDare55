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
            Singletons.AudioManager.StartGameMusic();
            Singletons.PauseMenu.OnPaused += OnPaused;
        }

        private void OnPaused(bool isPaused)
        {
            if (isPaused)
            {
                Singletons.AudioManager.PauseMusic();
                Singletons.NoteController.Pause();
            }
            else
            {
                Singletons.AudioManager.ResumeMusic();
                Singletons.NoteController.Resume();
            }
        }
    }
}