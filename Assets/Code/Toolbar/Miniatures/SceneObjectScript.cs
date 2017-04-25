#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class SceneObjectScript : MonoBehaviour {

    #region Public Properties

    public SceneObject miniaturedSceneObject;

    #endregion

    #region Methods

    void Start()
    {
        if (gameObject.GetComponent<Renderer>() != null)
        {
            gameObject.GetComponent<Renderer>().sortingLayerName = "PhysicalObjectsSortingLayer";
        }
    }


    #endregion

}
