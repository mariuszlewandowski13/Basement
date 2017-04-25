#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class PositionBarScript : MonoBehaviour {

    #region Private Properties

    private bool active = false;

    private GameObject toolbar;

    private float minYPosition = 0.2f;

    private float controllerYPreviousPosition;

    private Vector3 controllerPrevPosition;

    //private float controllerXPreviousPosition;
    //private float controllerZPreviousPosition;
    #endregion

    #region Public Properties

    public GameObject rotationCenter;


    #endregion


    #region Methods

    void Start()
    {
        GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision;
        toolbar = transform.parent.parent.gameObject;
    }

    private void ControllerCollision(GameObject gameObj, bool isEnter)
    {
        if (isEnter && !active)
        {
            gameObj.GetComponent<ControlObjects>().TriggerDown += OnTriggerDown;
            gameObj.GetComponent<ControlObjects>().TriggerUp += OnTriggerUp;
            active = true;
        }
        else if (!isEnter && !GetComponent<ObjectInteractionScript>().GetIsSelected() && active)
        {
            gameObj.GetComponent<ControlObjects>().TriggerDown -= OnTriggerDown;
            gameObj.GetComponent<ControlObjects>().TriggerUp -= OnTriggerUp;
            active = false;
        }
    }


    private void OnControllerMove(GameObject controller)
    {
        float controllerYPosition = controller.transform.position.y;


        float shiftY = controllerYPreviousPosition - controllerYPosition;


        float toolbarYPosition = toolbar.GetComponent<ToolbarMovementScript>().positionYOffset;

        if (toolbarYPosition - shiftY >= minYPosition) toolbar.GetComponent<ToolbarMovementScript>().positionYOffset -= shiftY;

        controllerYPreviousPosition = controllerYPosition;


        //rotation
        Vector3 controllerPos = controller.transform.position;

        Vector3 from = controllerPrevPosition - toolbar.transform.position;
        Vector3 to = controllerPos - toolbar.transform.position;

        

        from.y = 0.0f;
        to.y = 0.0f;


        





        float angle = Vector3.Angle(from, to) ;

       Vector3 cross = Vector3.Cross(from, to);
        if (cross.y < 0) angle = -angle;



        Vector3 toolbarRot = toolbar.transform.rotation.eulerAngles;
        toolbarRot.y += angle;
        toolbar.transform.rotation = Quaternion.Euler(toolbarRot);


        controllerPrevPosition = controllerPos;
        //UpdateRotationCenterPosition();

        //toolbar.transform.parent = rotationCenter.transform;




        //toolbar.transform.parent = prevParent.transform;

        //toolbar.transform.parent = prevParent.transform;
        //rotationCenter.transform.position = Camera.main.transform.position;
        //toolbar.transform.parent = rotationCenter.transform;



        //Vector3 newRot = Quaternion.LookRotation(controller.transform.forward, Vector3.up).eulerAngles;
        //newRot.x = 0.0f;


        //rotationCenter.transform.rotation = Quaternion.Euler(newRot.x, newRot.y, newRot.z);

    }



    private void OnTriggerDown(GameObject controller)
    {
        GetComponent<ObjectInteractionScript>().SetIsSelected(true);
        controller.GetComponent<ControlObjects>().ControllerMove += OnControllerMove;

        controllerYPreviousPosition = controller.transform.position.y;
        controllerPrevPosition = controller.transform.position;

        //UpdateRotationCenterPosition();

    }



    private void OnTriggerUp(GameObject controller)
    {
        GetComponent<ObjectInteractionScript>().SetIsSelected(false);
        controller.GetComponent<ControlObjects>().ControllerMove -= OnControllerMove;
    }

    //private void UpdateRotationCenterPosition()
    //{
    //    Vector3 position = cameraObject.transform.position;

    //    position.y = toolbar.transform.position.y;

    //    rotationCenter.transform.position = position;
    //}

    #endregion
}
