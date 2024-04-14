using UnityEngine;

namespace DefaultNamespace
{
    public class GameInput : MonoBehaviour
    {
        private void Update()
        {
            if (Singletons.PauseMenu.IsPaused)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PlayNote(0);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                PlayNote(1);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                PlayNote(2);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                PlayNote(3);
            }
        }

        private void PlayNote(int trackIndex)
        {
            Singletons.Playfield.AnimateTracker(trackIndex);
            Singletons.GameModel.PlayNote(trackIndex);
        }
    }
}