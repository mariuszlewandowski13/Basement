#region Usings

using UnityEngine;

#endregion

public class SaveTransformForEditButtonScript : MonoBehaviour {

    #region Public Properties

    public GameObject parentObject;
    public GameObject textObject;

    #endregion

    #region Private Properties
    private bool active = true;
    #endregion

    #region Methods
    void Update()
    {
        if (parentObject != null && active)
        {
            gameObject.transform.position = parentObject.transform.position;
            gameObject.transform.rotation = parentObject.transform.rotation;
        }
        if (parentObject == null || textObject == null) DestroyImmediate(gameObject);

        FadeColorpicker();

    }

    private void FadeColorpicker()
    {
        if (textObject != null)
        {
            if (!active && textObject.GetComponent<SelectingObjectsScript>().selected)
            {
                transform.FindChild("button").gameObject.SetActive(true);
                active = true;
            }
            else if (active && !textObject.GetComponent<SelectingObjectsScript>().selected)
            {
                transform.FindChild("button").gameObject.SetActive(false);
                active = false;
            }
        }
        
    }

    #endregion
}
