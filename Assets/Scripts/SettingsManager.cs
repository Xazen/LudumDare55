using UnityEngine;

namespace DefaultNamespace
{
    public class SettingsManager : MonoBehaviour
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

        private void Update()
        {
            if (Singletons.GameModel != null)
            {
                var notes = Singletons.GameModel.GetPerfectNotes();
                foreach (var note in notes)
                {
                    PlayNote(note.TrackIndex);
                }
            }
        }

        private void StartGame()
        {
            Singletons.AudioManager.ToCalibration(GlobalSettings.Difficulty);
        }

        private void PlayNote(int trackIndex)
        {
            Singletons.Playfield.AnimateTracker(trackIndex);
            Singletons.GameModel.PlayNote(trackIndex);
        }
    }
}