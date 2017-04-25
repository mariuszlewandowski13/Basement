#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class VideoMiniatureScript : MonoBehaviour {

    #region Private Properties

    public VideoObject videoObject;

    #endregion

    #region Methods
    private void LoadVideoForMiniature()
    {
        gameObject.GetComponent<MediaPlayerCtrl>().m_strFileName = videoObject.fileName;
    }

    public void SetMiniatureVideoObject(VideoObject video)
    {
        this.videoObject = new VideoObject(video);
        gameObject.GetComponent<SceneObjectScript>().miniaturedSceneObject = videoObject;
        LoadVideoForMiniature();
    }

    public Vector3 GetNewImageScalesRatio()
    {

      Vector3  newScale = gameObject.transform.localScale *2.0f;
      return newScale;
    }



    #endregion
}
