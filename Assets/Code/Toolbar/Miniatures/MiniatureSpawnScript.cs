#region Usings

using UnityEngine;
using System.Collections;

#endregion



public class MiniatureSpawnScript : MonoBehaviour
{

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

    private bool newScaleReady = false;

    private GameObject actualController;

    private Vector3 newMiniatureScaleForSpawning;



    #endregion

    #region Public Properties

    public float treshold = 0.05f;

    #endregion

    #region Methods

    void Start()
    {
        GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision;
        parent = gameObject.transform.parent;
        
        //SetScaleBoxesFadeIn(false);
    }

    void Update()
    {

        if (parent == null) noParentDestroy = true;    


        if (toDestroy || noParentDestroy)
        {
            CheckAndDestroy();
        }

        if (newScaleReady)
        {
            SetNewScale();
        }
    }

    private void ControllerCollision(GameObject gameObj, bool isEnter)
    {
        if (isEnter && !active)
        {
            gameObj.GetComponent<ControlObjects>().TriggerDown += StartSpawn;
            active = true;
            actualController = gameObj;
        }
        else if (!isEnter && !GetComponent<ObjectInteractionScript>().GetIsSelected() && active)
        {
            gameObj.GetComponent<ControlObjects>().TriggerDown -=StartSpawn;
            active = false;
        }
    }


    public void StartSpawn(GameObject controller)
    {
        StopRotation();
        GetComponent<ObjectInteractionScript>().SetIsSelected(true);
        controller.GetComponent<ControlObjects>().ControllerMove += Move;
        rotationParent = controller.GetComponent<ControlObjects>().rotationCenter;

        controllerFirstPosition = controllerSecondPosition = controller.transform.position;

        baseScale = gameObject.transform.localScale;
        basePosition = gameObject.transform.localPosition;
        baseRotation = gameObject.transform.localRotation;
        

        rotationParent.transform.position = controller.transform.position;
        rotationParent.transform.rotation = controller.transform.rotation;

        gameObject.transform.parent = rotationParent.transform;

        if (GetComponent<Rigidbody>() != null) Destroy(gameObject.GetComponent<Rigidbody>());
        toDestroy = false;

        SetSpawningObjectScale();
        controller.GetComponent<ControlObjects>().TriggerUp += SpawnObject;

        SetScaleBoxesFadeIn(true);
    }

    public void Move(GameObject controller)
    {
        controllerSecondPosition = controllerFirstPosition;
        controllerFirstPosition = controller.transform.position;

        rotationParent.transform.position = controller.transform.position;
        rotationParent.transform.rotation = controller.transform.rotation;
    }

    public void SpawnObject(GameObject controller)
    {
        controller.GetComponent<ControlObjects>().ControllerMove -= Move;
        GetComponent<ObjectInteractionScript>().SetIsSelected(false);
        SceneObject sceneObjectToSpawn = gameObject.GetComponent<SceneObjectScript>().miniaturedSceneObject;
        SetScaleBoxesFadeIn(false);

        ChechAndSetToDestroy();

        if (!toDestroy)
        {
            if (newScaleReady)
            {
                newScaleReady = false;
                gameObject.transform.localScale = newMiniatureScaleForSpawning;
            }
            GameObject.Find("Player").GetComponent<ObjectSpawnerScript>().SpawnObject(sceneObjectToSpawn, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.localScale, controller, true);
            if(!noParentDestroy)returnToMainPosition();
        }
        controller.GetComponent<ControlObjects>().TriggerUp -= SpawnObject;
        
        ControllerCollision(actualController, false);
        StartRotation();
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
            actualController.GetComponent<ControlObjects>().TriggerDown -= StartSpawn;
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
           // actualController.GetComponent<ControlObjects>().TriggerDown -= StartSpawn;
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

    private void SetSpawningObjectScale()
    {

        //if (GetComponent<ImageMiniatureScript>() != null)
        //{
        //    newMiniatureScaleForSpawning = GetComponent<ImageMiniatureScript>().GetNewImageScalesRatio();
        //    newScaleReady = true;
        //}
        //else if (GetComponent<ShapeMiniatureScript>() != null)
        //{
        //    newMiniatureScaleForSpawning = GetComponent<ShapeMiniatureScript>().GetNewImageScalesRatio();
        //    newScaleReady = true;
        //}
        //else if (GetComponent<Shapes3DMiniatureScript>() != null)
        //{
        //    newMiniatureScaleForSpawning = GetComponent<Shapes3DMiniatureScript>().GetWorldObjectScale();
        //    newScaleReady = true;
        //}
        //else if (GetComponent<VideoMiniatureScript>() != null)
        //{
        //    newMiniatureScaleForSpawning = GetComponent<VideoMiniatureScript>().GetNewImageScalesRatio();
        //    newScaleReady = true;
        //}
        //else if (GetComponent<TextGroupMiniatureScript>() != null)
        //{
        //    newMiniatureScaleForSpawning = GetComponent<TextGroupMiniatureScript>().GetNewTextGroupScalesRatio();
        //    newScaleReady = true;
        //}
            newMiniatureScaleForSpawning = transform.localScale*3.0f;
            newScaleReady = true;

    }

    private void SetNewScale()
    {

        bool zScaleReady = false;
        bool xScaleReady = false;
        bool yScaleReady = false;

        float speed = 0.01f;

        if (transform.localScale.z < newMiniatureScaleForSpawning.z)
        {
            transform.localScale += new Vector3(0.0f, 0.0f, speed);

        }
        else {
            zScaleReady = true;
        }

        if (transform.localScale.x < newMiniatureScaleForSpawning.x)
        {
            transform.localScale += new Vector3(speed, 0.0f, 0.0f);
        }
        else {
            xScaleReady = true;
        }

        if (transform.localScale.y < newMiniatureScaleForSpawning.y)
        {
            transform.localScale += new Vector3(0.0f, speed, 0.0f);
        }
        else {
            yScaleReady = true;
        }

        if (xScaleReady && zScaleReady && yScaleReady) newScaleReady = false;

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

    private void StopRotation()
    {
        GameObject.Find("Toolbar").GetComponent<ToolbarRotationScript>().RemoveFromRotationEvent();
        GameObject.Find("Toolbar").GetComponent<ToolbarRotationScript>().SpeedStop();
    }

    private void StartRotation()
    {
        GameObject.Find("Toolbar").GetComponent<ToolbarRotationScript>().AddToRotationEvent();
    }


    #endregion

}
