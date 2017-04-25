#region Usings

using UnityEngine;
using System.Collections;

#endregion


[RequireComponent(typeof(Renderer))]
public class PhotoSphereMiniatureScript : MonoBehaviour {

    #region Public Properties

    public PhotoSphere photoSphere;

    #endregion

    #region Methods

    public void SetPhotoSphere(PhotoSphere photoSph)
    {
        gameObject.GetComponent<SceneObjectScript>().miniaturedSceneObject = photoSph;
        this.photoSphere = photoSph;
        SetMaterial();
    }

    private void SetMaterial()
    {
        StartCoroutine(LoadTexture());
    }

    IEnumerator LoadTexture()
    {
        GetComponent<Renderer>().material = (Material)Resources.Load(photoSphere.GetMaterialFullPath(), typeof(Material));
        yield return null;

    }

    #endregion
}
