#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class ObjectToolsScript : MonoBehaviour {


    #region Methods
    void Start () {
       // UpdatePosition();
    }

    void UpdatePosition()
    {
        Renderer rend = gameObject.transform.parent.GetComponent<Renderer>();
        gameObject.transform.position = gameObject.transform.parent.position;

        Vector3 shiftVector = rend.bounds.extents;
        shiftVector.y = -shiftVector.y;

        gameObject.transform.position += shiftVector;
    }

    #endregion

}
