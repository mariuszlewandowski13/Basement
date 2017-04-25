using UnityEngine;
using System.Collections;


[RequireComponent(typeof(ObjectInteractionScript))]
public class DuplicateObjectScript : MonoBehaviour {
    #region Private Properties

    private GameObject tempObject;
    private GameObject rotationParent;

    private Vector3 controllerFirstPosition;
    //private Vector3 controllerSecondPosition;

    private bool active =  false;

    public Transform prevParent;

    #endregion

    #region Public Properties

    public bool canDuplicate = true;


    #endregion

    void Start () {
        GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision;
        tempObject = null;
        
    }


    private void ControllerCollision(GameObject gameObj, bool isEnter)
    {
        if (isEnter && !active)
        {
            gameObj.GetComponent<ControlObjects>().GripUp += OnGripUp;
            gameObj.GetComponent<ControlObjects>().GripDown += OnGripDown;
            active = true;
        }
        else if (!isEnter && !GetComponent<ObjectInteractionScript>().GetIsSelected() && active)
        {
            gameObj.GetComponent<ControlObjects>().GripUp -= OnGripUp;
            gameObj.GetComponent<ControlObjects>().GripDown -= OnGripDown;
            active = false;
        }
    }

    private void OnGripDown(GameObject controller)
    {
        if (canDuplicate)
        {
            GetComponent<ObjectInteractionScript>().SetIsSelected(true);
            tempObject = GameObject.Find("Player").GetComponent<ObjectSpawnerScript>().SpawnDuplicateObject(gameObject, controller);
            if (GetComponent<PhotonView>() != null)
            {
                tempObject.GetComponent<PhotonView>().ownershipTransfer = OwnershipOption.Takeover;
                tempObject.GetComponent<PhotonView>().RequestOwnership();
            }
            

            tempObject.GetComponent<ObjectInteractionScript>().Reset();
            controller.GetComponent<ControlObjects>().ControllerMove += OnControllerMove;
            rotationParent = controller.GetComponent<ControlObjects>().rotationCenter;

            controllerFirstPosition = /*controllerSecondPosition =*/ controller.transform.position;

            rotationParent.transform.position = controllerFirstPosition;
            rotationParent.transform.rotation = controller.transform.rotation;

            prevParent = tempObject.transform.parent;

            if (tempObject.GetComponent<PhotonView>() == null || tempObject.GetComponent<PhotonView>().isMine)
            {
                tempObject.transform.parent = rotationParent.transform;
                if (tempObject.GetComponent<PhotonView>() != null)
                {
                    SaveParentByNetwork();
                    SetParentByNetwork(rotationParent.GetComponent<PhotonView>().viewID);
                }
                
            }

            if (tempObject.GetComponent<SelectingObjectsScript>() != null)
            {
                tempObject.GetComponent<SelectingObjectsScript>().ResetSelecting();
                controller.GetComponent<ControlObjects>().SetSelected(tempObject);
            }
        }


    }
    private void OnGripUp(GameObject controller)
    {
        if (tempObject != null)
        {
            controller.GetComponent<ControlObjects>().ControllerMove -= OnControllerMove;

            tempObject.transform.parent = prevParent;
            LoadParentByNetwork();

            GetComponent<ObjectInteractionScript>().SetIsSelected(false);
            SaveManager.SaveGameObject(tempObject);
            //ControllerCollision(actualController, GetComponent<ObjectInteractionScript>().GetCollision());
        }
        
    }

    private void OnControllerMove(GameObject controller)
    {
            //controllerSecondPosition = controllerFirstPosition;
            controllerFirstPosition = controller.transform.position;

            rotationParent.transform.position = controllerFirstPosition;
            rotationParent.transform.rotation = controller.transform.rotation;

        if (tempObject.transform.parent != rotationParent.transform && tempObject.GetComponent<PhotonView>().isMine)
        {
            tempObject.transform.parent = rotationParent.transform;
            SaveParentByNetwork();
            SetParentByNetwork(rotationParent.GetComponent<PhotonView>().viewID);
        }

    }

    private void SetParentByNetwork(int number)
    {
        if (tempObject.GetComponent<PhotonView>() != null && PhotonNetwork.inRoom)
        {
            tempObject.GetComponent<PhotonView>().RPC("SetParentFromNetwork", PhotonTargets.Others, number);
        }
    }

    private void SaveParentByNetwork()
    {
        if (tempObject.GetComponent<PhotonView>() != null && PhotonNetwork.inRoom)
        {
            tempObject.GetComponent<PhotonView>().RPC("SaveParentFromNetwork", PhotonTargets.Others);
        }

    }

    private void LoadParentByNetwork()
    {
        if (tempObject.GetComponent<PhotonView>() != null && PhotonNetwork.inRoom)
        {
            tempObject.GetComponent<PhotonView>().RPC("LoadParentFromNetwork", PhotonTargets.Others);
        }

    }

}
