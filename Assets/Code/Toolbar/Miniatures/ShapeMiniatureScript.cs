#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class ShapeMiniatureScript : MonoBehaviour {

    #region Private Properties

    private float sizeTreshold = 0.065f;

    #endregion


    #region Methods
    private void LoadShapeAsMaterialForMiniature(ShapeObject shape)
    {

        Texture2D tex = shape.GetShapeAsTexture();
        GetComponent<Renderer>().material.mainTexture = tex;

        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", shape.color);

    }

    public void SetMiniatureShapeObject(ShapeObject shape)
    {
        gameObject.GetComponent<SceneObjectScript>().miniaturedSceneObject = shape;
        LoadShapeAsMaterialForMiniature(shape);
        SetScaleRatio(shape);

        UpdateColor(GetColorFromToolbar());

        GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").FindChild("panel_color").GetComponent<ToolbarMainColorPickerScript>().colorChange += UpdateColor;
    }

    private void SetScaleRatio(ShapeObject shapeObject)
    {
        float heightScale = transform.localScale.z;

        heightScale *= shapeObject.realRatio;
        heightScale -= transform.localScale.z;


        transform.localScale += new Vector3(heightScale, 0.0f, 0.0f);

        if (transform.localScale.x > sizeTreshold) transform.localScale -= new Vector3(transform.localScale.x - sizeTreshold, 0.0f, (transform.localScale.x - sizeTreshold) / shapeObject.realRatio);
        if (transform.localScale.z > sizeTreshold) transform.localScale -= new Vector3((transform.localScale.z - sizeTreshold) * shapeObject.realRatio, 0.0f, transform.localScale.z - sizeTreshold);

    }


    public Vector3 GetNewImageScalesRatio()
    {
        ShapeObject shapeObject = (ShapeObject)gameObject.GetComponent<SceneObjectScript>().miniaturedSceneObject;
        GameObject shapePrefab = GameObject.Find("Player").GetComponent<WorldObjects>().shapeObject;
        Vector3 newScale;

        newScale = shapePrefab.transform.localScale;

        Vector3 scale = shapeObject.GetSavedScale();
        if (scale != new Vector3())
        {
            newScale = scale;
        }
        else
        {
            float heightScale = newScale.z;
            heightScale *= shapeObject.realRatio;
            heightScale -= newScale.z;
            newScale += new Vector3(heightScale, 0.0f, 0.0f);
        }
        return newScale;
    }

    private Color GetColorFromToolbar()
    {
        return GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").FindChild("panel_color").GetComponent<ToolbarMainColorPickerScript>().color;
    }

    private void UpdateColor(Color color)
    {
        ((ShapeObject)gameObject.GetComponent<SceneObjectScript>().miniaturedSceneObject).color = color;
        GetComponent<Renderer>().material.SetColor("_Color", color);
    }

    void OnDestroy()
    {
        if (GameObject.Find("Player") != null)
        {
            GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").FindChild("panel_color").GetComponent<ToolbarMainColorPickerScript>().colorChange -= UpdateColor;
        }
       
    }

    #endregion
}
