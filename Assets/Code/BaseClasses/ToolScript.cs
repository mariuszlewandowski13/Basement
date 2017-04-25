#region Usings

using UnityEngine;

#endregion

public class ToolScript : MonoBehaviour {

    #region Public Properties

    public ToolsButtons toolsObject;
    private bool isPlaying = false;
    private bool highlighted = false;


    #endregion

    #region Private Properties

    private GameObject toolbar;

    private bool active = false;
    private bool colorsBar = false;

    #endregion

    #region Methods

    void Start()
    {
        toolbar = gameObject.transform.parent.parent.parent.gameObject;
        GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision;
    }

    public void SetToolsObjects(ToolsButtons obj, bool colorsBar)
    {
        toolsObject = obj;
        this.colorsBar = colorsBar;
    }

    private void ControllerCollision(GameObject gameObj, bool isEnter)
    {
        if (isEnter && !active)
        {
            gameObj.GetComponent<ControlObjects>().TriggerDown += OnTriggerDown;
            active = true;
        }
        else if (!isEnter && !GetComponent<ObjectInteractionScript>().GetIsSelected() && active)
        {
            gameObj.GetComponent<ControlObjects>().TriggerDown -= OnTriggerDown;
            active = false;
        }
    }

    private void OnTriggerDown(GameObject controller)
    {

        controller.GetComponent<ControlObjects>().TriggerDown -= OnTriggerDown;
        Statistics.AddStatisticsToolbarEvent(toolsObject.statsNum);
        if (toolsObject.sourceForSceneObjects > 0 )
        {
            toolsObject.LoadSceneObjects(controller);
            if (toolsObject.sourceForSceneObjects < 3)
            {
                toolsObject.newSceneObjectsList += LoadSceneObjects;
                toolbar.GetComponent<ToolbarManagerScript>().ClearToolbar();
            }
        }
        else if (toolsObject.sceneObjects != null) {
            toolbar.GetComponent<ToolbarManagerScript>().LoadNewSceneObjectsList(toolsObject, colorsBar);
        }
        
    }

    void OnEnable()
    {
        if (transform.GetChild(0).GetComponent<Animator>() != null && !isPlaying)
        {
            transform.GetChild(0).GetComponent<Animator>().StartPlayback();
            isPlaying = false;
        }
    }

    void Update()
    {
            if (!highlighted && (GetComponent<ObjectInteractionScript>().GetIsSelected() || GetComponent<ObjectInteractionScript>().GetCollision()) )
            {
                GetComponent<HighlightingSystem.Highlighter>().ConstantOn(ApplicationStaticData.toolsHighligthing);
                if (transform.GetChild(0).GetComponent<Animator>() != null && !isPlaying)
                {
                    transform.GetChild(0).GetComponent<Animator>().StopPlayback();
                    isPlaying = true;
                }
            highlighted = true;
            }
            else if(highlighted && (!GetComponent<ObjectInteractionScript>().GetIsSelected() && !GetComponent<ObjectInteractionScript>().GetCollision()) )
        {
            GetComponent<HighlightingSystem.Highlighter>().ConstantOff();
            if (isPlaying && transform.GetChild(0).GetComponent<Animator>() != null )
                {
                    transform.GetChild(0).GetComponent<Animator>().StartPlayback();
                    isPlaying = false;
                }
            highlighted = false;
        }
        

    }
    public void LoadSceneObjects()
    {
        toolsObject.newSceneObjectsList -= LoadSceneObjects;
        if (toolsObject.sceneObjects.Length > 0)
        {
            toolbar.GetComponent<ToolbarManagerScript>().LoadNewSceneObjectsList(toolsObject, colorsBar); 
        }
        else
        {
            toolbar.GetComponent<ToolbarManagerScript>().MainBar();
        }

    }

    #endregion
}
