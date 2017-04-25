#region Usings

using UnityEngine;
using System;
using System.Collections;
using System.Threading;
using System.Net;
using System.Runtime.InteropServices;

#endregion


[RequireComponent(typeof(Renderer))]
[Serializable]
public class ImageScript : MonoBehaviour, IResizable {

    #region Public Properties

    public ImageObject imageObject;
    private bool textureReady;
    private bool colorArrayReady;

    private Color32 [] colorArray;

    #endregion

    #region Methods

    void Start()
    {
        gameObject.GetComponent<Renderer>().sortingLayerName = "PhysicalObjectsSortingLayer";
    }

    private void LoadImgAsMaterial()
    {
        StartCoroutine(LoadTexture(imageObject.imgName, imageObject.loadType));
    }

    void Update()
    {
        if (textureReady)
        {
           // Debug.Log("asdasd");
            GetComponent<Renderer>().material.mainTexture = imageObject.texture;
            textureReady = false;
        }

        if (colorArrayReady)
        {
            imageObject.texture.SetPixels32(colorArray);
            colorArrayReady = false;
            textureReady = true;
        }

    }

    private void SetScaleRatio()
    {
        Vector3 scale = imageObject.GetSavedScale();
        if (scale != new Vector3())
        {
            gameObject.transform.localScale = scale;
        }
        else
        {
            float heightScale = transform.localScale.z;
            heightScale *= imageObject.realRatio;
            heightScale -= transform.localScale.z;
            transform.localScale += new Vector3(heightScale, 0.0f, 0.0f);
        }
        
    }

    
    public void SetImageObject(ImageObject img)
    {
        this.imageObject = new ImageObject(img);
        LoadImgAsMaterial();
        //SetScaleRatio();
    }

    public void LoadFromImageObject()
    {
        if (imageObject != null)
        {
            transform.position = imageObject.GetSavedPosition();
            transform.rotation = imageObject.GetSavedRotation();
            transform.localScale = imageObject.GetSavedScale();
        }
    }


    public void SetImageObjectByNetwork(ImageObject img)
    {
        if (GetComponent<PhotonView>() != null && PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("CreateAndSetImageObject", PhotonTargets.Others, img.realWidth, img.realHeight, img.imgName, (int)img.loadType);
        }
    }

    [PunRPC]
    public void CreateAndSetImageObject(int width, int height, string name, int loadType)
    {
        SetImageObject(new ImageObject(width, height, name, ApplicationStaticData.imagesPath, (LoadingType)loadType));
    }


    IEnumerator LoadTexture(string name, LoadingType loadType)
    {
        if (imageObject.texture == null)
        {
            imageObject.texture = new Texture2D(2, 2);
            if (loadType == LoadingType.remote)
            {
                WWW www = new WWW(name);
                yield return www;

               // Debug.Log(name);

                if (www.error != null)
                {
                    ObjectSpawnerScript.DestroyGameObject(gameObject, true);
                }
                else {
                      try
                      {
                        www.LoadImageIntoTexture(imageObject.texture);
                        textureReady = true;
                        //Thread th = new Thread(LoadTextureFromUrl);
                        //th.Start();
                    }
                      catch (Exception e)
                      {
                            ObjectSpawnerScript.DestroyGameObject(gameObject);
                            Debug.Log(e);
                      }
                }
                www.Dispose();         
            }
            else if (loadType == LoadingType.local)
            {
                Texture2D tex = new Texture2D(2, 2);
                byte[] bytesTex = imageObject.LoadImgFromBytes();
                
                if (bytesTex == null)
                {
                    ObjectSpawnerScript.DestroyGameObject(gameObject);
                }
                else {
                    tex.LoadImage(bytesTex);
                    imageObject.texture = tex;
                    textureReady = true;
                }
                
            }
        }
        else {
            textureReady = true;
        }
       
    }

    void LoadTextureFromUrl()
    {
        var webClient = new WebClient();
        byte[] imageBytes = webClient.DownloadData(imageObject.imgName);
        
        colorArray = ByteArrayToColor32(imageBytes);
        
        //for (var i = 0; i < imageBytes.Length; i += 4)
        //{
        //    Color color = new Color(imageBytes[i + 0], imageBytes[i + 1], imageBytes[i + 2], imageBytes[i + 3]);
        //    colorArray[i / 4] = color;
        //}

        colorArrayReady = true;
    }

    private static Color32[] ByteArrayToColor32(byte[] colors)
    {
        if (colors == null || colors.Length == 0)
            return null;

        int lengthOfColor32 = Marshal.SizeOf(typeof(Color32));
        int length = colors.Length / lengthOfColor32;
        
        Color32[] bytes = new Color32[length];

        GCHandle handle = default(GCHandle);
        try
        {
            handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            IntPtr ptr = handle.AddrOfPinnedObject();
            Marshal.Copy(colors, 0, ptr, length);
        }
        finally
        {
            if (handle != default(GCHandle))
                handle.Free();
        }

        return bytes;
    }

    public string GetResizableObjectPrefabName()
    {
        return "ImageOnResize";
    }

    public void SetModyficableObject(ModyficableObject appearanceObject)
    {
        SetImageObject((ImageObject)appearanceObject);
    }

    public ModyficableObject GetModyficableObject()
    {
        return imageObject;
    }
    public void SetMaterial(Material mat)
    {

    }

    public void UpdateActualRatio(float width, float height)
    {
        imageObject.UpdateActualRatio(width, height);
    }

    public void SetModyficableObjectByNetwork()
    {
        SetImageObjectByNetwork(imageObject);
    }

    #endregion
}
