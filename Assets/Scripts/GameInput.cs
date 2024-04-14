using UnityEngine;

namespace DefaultNamespace
{
    public class GameInput : MonoBehaviour
    {
        private void Update()
        {
            if (Singletons.PauseMenu.IsPaused || GlobalSettings.BlockInput)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                PlayNote(0);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                PlayNote(1);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                PlayNote(2);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                PlayNote(3);
            }
        }

        private void PlayNote(int trackIndex)
        {
            Singletons.AudioManager.Play_pressArrow(trackIndex);
            Singletons.Playfield.AnimateTracker(trackIndex);

            if (Singletons.GameModel.HasNote(trackIndex))
            {
                Singletons.GameModel.PlayNote(trackIndex);
            }
        }
    }
}