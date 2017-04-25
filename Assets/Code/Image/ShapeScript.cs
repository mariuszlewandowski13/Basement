#region Usings

using UnityEngine;
using System;

#endregion


[RequireComponent(typeof(Renderer))]
[Serializable]
public class ShapeScript : MonoBehaviour, IResizable {

    #region Public Properties

    public ShapeObject shapeObject;

    private bool materialSet;
    private bool colorSet;

    Texture2D newMaterial;

    #endregion

    #region Methods
    void Start()
    {
        gameObject.GetComponent<Renderer>().sortingLayerName = "PhysicalObjectsSortingLayer";
        if(gameObject.transform.FindChild("Plane") != null) gameObject.transform.FindChild("Plane").GetComponent<Renderer>().sortingLayerName = "PhysicalObjectsSortingLayer";
    }

    private void LoadShapeAsMaterial()
    {
        newMaterial = shapeObject.GetShapeAsTexture();
        materialSet = true;
    }

    private void LoadMaterialToRenderer()
    {
        if (transform.FindChild("Plane") != null)
        {
            transform.FindChild("Plane").GetComponent<Renderer>().material.mainTexture = newMaterial;
        }

        GetComponent<Renderer>().material.mainTexture = newMaterial;
    }

    private void SetScaleRatio()
    {
        Vector3 scale = shapeObject.GetSavedScale();
        if (scale != new Vector3())
        {
            gameObject.transform.localScale = scale;
        }
        else
        {
            float heightScale = transform.localScale.z;
            heightScale *= shapeObject.realRatio;
            heightScale -= transform.localScale.z;
            transform.localScale += new Vector3(heightScale, 0.0f, 0.0f);
        }

    }

    void Update()
    {
        if (materialSet)
        {
            LoadMaterialToRenderer();
            materialSet = false;
        }
    }

    private void SetColorToRenderer()
    {
        if (transform.FindChild("Plane") != null)
        {
            transform.FindChild("Plane").GetComponent<Renderer>().material.SetColor("_Color", shapeObject.color);
        }
        GetComponent<Renderer>().material.SetColor("_Color", shapeObject.color);
        //SetColorToRenderer();
    }

    public void UpdateColor(Color color)
    {
        shapeObject.color = color;
        SetColorToRenderer();
        UpdateColorByNetwork();
    }

    

    private void SetColorToShapeObject(Color color)
    {
        shapeObject.color = color;
    }

    public void SetShapeObject(object shape, float alpha = 1.0f)
    {
        
        this.shapeObject = new ShapeObject((ShapeObject)shape);
        shapeObject.color.a = alpha;
        LoadShapeAsMaterial();
        UpdateColor(shapeObject.color);
    }

    public void LoadFromShapeObject()
    {
        if (shapeObject != null)
        {
            transform.position = shapeObject.GetSavedPosition();
            transform.rotation = shapeObject.GetSavedRotation();
            transform.localScale = shapeObject.GetSavedScale();
        }
    }

    public void SetShapeObjectByNetwork(ShapeObject img)
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("CreateAndSetShapeObject", PhotonTargets.Others, img.realWidth, img.realHeight, img.imgName);
        }
        
        UpdateColorByNetwork();
    }
    public void UpdateColorByNetwork()
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("UpdateColorFromNetwork", PhotonTargets.Others, shapeObject.color.r, shapeObject.color.g, shapeObject.color.b, shapeObject.color.a);
        }
        
    }

    [PunRPC]
    public void CreateAndSetShapeObject(int width, int height, string name)
    {
        SetShapeObject(new ShapeObject(width, height, name, ApplicationStaticData.shapesPath));
    }

    [PunRPC]
    public void UpdateColorFromNetwork(float r, float g, float b, float a)
    {
        Color color = new Color(r, g, b, a);

        shapeObject.color = color;
        SetColorToRenderer();
    }

    public string GetResizableObjectPrefabName()
    {
        return "ShapeOnResize";
    }

    public void SetModyficableObject(ModyficableObject appearanceObject)
    {
        SetShapeObject((ShapeObject)appearanceObject, 0.5f);
    }

    public ModyficableObject GetModyficableObject()
    {
        return shapeObject;
    }


    public void SetMaterial(Material mat)
    {

    }

    public void UpdateActualRatio(float width, float height)
    {
        shapeObject.UpdateActualRatio(width, height);
    }

    public void SetModyficableObjectByNetwork()
    {
        SetShapeObjectByNetwork(shapeObject);
    }



    #endregion
}
