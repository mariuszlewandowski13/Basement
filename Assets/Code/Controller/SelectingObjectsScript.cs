#region Usings

using UnityEngine;
using System.Collections.Generic;

#endregion

public class SelectingObjectsScript : MonoBehaviour {

    #region Private Properties

    private bool added = false;

    private GameObject actualController;
    private List<GameObject> controllers;

    private bool highlighted = false;

    #endregion

    #region Public Properties

    public bool selected = false;
    public bool active = false;

    #endregion

    #region Methods

    void Awake()
    {
        controllers = new List<GameObject>();
        selected = false;
        active = false;
    }

    void Start()
    {
        GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision; 
    }

    public void ControllerCollision(GameObject gameObj, bool isEnter)
    {
        if (isEnter && !active)
        {
            active = true;
            gameObj.GetComponent<ControlObjects>().TriggerDown += OnTriggerDown;
            added = true;
            actualController = gameObj;

        }
        else if (!isEnter && active && added)
        {
            active = false;
            gameObj.GetComponent<ControlObjects>().TriggerDown -= OnTriggerDown;
            added = false;
        }
    }

    private void OnTriggerDown(GameObject controller)
    {
        controller.GetComponent<ControlObjects>().SetSelected(gameObject);
    }

    private void FadeOutComponents()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<ObjectsFadeScript>() != null)
            {
                child.GetComponent<ObjectsFadeScript>().DecreaseFadeIn();
            }
        }
    }

    private void Update()
    {
        if ((active || selected) && !highlighted)
        {
            GetComponent<HighlightingSystem.Highlighter>().ReinitMaterials();
            GetComponent<HighlightingSystem.Highlighter>().ConstantOn(ApplicationStaticData.objectsHighligthing);
                highlighted = true;
        }
        else if ((!active && !selected) && highlighted)
        {
            GetComponent<HighlightingSystem.Highlighter>().ConstantOff();
            highlighted = false;
        }
            
    }

    void OnDestroy()
    {
        if (added)
        {
            actualController.GetComponent<ControlObjects>().TriggerDown -= OnTriggerDown;
        }
    }

    public void SetAsSelected(GameObject controller)
    {
        if (!controllers.Contains(controller))
        {
            controllers.Add(controller);
            selected = true;
            FadeInComponents();
        }
        
    }

    public void SetAsNonSelected(GameObject controller)
    {
        controllers.Remove(controller);
        if (controllers.Count == 0)
        {
            selected = false;
            FadeOutComponents();
        }    
    }

    private void FadeInComponents()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<ObjectsFadeScript>() != null)
            {
                child.GetComponent<ObjectsFadeScript>().IncreaseFadeIn();
            }
        }
    }


    public void ResetSelecting()
    {
        active = false;
        selected = false;
        controllers = new List<GameObject>();
    }
    #endregion

}
