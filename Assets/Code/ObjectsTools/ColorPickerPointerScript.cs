﻿#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class ColorPickerPointerScript : MonoBehaviour {

    #region Private Properties

    private bool active = false;
    private bool highlighted = false;
    public GameObject colorpicker;
    private Vector3 prevControllerPosition;


    #endregion


    void Start()
    {
        GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision;
        gameObject.GetComponent<Renderer>().sortingLayerName = "ToolsSortingLayer";
    }

    public void SetColorPicker(GameObject newColorpicekr)
    {
        colorpicker = newColorpicekr;
    }

    private void ControllerCollision(GameObject gameObj, bool isEnter)
    {
        if (colorpicker != null)
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
        ChangeHighlighting();


    }

    private void StartChangeColor(GameObject controller)
    {
        GetComponent<ObjectInteractionScript>().SetIsSelected(true);
        controller.GetComponent<ControlObjects>().ControllerMove += OnControllerMove;
        prevControllerPosition = controller.transform.position;
    }

    private void EndChangeColor(GameObject controller)
    {

        controller.GetComponent<ControlObjects>().ControllerMove -= OnControllerMove;
        GetComponent<ObjectInteractionScript>().SetIsSelected(false);
        if (colorpicker.GetComponent<ColorPickerScript>() != null)
        {
            colorpicker.GetComponent<ColorPickerScript>().SaveColor();
        }
    }

    private void OnControllerMove(GameObject controller)
    {
        Vector3 pos = transform.localPosition;

        Vector3 newControllerPosition = controller.transform.position;

        //Debug.Log((newControllerPosition - prevControllerPosition).x + " " + (newControllerPosition - prevControllerPosition).y + " " + (newControllerPosition - prevControllerPosition).z);
        Vector3 difference = (newControllerPosition - prevControllerPosition);

        Vector3 shiftVector = new Vector3( 0.0f, 0.0f, ((difference.z * controller.transform.right.z) + (difference.x * controller.transform.right.x)) / (transform.localScale.x * colorpicker.transform.localScale.x));
        
        transform.localPosition += shiftVector;

        RaycastHit hit;
        bool intersect = Physics.Raycast(transform.position, transform.forward, out hit);

        if (intersect && hit.transform.GetComponent<Renderer>() != null && hit.transform == colorpicker.transform)
        {
           // Renderer rend = transform.GetComponent<Renderer>();

            Vector2 pixelUV = hit.textureCoord2;

            Texture2D texture = (Texture2D)colorpicker.GetComponent<Renderer>().material.mainTexture;

            int width = colorpicker.GetComponent<Renderer>().material.mainTexture.width;
            int height = colorpicker.GetComponent<Renderer>().material.mainTexture.height;


            if ((pixelUV.x <= 1.0f && pixelUV.x > 0.0f) && (pixelUV.y <= 1.0f && pixelUV.y > 0.0f))
            {
                pixelUV.x *= width;
                pixelUV.y *= height;

                if (colorpicker.GetComponent<ColorPickerScript>() != null)
                {
                    colorpicker.GetComponent<ColorPickerScript>().UpdateColor(texture.GetPixel((int)pixelUV.x, (int)pixelUV.y));
                }
                else if (colorpicker.transform.parent.GetComponent<ToolbarMainColorPickerScript>() != null)
                {
                    colorpicker.transform.parent.GetComponent<ToolbarMainColorPickerScript>().SetColor(texture.GetPixel((int)pixelUV.x, (int)pixelUV.y));
                }

                    GetComponent<Renderer>().material.color = texture.GetPixel((int)pixelUV.x, (int)pixelUV.y);
            }

        }
        else {
            transform.localPosition = pos;
        }
        prevControllerPosition = newControllerPosition;

    }

    private void ChangeHighlighting()
    {
        if (active && !highlighted)
        {
            if (GetComponent<HighlightingSystem.Highlighter>() != null)
            {
                GetComponent<HighlightingSystem.Highlighter>().ConstantOn(ApplicationStaticData.toolsHighligthing);
            }
            highlighted = true;
        }
        else if (!active && highlighted)
        {
            if (GetComponent<HighlightingSystem.Highlighter>() != null)
            {
                GetComponent<HighlightingSystem.Highlighter>().ConstantOff();
            }
            highlighted = false;
        }
    }


}