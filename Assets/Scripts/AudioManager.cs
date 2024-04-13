using System;
using DefaultNamespace;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public const float TimeOffset = 6.21359223301f;

    [SerializeField]private AK.Wwise.Event MusicEvent;
    [HideInInspector] public Action<int, bool> RhythmCallback;

    private void Awake()
    {
        Singletons.RegisterAudioManager(this);
    }

    /// <summary>
    /// Start Level Music, set difficult via param
    /// </summary>
    /// <param name="difficulty"> between 0 and 2, 0 is easiest</param>
    public void StartGameMusic(int difficulty = 1)
    {
        if(difficulty >= 0 && difficulty < 3)
        { 
            AkSoundEngine.SetRTPCValue("Difficulty", difficulty); 
        }        
        MusicEvent.Post(gameObject, (uint)AkCallbackType.AK_MIDIEvent | (uint)AkCallbackType.AK_MusicSyncUserCue, MidiCallback);
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
        //if (in_type == AkCallbackType.AK_MusicSyncUserCue)
        //{
        //    if()
        //}
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
