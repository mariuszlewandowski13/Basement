using UnityEngine;
using System.Collections;

public class KeyboardMovementScript : MonoBehaviour {

    public Transform objectToFollow;

	
	void OnEnable () {
        if (objectToFollow != null)
        {
            Vector3 newRotation = objectToFollow.transform.rotation.eulerAngles;


            Vector3 lookRotation =  Quaternion.LookRotation(-gameObject.GetComponent<ToolbarMovementScript>().cameraEye.forward).eulerAngles;
            newRotation.y = lookRotation.y;
            gameObject.transform.rotation = Quaternion.Euler(newRotation);
            gameObject.GetComponent<ToolbarMovementScript>().positionYOffset = objectToFollow.GetComponent<ToolbarMovementScript>().positionYOffset;
        }
	}
}
