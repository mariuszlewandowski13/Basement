#region Usings

using UnityEngine;

#endregion

public class FollowScript : MonoBehaviour {

    #region Public Properties

    public GameObject parentObject;
    public float posTreshold = 0.007f;
    public float rotTreshold = 1.0f;

    #endregion

    #region Methods

    void Update () {
        if (parentObject != null)
        {
           if (Vector3.Distance(gameObject.transform.position, parentObject.transform.position) > posTreshold)
            {
                gameObject.transform.position = parentObject.transform.position;
            }
            if (Quaternion.Angle(gameObject.transform.rotation, parentObject.transform.rotation) > rotTreshold)
            {
                gameObject.transform.rotation = parentObject.transform.rotation;
            }      
        }
            
	}
    #endregion

}
