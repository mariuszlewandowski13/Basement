#region Usings

using UnityEngine;

#endregion

public class ModelOverrideScript : MonoBehaviour {

    #region Public Properties
    public GameObject objectToDeactive;
    #endregion

    #region Methods

    void Update () {
        if (objectToDeactive.GetComponent<Renderer>() != null)
        {
            Destroy(objectToDeactive.GetComponent<Renderer>());
            Destroy(this);
        }
	}

    #endregion

}
