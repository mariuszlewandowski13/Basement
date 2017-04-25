using UnityEngine;
using System.Collections;

public class TextEditButtonScript : MonoBehaviour {
    #region Private Properties

    private bool active = false;
    private bool highlighted = false;

    public GameObject myTextObject;

    #endregion

    void Start()
    {
        GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision;
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

    public void SetTextObject(GameObject newTextObject)
    {
        myTextObject = newTextObject;
    }

    private void OnTriggerDown(GameObject controller)
    {
        if (myTextObject != null)
        {
            myTextObject.GetComponent<TypingScript>().StartTyping();
        }
    }

    void Update()
    {
        if (!highlighted && (GetComponent<ObjectInteractionScript>().GetIsSelected() || GetComponent<ObjectInteractionScript>().GetCollision()))
        {
            GetComponent<HighlightingSystem.Highlighter>().ConstantOn(ApplicationStaticData.toolsHighligthing);
            highlighted = true;
        }
        else if (highlighted && (!GetComponent<ObjectInteractionScript>().GetIsSelected() && !GetComponent<ObjectInteractionScript>().GetCollision()))
        {
            GetComponent<HighlightingSystem.Highlighter>().ConstantOff();
            highlighted = false;
        }
    }
}
