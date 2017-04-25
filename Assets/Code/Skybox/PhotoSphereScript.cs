#region Usings

using UnityEngine;
using System.Collections;

#endregion

[RequireComponent(typeof(Renderer))]
public class PhotoSphereScript : MonoBehaviour, IResizable {

    #region Public Properties

    public PhotoSphere photoSphere;
    public bool isPhotoSphereSet = false;

    #endregion

    #region Methods

    private void SetScale()
    {
        float scaleX = photoSphere.scaleX;
        scaleX -= transform.localScale.x;

        float scaleY = photoSphere.scaleY;
        scaleY -= transform.localScale.y;

        float scaleZ = photoSphere.scaleZ;
        scaleZ -= transform.localScale.z;

        transform.localScale += new Vector3(scaleX, scaleY, scaleZ);
    }

    private void SetMaterial()
    {
        GetComponent<Renderer>().material = (Material)Resources.Load(photoSphere.GetMaterialFullPath(), typeof(Material));
    }

    public void SetPhotoSphere(PhotoSphere photoSph)
    {
        this.photoSphere = new PhotoSphere(photoSph);
        isPhotoSphereSet = true;
        //SetScale();
        SetMaterial();
    }

    public void LoadFromPhotoSphereObject()
    {
        if (photoSphere != null)
        {
            transform.position = photoSphere.GetSavedPosition();
            transform.rotation = photoSphere.GetSavedRotation();
            transform.localScale = photoSphere.GetSavedScale();
        }
    }


    public void SetPhotoSphereObjectByNetwork(PhotoSphere img)
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("CreateAndSetPhotoSphereObject", PhotonTargets.Others, img.scaleX, img.scaleY, img.scaleZ, img.materialName);
        }
        
    }

    [PunRPC]
    public void CreateAndSetPhotoSphereObject(float scaleX, float scaleY, float scaleZ, string name)
    {
        SetPhotoSphere(new PhotoSphere(scaleX, scaleY,scaleZ, name));
    }

    public string GetResizableObjectPrefabName()
    {
        return "PhotoSphereOnResize";
    }

    public void SetModyficableObject(ModyficableObject appearanceObject)
    {
        SetPhotoSphere((PhotoSphere)appearanceObject);
    }

    public ModyficableObject GetModyficableObject()
    {
        return photoSphere;
    }


    public void SetMaterial(Material mat)
    {

    }

    public void UpdateActualRatio(float width, float height)
    {
        photoSphere.UpdateActualRatio(width, height);
    }

    public void SetModyficableObjectByNetwork()
    {
        SetPhotoSphereObjectByNetwork(photoSphere);
    }

    #endregion
}
