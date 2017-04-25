#region Usings

using UnityEngine;
using System.Collections;

#endregion


public class SaveScript : MonoBehaviour {

    #region Public Properties

    private SaveLoadManager manager;
    private bool active = false;
    #endregion


    void Start()
    {
        manager = GameObject.Find("Player").GetComponent<SaveLoadManager>();
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
    }

    private void OnTriggerDown(GameObject controller)
    {
        if(manager != null)manager.SaveGame();
    }

    void Update()
    {
        if (active && GetComponent<HighlightingSystem.Highlighter>() != null) GetComponent<HighlightingSystem.Highlighter>().On();
    }

}
