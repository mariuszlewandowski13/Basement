#region Usings

using UnityEngine;
using System.Collections;

#endregion

[RequireComponent(typeof(Renderer))]
public class AudioMiniatureScript : MonoBehaviour {

    #region Methods
    void Start()
    {
        LoadImgAsMaterial();
    }

    private void LoadImgAsMaterial()
    {
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(AudioImageObject.GetAudioImageAsByteArray());
        GetComponent<Renderer>().material.mainTexture = tex;
    }

    public void SetAudioMute(bool isMuted)
    {
        gameObject.transform.GetChild(0).GetComponent<AudioSource>().mute = isMuted;
    }
    #endregion
}
