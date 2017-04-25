#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class DrawingLinesMiniatureScript : MonoBehaviour {

    #region Private Properties

    private bool active = false;
    public ToolsObject drawer;
    private bool isSet = false;

    #endregion

    #region Methods
    private void LoadDrawer()
    {
        Renderer rend = GetComponent<Renderer>();

        Material mat = GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").GetComponent<LinesTexturesScript>().linesMaterials[((LineDrawingObject)drawer).textureNumber];
        Texture tex = GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").GetComponent<LinesTexturesScript>().lines[((LineDrawingObject)drawer).textureNumber];

        if (mat != null)
        {
            rend.material = mat;
        }

        if (tex != null)
        {
            rend.material.mainTexture = tex;
        }
       
        Color color = GetColorFromToolbar();

        UpdateColor(color);
        isSet = true;
        GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").FindChild("panel_color").GetComponent<ToolbarMainColorPickerScript>().colorChange += UpdateColor;
    }

    public void SetMiniatureDrawerObject(ToolsObject drawer)
    {
        this.drawer = drawer;
        LoadDrawer();
    }

    void Start()
    {
        GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision;
    }

    void Update()
    {
        if (active && GetComponent<HighlightingSystem.Highlighter>() != null)
        {
            GetComponent<HighlightingSystem.Highlighter>().SeeThroughOff();
            GetComponent<HighlightingSystem.Highlighter>().On();
        }
    }

    private void ControllerCollision(GameObject gameObj, bool isEnter)
    {
        if (isEnter && !active)
        {
            gameObj.GetComponent<ControlObjects>().TriggerDown += StartDraw;
            active = true;
        }
        else if (!isEnter && !GetComponent<ObjectInteractionScript>().GetIsSelected() && active)
        {
            gameObj.GetComponent<ControlObjects>().TriggerDown -= StartDraw;
            active = false;
        }
    }

    public void StartDraw(GameObject controller)
    {
       //Color color = GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").FindChild("panel_color").GetComponent<ToolbarMainColorPickerScript>().color;
       GameObject.Find("Player").GetComponent<ObjectSpawnerScript>().SpawnObject(drawer, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.localScale, controller);
    }

    private Color GetColorFromToolbar()
    {
        return GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").FindChild("panel_color").GetComponent<ToolbarMainColorPickerScript>().color;
    }

    private void UpdateColor(Color color)
    {
        ((LineDrawingObject)drawer).color = color;
        //GetComponent<Renderer>().material.SetColor("_Color", color);
        //GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
    }

    void OnDestroy()
    {
        if (isSet && GameObject.Find("Player") != null)
        {
            GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").FindChild("panel_color").GetComponent<ToolbarMainColorPickerScript>().colorChange -= UpdateColor;
        }
    }

    #endregion
}
