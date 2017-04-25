#region Usings
using UnityEngine;
#endregion


public class ImageMoveScript : MonoBehaviour
{
    #region Private Properties

    private bool toDestroy = false;
    private GameObject rotationParent;

    private Transform prevParent;

    private Vector3 controllerFirstPosition;
    private Vector3 controllerSecondPosition;

    private GameObject actualController;

    public bool active = false;

    #endregion
    #region Public Properties

    public float treshold = 0.05f;
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

    private void OnControllerMove(GameObject controller)
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
           
            controller.GetComponent<ControlObjects>().TriggerUp += OnTriggerUp;
            if (GetComponent<PhotonView>() != null)
            {
                GetComponent<PhotonView>().RequestOwnership();
            }
            
            GetComponent<ObjectInteractionScript>().SetIsSelected(true);

            controller.GetComponent<ControlObjects>().ControllerMove += OnControllerMove;
            rotationParent = controller.GetComponent<ControlObjects>().rotationCenter;
            controllerFirstPosition = controllerSecondPosition = controller.transform.position;

            rotationParent.transform.position = controllerFirstPosition;
            rotationParent.transform.rotation = controller.transform.rotation;
            prevParent = gameObject.transform.parent;

            if (GetComponent<PhotonView>() == null || GetComponent<PhotonView>().isMine)
            {
                gameObject.transform.parent = rotationParent.transform;
                SaveParentByNetwork();
                SetParentByNetwork(rotationParent.GetComponent<PhotonView>().viewID);
            }

            if (GetComponent<Rigidbody>() != null) Destroy(gameObject.GetComponent<Rigidbody>());
            toDestroy = false;
        }
    }
    private void OnTriggerUp(GameObject controller)
    {
        controller.GetComponent<ControlObjects>().TriggerUp -= OnTriggerUp;
        controller.GetComponent<ControlObjects>().ControllerMove -= OnControllerMove;
        GetComponent<ObjectInteractionScript>().SetIsSelected(false);
        gameObject.transform.parent = prevParent;
        LoadParentByNetwork();
        CheckAndSetToDestroy();

        if (!toDestroy && GetComponent<PhotonView>() != null)
        {
            SaveManager.SaveGameObject(gameObject);
        }
    }

    private void CheckAndSetToDestroy()
    {
        float distance = Vector3.Distance(controllerFirstPosition, controllerSecondPosition);
        if (distance > treshold)
        {
            toDestroy = true;
            Rigidbody rigidbod = gameObject.AddComponent<Rigidbody>();
            rigidbod.AddForce((controllerFirstPosition - controllerSecondPosition) * 8000.0f, ForceMode.Force);
            rigidbod.mass = 0.01f;
            rigidbod.useGravity = true;
            rigidbod.detectCollisions = false;
            rigidbod.interpolation = RigidbodyInterpolation.Interpolate;
            DestroyImmediate(GetComponent<BoxCollider>());
            Transform scaleHandler = transform.FindChild("ScaleHandler");

            ControllerCollision(actualController, false);
            if (scaleHandler != null)
            {
                DestroyObjectAndChildrenColliders(scaleHandler);
            }
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

    public void SetParentByNetwork(int number)
    {
        if (GetComponent<PhotonView>() != null && PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("SetParentFromNetwork", PhotonTargets.Others, number);
        }
        
    }

    public void SaveParentByNetwork()
    {
        if (GetComponent<PhotonView>() != null && PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("SaveParentFromNetwork", PhotonTargets.Others);
        }
        
    }

    public void LoadParentByNetwork()
    {
        if (GetComponent<PhotonView>() != null && PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("LoadParentFromNetwork", PhotonTargets.Others);
        }
        
    }

    public void DeleteInNetwork()
    {
        if (GetComponent<PhotonView>() != null && PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("DeleteFromNetwork", PhotonTargets.Others);
        }
    }

    [PunRPC]
    private void DeleteFromNetwork()
    {
        Destroy(gameObject);
    }

    #endregion
}
