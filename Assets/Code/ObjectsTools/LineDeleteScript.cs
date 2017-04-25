using UnityEngine;
using System.Collections;

public class LineDeleteScript : MonoBehaviour {

    #region Public Properties

    public GameObject line = null;

    #endregion

    #region Private Properties

    private bool active;
    private bool highlighted;
    #endregion

    void Start()
    {
        GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision;
    }

    private void ControllerCollision(GameObject gameObj, bool isEnter)
    {
        if (isEnter && !active)
        {
            active = true;
            gameObj.GetComponent<ControlObjects>().TriggerDown += OnTriggerDown;
        }
        else if (!isEnter && !GetComponent<ObjectInteractionScript>().GetIsSelected() && active)
        {
            active = false;
            gameObj.GetComponent<ControlObjects>().TriggerDown -= OnTriggerDown;
        }
        ChangeHighlighting();
    }

    private void OnTriggerDown(GameObject controller)
    {
        if (line != null)
        {
            SaveManager.DeleteFromDatabase(line.GetComponent<PhotonView>().viewID, line.GetComponent<LineScript>().lineObject.pointsFileName);
            GameObject.Find("RemoteDrawingObject").GetComponent<RemoteDrawingScript>().DestroyLine(line.GetComponent<PhotonView>().viewID);
            Destroy(line);
        }
    }

    

    void Update()
    {
        
        if (line == null) Destroy(gameObject);
    }

    private void ChangeHighlighting()
    {
        if (active && !highlighted)
        {
            GetComponent<HighlightingSystem.Highlighter>().ConstantOn(ApplicationStaticData.toolsHighligthing);
            highlighted = true;
        }
        else if (!active && highlighted)
        {
            GetComponent<HighlightingSystem.Highlighter>().ConstantOff();
            highlighted = false;
        }

    }
}
