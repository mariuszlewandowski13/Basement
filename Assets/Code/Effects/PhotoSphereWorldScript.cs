#region Usings

using UnityEngine;

#endregion

public class PhotoSphereWorldScript : MonoBehaviour {

    #region Private Properties

    public bool toDestroy = false;
    private GameObject rotationParent;

    private Vector3 controllerFirstPosition;
    private Vector3 controllerSecondPosition;

    private float treshold = 0.15f;

    private bool wearing = false;

    private Vector3 basePosition;
    private Vector3 baseScale;

    public bool active = false;

    private GameObject actualController;

    private Transform prevParent;

    private float destroyTreshold = 0.05f;

    #endregion


    #region Public Properties

    public bool canMove;

    #endregion

    #region Methods
    void Start()
    {
        GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision;
        if (GetComponent<PhotonView>() != null)
        {
            GetComponent<PhotonView>().ownershipTransfer = OwnershipOption.Takeover;
        }

        canMove = true;

    }

    private void ControllerCollision(GameObject gameObj, bool isEnter)
    {
        if (!wearing)
        {
            if (isEnter && !active)
            {
                if ((gameObj.GetComponent<ControlObjects>().secondController.GetComponent<ControlObjects>().pickup == null ||
            (gameObject.transform.FindChild("ScaleHandler") == null && gameObj.GetComponent<ControlObjects>().secondController.GetComponent<ControlObjects>().pickup.transform.FindChild("ScaleHandler") == null) ||
            (gameObj.GetComponent<ControlObjects>().secondController.GetComponent<ControlObjects>().pickup.transform.FindChild("ScaleHandler") == null && gameObject.transform.FindChild("ScaleHandler") != null && !gameObj.GetComponent<ControlObjects>().secondController.GetComponent<ControlObjects>().pickup.transform.IsChildOf(gameObject.transform.FindChild("ScaleHandler"))) ||
            (gameObj.GetComponent<ControlObjects>().secondController.GetComponent<ControlObjects>().pickup.transform.FindChild("ScaleHandler") != null && gameObject.transform.FindChild("ScaleHandler") == null && !transform.IsChildOf(gameObj.GetComponent<ControlObjects>().secondController.GetComponent<ControlObjects>().pickup.transform.FindChild("ScaleHandler"))) ||
            (gameObject.transform.FindChild("ScaleHandler") != null && gameObj.GetComponent<ControlObjects>().secondController.GetComponent<ControlObjects>().pickup.transform.FindChild("ScaleHandler") != null)
            ) && canMove)
                { 
                    gameObj.GetComponent<ControlObjects>().TriggerDown += OnTriggerDown;
                    active = true;
                    actualController = gameObj;
                }

            }
            else if (!isEnter && !GetComponent<ObjectInteractionScript>().GetIsSelected() && active)
            {
                gameObj.GetComponent<ControlObjects>().TriggerDown -= OnTriggerDown;
                active = false;
            }
        }

    }



    private void OnTriggerUp(GameObject controller)
    {
        controller.GetComponent<ControlObjects>().ControllerMove -= OnControllerMove;
        controller.GetComponent<ControlObjects>().TriggerUp -= OnTriggerUp;
        if (!wearing)
        {
            gameObject.transform.parent = null;
            CheckAndSetToDestroy();
            gameObject.transform.parent = prevParent;
            LoadParentByNetwork();
            GetComponent<ObjectInteractionScript>().SetIsSelected(false);

        }
        if (!toDestroy)
        {
            SaveManager.SaveGameObject(gameObject);
        }
    }

    private void OnTriggerDown(GameObject controller)
    {

        if ((controller.GetComponent<ControlObjects>().secondController.GetComponent<ControlObjects>().pickup == null ||
           (gameObject.transform.FindChild("ScaleHandler") == null && controller.GetComponent<ControlObjects>().secondController.GetComponent<ControlObjects>().pickup.transform.FindChild("ScaleHandler") == null) ||
           (controller.GetComponent<ControlObjects>().secondController.GetComponent<ControlObjects>().pickup.transform.FindChild("ScaleHandler") == null && gameObject.transform.FindChild("ScaleHandler") != null && !controller.GetComponent<ControlObjects>().secondController.GetComponent<ControlObjects>().pickup.transform.IsChildOf(gameObject.transform.FindChild("ScaleHandler"))) ||
           (controller.GetComponent<ControlObjects>().secondController.GetComponent<ControlObjects>().pickup.transform.FindChild("ScaleHandler") != null && gameObject.transform.FindChild("ScaleHandler") == null && !transform.IsChildOf(controller.GetComponent<ControlObjects>().secondController.GetComponent<ControlObjects>().pickup.transform.FindChild("ScaleHandler"))) ||
           (gameObject.transform.FindChild("ScaleHandler") != null && controller.GetComponent<ControlObjects>().secondController.GetComponent<ControlObjects>().pickup.transform.FindChild("ScaleHandler") != null)
           ) && canMove)
        {
            if (GetComponent<PhotonView>() != null)
            {
                GetComponent<PhotonView>().RequestOwnership();
            }

            GetComponent<ObjectInteractionScript>().SetIsSelected(true);
            controller.GetComponent<ControlObjects>().TriggerUp += OnTriggerUp;
            controller.GetComponent<ControlObjects>().ControllerMove += OnControllerMove;
            if (!wearing)
            {
                rotationParent = controller.GetComponent<ControlObjects>().rotationCenter;

                basePosition = gameObject.transform.position;
                baseScale = gameObject.transform.localScale;

                controllerFirstPosition = controllerSecondPosition = controller.transform.position;

                rotationParent.transform.position = controllerFirstPosition;
                rotationParent.transform.rotation = controller.transform.rotation;
                prevParent = gameObject.transform.parent;


                if (GetComponent<PhotonView>().isMine)
                {
                    //Debug.Log("Controller pos: " + controller.transform.position.x + " " + controller.transform.position.y + " " + controller.transform.position.z );
                    //Debug.Log("Controller pos: " + rotationParent.transform.position.x + " " + rotationParent.transform.position.y + " " + rotationParent.transform.position.z);
                    //Debug.Log("isMine on triggerdown");
                    gameObject.transform.parent = rotationParent.transform;
                    SaveParentByNetwork();
                    SetParentByNetwork(rotationParent.GetComponent<PhotonView>().viewID);
                }

                if (GetComponent<Rigidbody>() != null) Destroy(gameObject.GetComponent<Rigidbody>());
                toDestroy = false;
            }
        }
        
    }

