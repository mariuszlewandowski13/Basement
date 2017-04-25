#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class PhotoSphereMiniatureWorldScript : MonoBehaviour {

    #region Private Properties

    private Vector3 basePosition;
    private Vector3 baseScale;
    private Quaternion baseRotation;
    public Transform parent;
    private GameObject rotationParent;

    private bool toDestroy = false;
    private bool noParentDestroy = false;

    private Vector3 controllerFirstPosition;
    private Vector3 controllerSecondPosition;

    private bool active = false;

    private bool wearing = false;

    private GameObject actualController;

    private float tresholdToWorld = 0.15f;

    #endregion

    #region Public Properties

    public float treshold = 0.05f;

    #endregion

    #region Methods

    void Start()
    {
        GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision;
        parent = gameObject.transform.parent;
        baseScale = gameObject.transform.localScale;
        // SetScaleBoxesFadeIn(false);    
    }

    void Update()
    {

        if (parent == null) noParentDestroy = true;

        if (toDestroy || noParentDestroy)
        {
            CheckAndDestroy();
        }
    }

    private void ControllerCollision(GameObject gameObj, bool isEnter)
    {
        if (!wearing)
        {
            if (isEnter && !active)
            {
                gameObj.GetComponent<ControlObjects>().TriggerDown += StartSpawn;
                active = true;
                actualController = gameObj;
            }
            else if (!isEnter && !GetComponent<ObjectInteractionScript>().GetIsSelected() && active)
            {
                gameObj.GetComponent<ControlObjects>().TriggerDown -= StartSpawn;
                gameObj.GetComponent<ControlObjects>().ControllerMove -= Move;
                active = false;
            }
        }

    }


    public void StartSpawn(GameObject controller)
    {
        GetComponent<ObjectInteractionScript>().SetIsSelected(true);
        controller.GetComponent<ControlObjects>().ControllerMove += Move;

        if (!wearing)
        {
            rotationParent = controller.GetComponent<ControlObjects>().rotationCenter;

            controllerFirstPosition = controllerSecondPosition = controller.transform.position;

            basePosition = gameObject.transform.localPosition;
            baseRotation = gameObject.transform.localRotation;


            rotationParent.transform.position = controller.transform.position;
            rotationParent.transform.rotation = controller.transform.rotation;

            gameObject.transform.parent = rotationParent.transform;

            if (GetComponent<Rigidbody>() != null) Destroy(gameObject.GetComponent<Rigidbody>());
            toDestroy = false;

            controller.GetComponent<ControlObjects>().TriggerUp += SpawnObject;
            SetScaleBoxesFadeIn(true);
        }
        
    }

    public void Move(GameObject controller)
    {
        if (!wearing)
        {
            controllerSecondPosition = controllerFirstPosition;
            controllerFirstPosition = controller.transform.position;

            rotationParent.transform.position = controllerFirstPosition;
            rotationParent.transform.rotation = controller.transform.rotation;

            if (Vector3.Distance(gameObject.transform.position, Camera.main.transform.position) < tresholdToWorld)
            {
                controller.GetComponent<ControlObjects>().TriggerUp -= SpawnObject;
                controller.GetComponent<ControlObjects>().ControllerMove -= Move;
                GetComponent<ObjectInteractionScript>().SetIsSelected(false);
                ControllerCollision(actualController, false);
                WearSphere(controller);
                

            }
        }
    }

    private void WearSphere(GameObject controller)
    {
        SetScaleBoxesFadeIn(false);
        wearing = true;
        if (GetComponent<DuplicateObjectScript>() != null)
        {
            GetComponent<DuplicateObjectScript>().canDuplicate = false;
        }

        gameObject.layer = LayerMask.NameToLayer("NewWorld");
        

        gameObject.transform.parent = null;
        gameObject.transform.position = Camera.main.transform.position;
        gameObject.transform.localScale = new Vector3(200.0f, 200.0f, 200.0f);

        GameObject.Find("Camera (eye)").GetComponent<Camera>().cullingMask = (1 << LayerMask.NameToLayer("NewWorld")) | (1 << LayerMask.NameToLayer("Controllers"));
        ControlObjects.TriggerDownGlobal += UnWearSphere;
        GameObject.Find("Camera (eye)").GetComponent<TeleportVive>().enabled = false;

    }

    private void UnWearSphere(GameObject controller)
    {
        gameObject.layer = LayerMask.NameToLayer("PhysicalObjects");
        wearing = false;

        returnToMainPosition();

        GameObject.Find("Camera (eye)").GetComponent<Camera>().cullingMask = -1;
        GameObject.Find("Camera (eye)").GetComponent<Camera>().cullingMask &= ~(1 << LayerMask.NameToLayer("MyAvatar"));
        ControlObjects.TriggerDownGlobal -= UnWearSphere;
        GameObject.Find("Camera (eye)").GetComponent<TeleportVive>().enabled = true;
        if (GetComponent<DuplicateObjectScript>() != null)
        {
            GetComponent<DuplicateObjectScript>().canDuplicate = true;

        }
    }

    public void SpawnObject(GameObject controller)
    {
        controller.GetComponent<ControlObjects>().ControllerMove -= Move;

        if (!wearing)
        {
            GetComponent<ObjectInteractionScript>().SetIsSelected(false);
            SceneObject sceneObjectToSpawn = gameObject.GetComponent<SceneObjectScript>().miniaturedSceneObject;
            SetScaleBoxesFadeIn(false);

            ChechAndSetToDestroy();

            if (!toDestroy)
            {
                GameObject.Find("Player").GetComponent<ObjectSpawnerScript>().SpawnObject(sceneObjectToSpawn, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.localScale, controller, true);
                if (!noParentDestroy) returnToMainPosition();
            }
            controller.GetComponent<ControlObjects>().TriggerUp -= SpawnObject;

            ControllerCollision(actualController, false);
        }
 
    }

    private void ChechAndSetToDestroy()
    {
        if (parent == null)
        {
            noParentDestroy = true;
        }
        float distance = Vector3.Distance(controllerFirstPosition, controllerSecondPosition);
        if (distance > treshold)
        {
            toDestroy = true;
            gameObject.AddComponent<Rigidbody>().
            GetComponent<Rigidbody>().AddForce((controllerFirstPosition - controllerSecondPosition) * 6000.0f, ForceMode.Force);
            GetComponent<Rigidbody>().mass = 0.01f;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().detectCollisions = false;
            
            //GetComponent<Collider>().enabled = false;
        }
    }

    private void CheckAndDestroy()
    {
        if (toDestroy && Vector3.Distance(gameObject.transform.position, GameObject.Find("Player").transform.position) > 10.0f)
        {
            if (!noParentDestroy)
            {
                Destroy(gameObject.GetComponent<Rigidbody>());
                GetComponent<Collider>().enabled = true;
                toDestroy = false;
                returnToMainPosition();
            }
            else {
                
                Destroy(gameObject);
            }
        }
        else if (!toDestroy && noParentDestroy)
        {

            //actualController.GetComponent<ControlObjects>().TriggerDown -= StartSpawn;
            Destroy(gameObject);
        }

    }

    private void returnToMainPosition()
    {
        gameObject.transform.parent = parent;

        gameObject.transform.localPosition = basePosition;
        gameObject.transform.localRotation = baseRotation;
        gameObject.transform.localScale = baseScale;
    }

    private void SetScaleBoxesFadeIn(bool active)
    {
        Transform scaleHandler = gameObject.transform.FindChild("ScaleHandler");
        if (scaleHandler != null)
        {
            if (active)
            {
                scaleHandler.GetComponent<ObjectsFadeScript>().IncreaseFadeIn();
            }
            else {
                scaleHandler.GetComponent<ObjectsFadeScript>().DecreaseFadeIn();
            }
        }
    }

    #endregion
}
