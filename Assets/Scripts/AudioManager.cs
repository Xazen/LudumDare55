using System;
using DefaultNamespace;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public const float TimeOffset = 6.21359223301f;

    [SerializeField] private AK.Wwise.Event MusicEvent;
    [HideInInspector] public Action<int, bool> RhythmCallback;
    [HideInInspector] public Action<bool> GameEnded;

    private void Awake()
    {
        Singletons.RegisterAudioManager(this);
    }

    private void Start()
    {
        AkSoundEngine.SetState("GameState", "MainMenu");
        MusicEvent.Post(gameObject, (uint)AkCallbackType.AK_MIDIEvent | (uint)AkCallbackType.AK_MusicSyncUserCue, MidiCallback);
    }

    /// <summary>
    /// Start Level Music, set difficult via param
    /// </summary>
    /// <param name="difficulty"> between 0 and 2, 0 is easiest</param>
    public void StartGameMusic(SongDifficulty difficulty)
    {
        AkSoundEngine.SetState("Difficulty", difficulty.ToString());
        Debug.Log("Difficulty: " + difficulty.ToString());
        AkSoundEngine.SetState("GameState", "StartGame");
    }
    
    public void ToMenu()
    {
        AkSoundEngine.SetState("GameState", "MainMenu");
    }

    public void ToCalibration(SongDifficulty difficulty)
    {
        AkSoundEngine.PostEvent("StopGameMusic", gameObject);
        StartGameMusic(difficulty);
        MusicEvent.Post(gameObject, (uint)AkCallbackType.AK_MIDIEvent | (uint)AkCallbackType.AK_MusicSyncUserCue, MidiCallback);
    }
    
    public void StopMusic()
    {
        AkSoundEngine.PostEvent("StopGameMusic_Immeditate", gameObject);
    }

    private void MidiCallback(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if (in_type == AkCallbackType.AK_MIDIEvent)
        {
            var midiEvent = (AkMIDIEventCallbackInfo)in_info;
            if (midiEvent.byParam1 >= 36 && midiEvent.byParam1 <= 39 && midiEvent.byParam2 == 127)
            {
                RhythmCallback?.Invoke(midiEvent.byParam1 - 36, false);
                //Debug.Log("HIT: " + (midiEvent.byParam1 - 36));
            }
            if (midiEvent.byParam1 >= 40 && midiEvent.byParam1 <= 43 && midiEvent.byParam2 == 127)
            {
                RhythmCallback?.Invoke(midiEvent.byParam1 - 40, true);
                //Debug.Log("HIT: " + (midiEvent.byParam1 - 40));
            }
        }
        if (in_type == AkCallbackType.AK_MusicSyncUserCue)
        {
            if (((AkMusicSyncCallbackInfo)in_info).userCueName == "ENDE")
            {
                bool won = Singletons.GameModel.HaveAllDragonBalls();
                AkSoundEngine.SetState("GameState", won ? "Won" : "Lost");
                GameEnded.Invoke(won);
            }

        }
    }

    public void PauseMusic()
    {
        AkSoundEngine.PostEvent("Pause", gameObject);
    }

    public void ResumeMusic()
    {
        AkSoundEngine.PostEvent("Resume", gameObject);
    }
}
