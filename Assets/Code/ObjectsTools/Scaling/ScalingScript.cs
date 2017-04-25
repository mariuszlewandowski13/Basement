using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingScript : MonoBehaviour {

    #region Private Properties

    private Vector3 controllerPrevPosition;
    private GameObject mainObject;
    private GameObject tempObject;
    private GameObject parent;

    public float scaleMultiplier = -0.075f;

    private Vector3 directionVector;

    private Vector3 topMainScale;

    private bool active = false;
    private bool selected = false;

    private float horizontalMultiplier = 1.0f;
    private float verticalMultiplier = 1.0f;
    private float forwarBackwardMultiplier = 1.0f;
    private GameObject actualController;

    #endregion

    #region Public Properties

    public GameObject referencePoint;
    public bool horizontal = false;
    public bool vertical = false;
    public bool forwardBack = false;

    #endregion

    #region Methods

    void Start()
    {
        parent = gameObject.transform.parent.gameObject;
        mainObject = parent.GetComponent<ScaleHandlerScript>().GetParentObject();

        SetColor();

        GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision;

        if (!horizontal) horizontalMultiplier = 0.0f;
        if (!vertical) verticalMultiplier = 0.0f;
        if (!forwardBack) forwarBackwardMultiplier = 0.0f;

        gameObject.GetComponent<Renderer>().sortingLayerName = "ToolsSortingLayer";
    }

    private void ControllerCollision(GameObject gameObj, bool isEnter)
    {
        if (isEnter && !active && gameObj.GetComponent<ControlObjects>().secondController.GetComponent<ControlObjects>().pickup != mainObject)
        {
            gameObj.GetComponent<ControlObjects>().TriggerDown += OnTriggerDown;

            active = true;
            SetColor();
            actualController = gameObj;
        }
        else if (!isEnter && !GetComponent<ObjectInteractionScript>().GetIsSelected() && active)
        {
            gameObj.GetComponent<ControlObjects>().TriggerDown -= OnTriggerDown;

            active = false;
            SetColor();

        }
    }

    private void OnTriggerDown(GameObject controller)
    {
        if (controller.GetComponent<ControlObjects>().secondController.GetComponent<ControlObjects>().pickup != mainObject)
        {
            controller.GetComponent<ControlObjects>().TriggerUp += OnTriggerUp;
            GetComponent<ObjectInteractionScript>().SetIsSelected(true);
            tempObject = parent.GetComponent<ScaleHandlerScript>().AddNewResizingObject(gameObject);
            
            selected = true;
            SetColor();
            controllerPrevPosition = controller.transform.position;
            
            gameObject.transform.parent = tempObject.transform;
            

            topMainScale = referencePoint.transform.localScale;
            
            CalculateDirection();
            controller.GetComponent<ControlObjects>().ControllerMove += OnControllerMove;
        }
    }

    private void OnControllerMove(GameObject controller)
    {
        if (tempObject != null)
        {
            lock (parent.GetComponent<ScaleHandlerScript>().tempObjectLock)
            {
                Vector3 actualControllerPosition = controller.transform.position;

                Vector3 shiftVector = actualControllerPosition - controllerPrevPosition;

                Transform actTempObjParent = tempObject.transform.parent;

                tempObject.transform.parent = referencePoint.transform;
                
                ResizeTempObject(shiftVector);
                
                controllerPrevPosition = actualControllerPosition;
                tempObject.transform.parent = actTempObjParent;
            }
        }

    }

    private void ResizeTempObject(Vector3 shiftVector)
    {
        referencePoint.transform.localScale += new Vector3((shiftVector.y * scaleMultiplier * directionVector.y + shiftVector.x * scaleMultiplier * directionVector.x + shiftVector.z * scaleMultiplier * directionVector.z) * horizontalMultiplier,
            (shiftVector.y * scaleMultiplier * directionVector.y + shiftVector.x * scaleMultiplier * directionVector.x + shiftVector.z * scaleMultiplier * directionVector.z) * verticalMultiplier,
            (shiftVector.y * scaleMultiplier * directionVector.y + shiftVector.x * scaleMultiplier * directionVector.x + shiftVector.z * scaleMultiplier * directionVector.z) * forwarBackwardMultiplier);
    }

    private void SetColor()
    {
        if (active || selected)
        {
            transform.FindChild("active").gameObject.SetActive(true);
            transform.FindChild("notActive").gameObject.SetActive(false);
        }
        else if (!active && !selected)
        {
            transform.FindChild("active").gameObject.SetActive(false);
            transform.FindChild("notActive").gameObject.SetActive(true);
        }
    }

    private void OnTriggerUp(GameObject controller)
    {
        controller.GetComponent<ControlObjects>().TriggerUp -= OnTriggerUp;
        controller.GetComponent<ControlObjects>().ControllerMove -= OnControllerMove;
        

        parent.GetComponent<ScaleHandlerScript>().RemoveResizingObject(gameObject);

        referencePoint.transform.localScale = topMainScale;
        
        selected = false;
        SetColor();

        GetComponent<ObjectInteractionScript>().SetIsSelected(false);
        ControllerCollision(actualController, GetComponent<ObjectInteractionScript>().GetCollision());
    }

    private void CalculateDirection()
    {
        directionVector = (referencePoint.transform.position - gameObject.transform.position).normalized;
    }

    #endregion
}
