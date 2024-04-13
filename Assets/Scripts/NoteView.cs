using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text debugTrackText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTrack(int trackIndex)
    {
        debugTrackText.text = trackIndex.ToString();
    }
}
