#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class TextColorScript : MonoBehaviour
{

    #region Private Properties

    private Vector3 controllerFirstPosition;
    private Vector3 controllerSecondPosition;
    private GameObject objectToChangeColor;

    private bool active = false;

    #endregion

    #region Public Properties

    public Color color;

    #endregion


    #region Methods
    void Start()
    {
        GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision;
        objectToChangeColor = transform.parent.GetComponent<SaveTransformScript>().parentObject.transform.parent.gameObject;
        gameObject.GetComponent<Renderer>().sortingLayerName = "ToolsSortingLayer";
    }

    private void ControllerCollision(GameObject gameObj, bool isEnter)
    {
        if (isEnter && !active)
        {
            gameObj.GetComponent<ControlObjects>().TriggerUp += EndChangeColor;
            gameObj.GetComponent<ControlObjects>().TriggerDown += StartChangeColor;
            active = true;
            //objectToChangeColor.transform.FindChild("ScaleHandlerPlane").FindChild("ScaleHandler").GetComponent<ObjectsFadeScript>().IncreaseFadeIn();
        }
        else if (!isEnter && !GetComponent<ObjectInteractionScript>().GetIsSelected() && active)
        {
            gameObj.GetComponent<ControlObjects>().TriggerUp -= EndChangeColor;
            gameObj.GetComponent<ControlObjects>().TriggerDown -= StartChangeColor;
            active = false;
            //objectToChangeColor.transform.FindChild("ScaleHandlerPlane").FindChild("ScaleHandler").GetComponent<ObjectsFadeScript>().DecreaseFadeIn();
        }
    }


    private void StartChangeColor(GameObject controller)
    {
        GetComponent<ObjectInteractionScript>().SetIsSelected(true);
        controller.GetComponent<ControlObjects>().ControllerMove += OnControllerMove;
    }

    private void EndChangeColor(GameObject controller)
    {
        
        controller.GetComponent<ControlObjects>().ControllerMove -= OnControllerMove;
        GetComponent<ObjectInteractionScript>().SetIsSelected(false);
        //ControllerCollision(actualController, GetComponent<ObjectInteractionScript>().GetCollision());
    }


    private void OnControllerMove(GameObject controller)
    {

        RaycastHit hit;
        bool intersect = Physics.Raycast(controller.transform.position, controller.transform.forward, out hit);


        if (intersect && hit.transform.GetComponent<Renderer>() != null)
        {
            //Renderer rend = hit.transform.GetComponent<Renderer>();
            //Texture2D tex = rend.material.mainTexture as Texture2D;
            Vector2 pixelUV = hit.textureCoord;

            Texture2D texture = (Texture2D)GetComponent<Renderer>().material.mainTexture;

            int width = GetComponent<Renderer>().material.mainTexture.width;
            int height = GetComponent<Renderer>().material.mainTexture.height;


            pixelUV.x *= width;
            pixelUV.y *= height;

            this.color = texture.GetPixel((int)pixelUV.x, (int)pixelUV.y);
            objectToChangeColor.GetComponent<TextScript>().SetColor(this.color);
        }

    }

    void Update()
    {
        if (GetComponent<Renderer>() != null)
        {
            if (objectToChangeColor.transform.FindChild("ScaleHandlerPlane").FindChild("ScaleHandler") != null)
            {
                if (objectToChangeColor.transform.FindChild("ScaleHandlerPlane").FindChild("ScaleHandler").FindChild("Top") != null)
                {
                    if (objectToChangeColor.transform.FindChild("ScaleHandlerPlane").FindChild("ScaleHandler").FindChild("Top").GetComponent<Renderer>() != null)
                    {
                        float alpha = objectToChangeColor.transform.FindChild("ScaleHandlerPlane").FindChild("ScaleHandler").FindChild("Top").GetComponent<Renderer>().material.color.a;
                        Color color = GetComponent<Renderer>().material.color;
                        color.a = alpha;
                        GetComponent<Renderer>().material.color = color;
                    }
                }

            }


        }

    }



    #endregion
}
