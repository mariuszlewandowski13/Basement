using UnityEngine;
using System.Collections;

public class Shapes3DMiniatureScript : MonoBehaviour {

    #region Public Properties
    public Shape3DObject shape3D;
    #endregion

    #region Methods
    public void Set3DShape(Shape3DObject shape)
    {
        this.shape3D = shape;
        GetComponent<SceneObjectScript>().miniaturedSceneObject = shape;

        Color color = GetColorFromToolbar();

        UpdateColor(color);

        GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").FindChild("panel_color").GetComponent<ToolbarMainColorPickerScript>().colorChange += UpdateColor;

    }

    public Vector3 GetWorldObjectScale()
    {
        return GameObject.Find("Player").GetComponent<WorldObjects>().shapes3DObjects[shape3D.shape3DObjectNumber].transform.lossyScale;
    }

    private Color GetColorFromToolbar()
    {
        return GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").FindChild("panel_color").GetComponent<ToolbarMainColorPickerScript>().color;
    }

    private void UpdateColor(Color color)
    {
        shape3D.color = color;
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
