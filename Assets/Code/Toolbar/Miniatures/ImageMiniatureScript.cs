#region Usings

using UnityEngine;
using System.Collections;
using System;

#endregion

public class ImageMiniatureScript : MonoBehaviour {

    #region Private Properties

    private float sizeTreshold = 0.065f;

    #endregion

    #region Methods
    private void LoadImgAsMaterialForMiniature(ImageObject img)
    {
        StartCoroutine(LoadTexture(img.GetMiniatureTextureName(), img.loadType, img));
    }

    public void SetMiniatureImageObject(ImageObject img)
    {
        gameObject.GetComponent<SceneObjectScript>().miniaturedSceneObject = img;
        LoadImgAsMaterialForMiniature(img);
        SetScaleRatio(img);
    }

    private void SetScaleRatio(ImageObject imageObject)
    {
        float heightScale = transform.localScale.z;

        heightScale *= imageObject.realRatio;
        heightScale -= transform.localScale.z;


        transform.localScale += new Vector3(heightScale, 0.0f, 0.0f);

        if (transform.localScale.x > sizeTreshold) transform.localScale -= new Vector3(transform.localScale.x - sizeTreshold, 0.0f, (transform.localScale.x - sizeTreshold) / imageObject.realRatio);
        if (transform.localScale.z > sizeTreshold) transform.localScale -= new Vector3((transform.localScale.z - sizeTreshold) * imageObject.realRatio, 0.0f, transform.localScale.z - sizeTreshold);
    }

    public Vector3 GetNewImageScalesRatio()
    {
        ImageObject imageObject = (ImageObject)gameObject.GetComponent<SceneObjectScript>().miniaturedSceneObject;
        GameObject imagePrefab = GameObject.Find("Player").GetComponent<WorldObjects>().imageObject;
        Vector3 newScale;

        newScale = imagePrefab.transform.localScale;

        Vector3 scale = imageObject.GetSavedScale();
        if (scale != new Vector3())
        {
            newScale = scale;
        }
        else
        {
            float heightScale = newScale.z;
            heightScale *= imageObject.realRatio;
            heightScale -= newScale.z;
            newScale += new Vector3(heightScale, 0.0f, 0.0f);
        }
        return newScale;

    }

    IEnumerator LoadTexture(string name, LoadingType loadType, ImageObject img)
    {
        if (loadType == LoadingType.remote)
        {
            WWW www = new WWW(name);
            yield return www;
          //  Debug.Log(name);
            if (www.texture != null && www.error == null)
            {
                try
                {
                    GetComponent<Renderer>().material.mainTexture = www.texture;
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    throw;
                }
            }
        } else if (loadType == LoadingType.local)
        {
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage( img.LoadImgFromBytes());
            GetComponent<Renderer>().material.mainTexture = tex;
        }
    }



    #endregion
}
