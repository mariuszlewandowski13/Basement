#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class TextObjectToolsScript : MonoBehaviour
{


    #region Methods
    void Start()
    {
        Renderer rend = gameObject.transform.parent.GetComponent<Renderer>();
        Vector3 shiftVector = rend.bounds.size;
        shiftVector.y = -shiftVector.y;
        Debug.Log(shiftVector);
        gameObject.transform.position += shiftVector;
    }

    #endregion

}