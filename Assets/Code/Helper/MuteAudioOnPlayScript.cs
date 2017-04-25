using UnityEngine;
using System.Collections;

public class MuteAudioOnPlayScript : MonoBehaviour {

    private bool muted = false;

    void Update()
    {
        if (GetComponent<AudioSource>() != null && !muted)
        {
            GetComponent<AudioSource>().mute = true;
            muted = true;
        }
    }
}