    private void OnControllerMove(GameObject controller)
    {
        if (!wearing)
        {
            controllerSecondPosition = controllerFirstPosition;
            controllerFirstPosition = controller.transform.position;

            rotationParent.transform.position = controllerFirstPosition;
            rotationParent.transform.rotation = controller.transform.rotation;
            if (gameObject.transform.parent != rotationParent.transform && GetComponent<PhotonView>().isMine)
            {
                gameObject.transform.parent = rotationParent.transform;
                SaveParentByNetwork();
                SetParentByNetwork(rotationParent.GetComponent<PhotonView>().viewID);
            }


            if (Vector3.Distance(gameObject.transform.position, Camera.main.transform.position) < treshold)
            {

                GetComponent<ObjectInteractionScript>().SetIsSelected(false);
                ControllerCollision(actualController, false);
                WearSphere(controller);
                
            }
        }
        
    }

    private void WearSphere(GameObject controller)
    {
        if (GetComponent<DuplicateObjectScript>() != null)
        {
            GetComponent<DuplicateObjectScript>().canDuplicate = false;
        }
            
        gameObject.layer = LayerMask.NameToLayer("NewWorld");
        wearing = true;

        gameObject.transform.parent = null;
        LoadParentByNetwork();
        gameObject.transform.position = Camera.main.transform.position;
        gameObject.transform.localScale = baseScale*200.0f;

        GameObject.Find("Camera (eye)").GetComponent<Camera>().cullingMask = (1 << LayerMask.NameToLayer("NewWorld")) | (1 << LayerMask.NameToLayer("Controllers"));
        ControlObjects.TriggerDownGlobal += UnWearSphere;
        GameObject.Find("Camera (eye)").GetComponent<TeleportVive>().enabled = false;

    }

    private void UnWearSphere(GameObject controller)
    {
        gameObject.layer = LayerMask.NameToLayer("PhysicalObjects");
        wearing = false;
        
        gameObject.transform.position = basePosition;
        gameObject.transform.localScale = baseScale;
        gameObject.transform.parent = prevParent;
        LoadParentByNetwork();

        GameObject.Find("Camera (eye)").GetComponent<Camera>().cullingMask = -1 ;
        GameObject.Find("Camera (eye)").GetComponent<Camera>().cullingMask &= ~(1 << LayerMask.NameToLayer("MyAvatar"));


        ControlObjects.TriggerDownGlobal -= UnWearSphere;
        GameObject.Find("Camera (eye)").GetComponent<TeleportVive>().enabled = true;
        if (GetComponent<DuplicateObjectScript>() != null)
        {
            GetComponent<DuplicateObjectScript>().canDuplicate = true;

        }
    }

    private void CheckAndSetToDestroy()
    {
        float distance = Vector3.Distance(controllerFirstPosition, controllerSecondPosition);
        if (distance > destroyTreshold)
        {
            toDestroy = true;
            //GameObject.Find("Player").GetComponent<ObjectsDestroyerScript>().AddObjectToDestroy(gameObject);

            Rigidbody rigidbo = gameObject.AddComponent<Rigidbody>();

            rigidbo.AddForce((controllerFirstPosition - controllerSecondPosition) * 8000.0f, ForceMode.Force);
            rigidbo.mass = 0.01f;
            rigidbo.useGravity = true;
            rigidbo.detectCollisions = false;
            rigidbo.interpolation = RigidbodyInterpolation.Interpolate;
            Transform scaleHandler = transform.FindChild("ScaleHandler");
            ControllerCollision(actualController, false);
            if (scaleHandler != null)
            {
                DestroyObjectAndChildrenColliders(scaleHandler);
            }
            ControllerCollision(actualController, false);
            Destroy(gameObject, 5.0f);
        }
    }

    private void OnDestroy()
    {
        GetComponent<ObjectInteractionScript>().ControllerCollision -= ControllerCollision;

    }

    private void DestroyObjectAndChildrenColliders(Transform gameObject)
    {
        foreach (Transform child in gameObject)
        {
            if (child.GetComponent<Collider>() != null) child.GetComponent<Collider>().enabled = false;
        }
    }

    private void SetParentByNetwork(int number)
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("SetParentFromNetwork", PhotonTargets.Others, number);
        }

    }

    private void SaveParentByNetwork()
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("SaveParentFromNetwork", PhotonTargets.Others);
        }

    }

    private void LoadParentByNetwork()
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("LoadParentFromNetwork", PhotonTargets.Others);
        }

    }

    public void DeleteInNetwork()
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("DeletePhotoSphereFromNetwork", PhotonTargets.Others);
        }
    }

    [PunRPC]
    private void DeletePhotoSphereFromNetwork()
    {
        Destroy(gameObject);
    }



    #endregion


}
