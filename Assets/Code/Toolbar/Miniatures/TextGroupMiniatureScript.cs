using UnityEngine;
using System.Collections;

public class TextGroupMiniatureScript : MonoBehaviour {


    #region Public Properties
    public TextGroupObject textGroup;
    #endregion

    #region Methods
    public void SetTextGroup(TextGroupObject textGroup)
    {
        this.textGroup = textGroup;
        GetComponent<SceneObjectScript>().miniaturedSceneObject = textGroup;
        UpdateColor(GetColorFromToolbar());
        GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").FindChild("panel_color").GetComponent<ToolbarMainColorPickerScript>().colorChange += UpdateColor;
    }



    private Color GetColorFromToolbar()
    {
        return GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").FindChild("panel_color").GetComponent<ToolbarMainColorPickerScript>().color;
    }

    private void UpdateColor(Color color)
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Renderer>() != null)
            {
                child.GetComponent<Renderer>().material.SetColor("_Color", color);
            }
            if (child.GetComponent<TextMesh>() != null)
            {
                child.GetComponent<TextMesh>().color = color;
            }
        }

        textGroup.color = color;
    }

    void OnDestroy()
    {
        if (GameObject.Find("Player") != null)
        {
            GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").FindChild("panel_color").GetComponent<ToolbarMainColorPickerScript>().colorChange -= UpdateColor;
        }
        
    }

    public Vector3 GetNewTextGroupScalesRatio()
    {
        GameObject imagePrefab = GameObject.Find("Player").GetComponent<WorldObjects>().textGroupObjects[textGroup.textGroupObjectNumber];
        return imagePrefab.transform.localScale;
    }

    #endregion


}
