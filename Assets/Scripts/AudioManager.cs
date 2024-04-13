using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public const float TimeOffset = 8f;

    [SerializeField]private AK.Wwise.Event MusicEvent;
    [HideInInspector] public Action<int> RhythmCallback;

    void Start()
    {
        MusicEvent.Post(gameObject, (uint)AkCallbackType.AK_MIDIEvent, MidiCallback);
    }

    private void MidiCallback(object in_cookie, AkCallbackType in_type, object in_info)
    {
        var midiEvent = (AkMIDIEventCallbackInfo)in_info;
        if(midiEvent.byParam1 >= 36 && midiEvent.byParam1 <= 39 && midiEvent.byParam2 == 127)
        {
            RhythmCallback?.Invoke(midiEvent.byParam1 - 36);
            Debug.Log("HIT: " + (midiEvent.byParam1 - 36));
        }
    }
}
