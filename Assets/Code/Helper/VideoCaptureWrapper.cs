#region Usings

using UnityEngine;
using System.Collections;

#endregion


public class VideoCaptureWrapper : MonoBehaviour {

    #region Private Properties

    private CapturePanorama.CapturePanorama capturePanorama;
    //private string screenShotsFolderPath = "Assets/Artwork/CapturedVideos/";

    #endregion

    #region Methods
    void Start()
    {
        capturePanorama = gameObject.GetComponent<CapturePanorama.CapturePanorama>();
    }


    void StartCaptureVideo()
    {
        capturePanorama.StartCaptureEveryFrame();
    }

    void EndCaptureVideo()
    {
        capturePanorama.StopCaptureEveryFrame();
    }
    #endregion
}
