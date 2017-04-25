#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class ColorPickerScript : MonoBehaviour {

    #region Private Properties
    private GameObject objectToChangeColor;
    private bool active = false;
    private float brightness = 0.5f;

    #endregion

    #region Public Properties

    public Color color;
    

    #endregion


    #region Methods
    void Start()
    {
        GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision;

        objectToChangeColor = transform.parent.GetComponent<SaveTransformScript>().parentObject;
        color = objectToChangeColor.GetComponent<Renderer>().material.color;
        gameObject.GetComponent<Renderer>().sortingLayerName = "ToolsSortingLayer";
    }

    private void ControllerCollision(GameObject gameObj, bool isEnter)
    {
        if (isEnter && !active)
        {
            gameObj.GetComponent<ControlObjects>().TriggerDown += StartChangeColor;
            gameObj.GetComponent<ControlObjects>().TriggerUp += EndChangeColor;
            //objectToChangeColor.transform.FindChild("ScaleHandler").GetComponent<ObjectsFadeScript>().IncreaseFadeIn();
            active = true;
        }
        else if (!isEnter && !GetComponent<ObjectInteractionScript>().GetIsSelected() && active)
        {
            gameObj.GetComponent<ControlObjects>().TriggerDown -= StartChangeColor;
            gameObj.GetComponent<ControlObjects>().TriggerUp -= EndChangeColor;
            //objectToChangeColor.transform.FindChild("ScaleHandler").GetComponent<ObjectsFadeScript>().DecreaseFadeIn();
            active = false;
        }
    }

    private void StartChangeColor(GameObject controller)
    {
        GetComponent<ObjectInteractionScript>().SetIsSelected(true);
        //controller.GetComponent<ControlObjects>().ControllerMove += OnControllerMove;   
    }

    private void EndChangeColor(GameObject controller)
    {

        //controller.GetComponent<ControlObjects>().ControllerMove -= OnControllerMove;
        GetComponent<ObjectInteractionScript>().SetIsSelected(false);
        
    }


    private void OnControllerMove(GameObject controller)
    {

        RaycastHit hit;
        bool intersect = Physics.Raycast(controller.transform.position, controller.transform.forward,  out hit);


        if (intersect && hit.transform.GetComponent<Renderer>() != null)
        {
          //  Renderer rend = hit.transform.GetComponent<Renderer>();
           
            Vector2 pixelUV = hit.textureCoord;

            Texture2D texture = (Texture2D)GetComponent<Renderer>().material.mainTexture;



            int width = GetComponent<Renderer>().material.mainTexture.width;
            int height = GetComponent<Renderer>().material.mainTexture.height;


            if ((pixelUV.x <= 1.0f && pixelUV.x > 0.0f) && (pixelUV.y <= 1.0f && pixelUV.y > 0.0f))
            {
                pixelUV.x *= width;
                pixelUV.y *= height;

                UpdateColor(texture.GetPixel((int)pixelUV.x, (int)pixelUV.y));
               // SaveColor();
            }
            
        }

    }

    //void Update()
    //{
    //    if (GetComponent<Renderer>() != null)
    //    {
    //        if (objectToChangeColor.transform.FindChild("ScaleHandler") != null)
    //        {
    //            if (objectToChangeColor.transform.FindChild("ScaleHandler").FindChild("Top") != null)
    //            {
    //                if (objectToChangeColor.transform.FindChild("ScaleHandler").FindChild("Top").GetComponent<Renderer>() != null)
    //                {
    //                    float alpha = objectToChangeColor.transform.FindChild("ScaleHandler").FindChild("Top").GetComponent<Renderer>().material.color.a;
    //                    Color color = GetComponent<Renderer>().material.color;
    //                    color.a = alpha;
    //                    GetComponent<Renderer>().material.color = color;
    //                }
    //            }
                    
    //        }
    //    }

    //    //if (active)
    //    //{
    //    //    if (GetComponent<HighlightingSystem.Highlighter>() != null)
    //    //    {
    //    //        GetComponent<HighlightingSystem.Highlighter>().OnParams(Color.blue);
    //    //        GetComponent<HighlightingSystem.Highlighter>().On();
    //    //    }
    //    //}
        
    //}

    public void UpdateColor(Color newColor)
    {
        this.color = newColor;
        
        double h, s, l;
        HelperFunctionsScript.RGB2HSL(color, out h, out s, out l);
        l = brightness;
        color = HelperFunctionsScript.HSL2RGB(h, s, l);
        this.color.a = 1.0f;
        UpdateColorInObject();
    }

    public void UpdateColorBrightness(float brightness)
    {
        this.brightness = brightness;
        double h, s, l;
        HelperFunctionsScript.RGB2HSL(color, out h, out s, out l);
        l = brightness;
        color = HelperFunctionsScript.HSL2RGB(h, s, l);
        this.color.a = 1.0f;
        UpdateColorInObject();
    }

    private void UpdateColorInObject()
    {
      //  Debug.Log(brightness);
        if (objectToChangeColor.GetComponent<ShapeScript>() != null)
        {
            objectToChangeColor.GetComponent<ShapeScript>().UpdateColor(this.color);
        }
        else if (objectToChangeColor.GetComponent<TextScript>() != null)
        {
            objectToChangeColor.GetComponent<TextScript>().SetColor(this.color);
        }
        else if (objectToChangeColor.GetComponent<Shape3DScript>() != null)
        {
            objectToChangeColor.GetComponent<Shape3DScript>().SetColor(this.color);
        }
    }

    public void SaveColor()
    {
        SaveManager.SaveGameObject(objectToChangeColor);
    }



    #endregion
}
