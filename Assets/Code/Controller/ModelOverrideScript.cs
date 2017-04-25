#region Usings

using UnityEngine;

#endregion

public class ModelOverrideScript : MonoBehaviour {

    #region Public Properties
    public GameObject objectToDeactive;
    #endregion

    #region Methods

    void Update () {
        if (objectToDeactive.GetComponentInChildren<Renderer>() != null)
        {
            Destroy(objectToDeactive);
            Destroy(this);
        }
	}

    #endregion

}
