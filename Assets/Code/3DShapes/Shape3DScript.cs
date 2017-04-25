#region Usings
using UnityEngine;
using System;
#endregion

/*
    Script for handling 3DShapeObject operations
*/

[Serializable]
public class Shape3DScript : MonoBehaviour, IResizable {

    #region Public Properties

    public Shape3DObject shapeObject;// Our actual shape3DObject

    #endregion

    #region Private Properties

    private Renderer rend;//We keep reference on Renderer, so we don't have to use GetComponent<> each time

    #endregion

    #region Methods

    void Awake()
    {

        rend = GetComponent<Renderer>();

        //setting layer for camera rendering 
        if (rend != null)
        {
            rend.sortingLayerName = "PhysicalObjectsSortingLayer";
        }
    }

    public void SetShape3DObject(Shape3DObject shape, float alpha = 1.0f)
    {
        this.shapeObject = new Shape3DObject(shape);
        shapeObject.color.a = alpha;
        UpdateColor(shapeObject.color);
        UpdateColorByNetwork();
    }

    

    public void SetColor(Color color)
    {
        shapeObject.color = color;
        UpdateColor(color);
        UpdateColorByNetwork();
    }

    public void LoadFromShape3DObject()
    {
        if (shapeObject != null)
        {
            transform.position = shapeObject.GetSavedPosition();
            transform.rotation = shapeObject.GetSavedRotation();
            transform.localScale = shapeObject.GetSavedScale();
        }
    }

    private void UpdateColor(Color color)
    {
        rend.material.SetColor("_Color",  shapeObject.color);
    }

    public void UpdateColorByNetwork()
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("UpdateShape3DColorFromNetwork", PhotonTargets.Others, shapeObject.color.r, shapeObject.color.g, shapeObject.color.b, shapeObject.color.a);
        }
        
    }

    [PunRPC]
    public void UpdateShape3DColorFromNetwork(float r, float g, float b, float a)
    {
        Color color = new Color(r, g, b, a);
        shapeObject.color = color;
        UpdateColor(color);
    }

    public void SetShape3DObjectByNetwork()
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("SetShape3DObjectFromNetwork", PhotonTargets.Others, shapeObject.shape3DObjectNumber, shapeObject.color.r, shapeObject.color.g, shapeObject.color.b, shapeObject.color.a);
        }
       
    }

    [PunRPC]
    public void SetShape3DObjectFromNetwork(int number, float r, float g, float b, float a)
    {
        Color color = new Color(r, g, b, a);
        shapeObject = new Shape3DObject(number, color);
        UpdateColor(color);
    }

    public string GetResizableObjectPrefabName()
    {
        return GameObject.Find("Player").GetComponent<WorldObjects>().shapes3DObjects[shapeObject.shape3DObjectNumber].name;
    }

    public void SetModyficableObject(ModyficableObject modyficableObject)
    {
        SetShape3DObject((Shape3DObject)modyficableObject, 0.5f);
    }

    public ModyficableObject GetModyficableObject()
    {
        return shapeObject;
    }

    public void SetMaterial(Material mat)
    {
        rend.material = mat;
    }

    public void UpdateActualRatio(float width, float height)
    {
    }

    public void SetModyficableObjectByNetwork()
    {
    }

    #endregion
}
