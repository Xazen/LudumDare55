using System;
using DefaultNamespace;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public enum WonTriggers
    {
        MusicEnd,
        Shenlon,
        RIP,
        PointsScreen
    }

    public const float TimeOffset = 6.21359223301f;

    [SerializeField] private AK.Wwise.Event MusicEvent;
    [HideInInspector] public Action<int, bool> RhythmCallback;
    [HideInInspector] public Action<bool> GameEnded;
    [HideInInspector] public Action<WonTriggers> WonTrigger;

    private bool checkWonCues = false;

    private void Awake()
    {
        Debug.Log("AudioManager Awake");
        if (Singletons.AudioManager == null)
        {
            DontDestroyOnLoad(gameObject);
            Singletons.RegisterAudioManager(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("AudioManager Start");
        AkSoundEngine.SetState("GameState", "MainMenu");
        PostMusicEvent();
    }

    /// <summary>
    /// Start Level Music, set difficult via param
    /// </summary>
    /// <param name="difficulty"> between 0 and 2, 0 is easiest</param>
    public void StartGameMusic(SongDifficulty difficulty)
    {
        Debug.Log("AudioManager StartGameMusic");
        checkWonCues = false;
        AkSoundEngine.SetState("Difficulty", difficulty.ToString());
        Debug.Log("Difficulty: " + difficulty.ToString());
        AkSoundEngine.SetState("GameState", "StartGame");
    }

    public void Restart(SongDifficulty difficulty)
    {
        StopMusic();
        StartGameMusic(difficulty);
        PostMusicEvent();
    }
    
    public void ToMenu()
    {
        Debug.Log("AudioManager ToMenu");
        AkSoundEngine.SetState("GameState", "MainMenu");
        checkWonCues = false;
    }    

    public void ToCalibration(SongDifficulty difficulty)
    {
        Debug.Log("AudioManager ToCalibration");
        checkWonCues = false;
        AkSoundEngine.PostEvent("StopGameMusic", gameObject);
        StartGameMusic(difficulty);
        PostMusicEvent();
    }
    
    public void StopMusic()
    {
        Debug.Log("AudioManager StopMusic");
        AkSoundEngine.PostEvent("StopGameMusic_Immeditate", gameObject);
        checkWonCues = false;
    }    

    public void PauseMusic()
    {
        Debug.Log("AudioManager PauseMusic");
        AkSoundEngine.PostEvent("Pause", gameObject);
    }

    public void ResumeMusic()
    {
        Debug.Log("AudioManager ResumeMusic");
        AkSoundEngine.PostEvent("Resume", gameObject);
    }

    public void PlayDragonballSound()
    {
        AkSoundEngine.PostEvent("DragonballTrigger", gameObject);
    }

    public void Play_Hit(int arrowNumber)
    {
        AkSoundEngine.PostEvent("Play_hit", gameObject);
    }

    public void Play_pressArrow(int arrowNumber)
    {
        AkSoundEngine.PostEvent("Play_reflect_deflect", gameObject);
    }

    private void PostMusicEvent()
    {
        MusicEvent.Post(gameObject, (uint)AkCallbackType.AK_MIDIEvent | (uint)AkCallbackType.AK_MusicSyncUserCue | (uint)AkCallbackType.AK_MusicSyncExit, MidiCallback);
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
            if (((AkMusicSyncCallbackInfo)in_info).userCueName == "WON")
            {
                checkWonCues = true;
                WonTrigger.Invoke(WonTriggers.MusicEnd);
            }
            if (checkWonCues && ((AkMusicSyncCallbackInfo)in_info).userCueName == "Shenlon")
            {
                WonTrigger.Invoke(WonTriggers.Shenlon);
            }
            if (checkWonCues && ((AkMusicSyncCallbackInfo)in_info).userCueName == "RIP")
            {
                WonTrigger.Invoke(WonTriggers.RIP);
            }
        }
        if(checkWonCues && in_type == AkCallbackType.AK_MusicSyncExit)
        {
            WonTrigger.Invoke(WonTriggers.PointsScreen);
        }
    }
}
