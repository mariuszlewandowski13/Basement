#region Usings

using UnityEngine;
using System.Collections.Generic;

#endregion


public class ControlObjects : MonoBehaviour
{
    #region Public Events & Delegates

    public delegate void Interaction(GameObject gameObject);
    public event Interaction ControllerMove;
    public event Interaction TriggerUp;
    public event Interaction TriggerDown;

    public static event Interaction TriggerDownGlobal;

    public event Interaction GripDown;
    
    public event Interaction GripUp;
 
    public GameObject rotationCenter;

    public GameObject secondController;

    public GameObject pickup = null;

    public List<GameObject> collidingGameObjects;
    public List<GameObject> realCollidingGameObjects;

    #endregion

    #region Private Properties

    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    public Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;


    private bool canEnter = true;
    public GameObject selected;

    private object selectedLock = new object();
    private object pickupLock = new object();
    private object canEnterLock = new object();

    private float vibrationStartTime;
    private float vibrationTime;
    private bool vibrate = false;

    #endregion
    #region Methods

    void Start()
    {
        trackedObj = transform.parent.GetComponent<SteamVR_TrackedObject>();
        collidingGameObjects = new List<GameObject>();
        realCollidingGameObjects = new List<GameObject>();
    }

    void Update()
    {
        if (controller == null)
        {
            return;
        }
            bool gripDown = controller.GetPressDown(gripButton);
            bool gripUp = controller.GetPressUp(gripButton);
            bool triggerDown = controller.GetPressDown(triggerButton);
            bool triggerUp = controller.GetPressUp(triggerButton);

            lock(pickupLock)
            {

                    if (triggerDown && TriggerDown != null)
                    {
                        TriggerDown(gameObject);
                    }

                    if (triggerDown && TriggerDownGlobal != null)
                    {
                        TriggerDownGlobal(gameObject);
                    }

                    if (triggerUp && TriggerUp != null)
                    {
                        TriggerUp(gameObject);
                    }

                    if (gripDown && GripDown != null)
                    {
                        GripDown(gameObject);
                    }

                    if (gripUp && GripUp != null)
                    {
                        GripUp(gameObject);
                    }

                    if (ControllerMove != null)
                    {
                        ControllerMove(gameObject);
                    }
                    if (triggerDown && pickup == null)
                    {
                        DeleteSelection();
                    }
            
        }
        CheckCollidingObjects();
        realCollidingGameObjects.Clear();

        if (vibrate)
        {
            controller.TriggerHapticPulse(1500);
        }

        if (vibrate && Time.time - vibrationStartTime > vibrationTime)
        {
            vibrate = false;
            vibrationTime = 0.0f;
        }

    }

    private void OnTriggerEnter(Collider collider)
    {
        lock (canEnterLock)
        {
            if (canEnter)
            {
                lock (pickupLock)
                {
                    if (pickup != null)
                    {
                        if (collider.gameObject.GetComponent<ObjectInteractionScript>() != null && !pickup.GetComponent<ObjectInteractionScript>().GetIsSelected() && secondController.GetComponent<ControlObjects>().GetPickup() != collider.gameObject)
                        {
                            pickup.GetComponent<ObjectInteractionScript>().SetCollision(false, gameObject);
                            collidingGameObjects.Add(pickup);
                            pickup = collider.gameObject;
                            pickup.GetComponent<ObjectInteractionScript>().SetCollision(true, gameObject);
                        }

                    }
                    else {
                        if (collider.gameObject.GetComponent<ObjectInteractionScript>() != null && secondController.GetComponent<ControlObjects>().GetPickup() != collider.gameObject)
                        {
                            pickup = collider.gameObject;
                            pickup.GetComponent<ObjectInteractionScript>().SetCollision(true, gameObject);
                        }
                    }

                }
            }
        }
        
    }


    public GameObject GetPickup()
    {
        GameObject result;
        lock(pickupLock)
        {
            result = pickup;
        }
        return result;

    }

    public void LockEntering(bool lockedValue)
    {
        lock(canEnterLock)
        {
            canEnter = !lockedValue;
        }
    }

    public void SetSelected(GameObject newSelected)
    {
        lock(selectedLock)
        {
            if (selected != null)
            {
                if (selected != newSelected)
                {
                    selected.GetComponent<SelectingObjectsScript>().SetAsNonSelected(gameObject);
                    selected = newSelected;
                    selected.GetComponent<SelectingObjectsScript>().SetAsSelected(gameObject);
                }
            }
            else
            {
                selected = newSelected;
                selected.GetComponent<SelectingObjectsScript>().SetAsSelected(gameObject);
            }
        }
    }


    public void DeleteSelection()
    {
        lock (selectedLock)
        {
            if (selected != null)
            {
                selected.GetComponent<SelectingObjectsScript>().SetAsNonSelected(gameObject);
                selected = null;
            }
        }

    }

    void OnTriggerStay(Collider collider)
    {
        if ((collidingGameObjects.Contains(collider.gameObject) || pickup == collider.gameObject) && !realCollidingGameObjects.Contains(collider.gameObject))
        {
            realCollidingGameObjects.Add(collider.gameObject);
        }

    }

    private void CheckCollidingObjects()
    {
        List<GameObject> objToRemove = new List<GameObject>();
        
        foreach (GameObject obj in collidingGameObjects)
        {
            if (!realCollidingGameObjects.Contains(obj))
            {
                objToRemove.Add(obj);
            }
        }

        foreach (GameObject obj in objToRemove)
        {
            collidingGameObjects.Remove(obj);
        }

        if (pickup != null && !realCollidingGameObjects.Contains(pickup) && !pickup.GetComponent<ObjectInteractionScript>().GetIsSelected())
        {
            pickup.GetComponent<ObjectInteractionScript>().SetCollision(false, gameObject);
            pickup = null;
        }
    }

    public void SetVibrations(float sec)
    {
        vibrationStartTime = Time.time;
        vibrationTime = sec;
        vibrate = true;
    }


    #endregion
}