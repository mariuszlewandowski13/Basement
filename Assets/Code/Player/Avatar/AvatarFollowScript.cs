using UnityEngine;
using System.Collections;

public class AvatarFollowScript : MonoBehaviour {

    #region Public Properties

    public bool isEnabled = false;
    public float yOffset = 1.25f;
    public float posTreshold = 0.007f;
    public float rotTreshold = 1.0f;

    #endregion

    void Update () {

        if (isEnabled)
        {
            Vector3 newPosition = Camera.main.transform.position;
            newPosition.y = yOffset;

            Vector3 newRotation = transform.rotation.eulerAngles;

            newRotation.y = Camera.main.transform.rotation.eulerAngles.y;


            if (Vector3.Distance(transform.position, newPosition) > posTreshold)
            {
                transform.position = newPosition;
            }
            if (Quaternion.Angle(transform.rotation, Quaternion.Euler(newRotation)) > rotTreshold)
            {
                transform.rotation = Quaternion.Euler(newRotation);
            }


        }
        
	}
}
