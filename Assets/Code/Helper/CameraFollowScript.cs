using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour {

    public GameObject cameraToFollow;
    public float positionOffset;


	void Update () {
        if (cameraToFollow != null)
        {
            gameObject.transform.position = cameraToFollow.transform.position + positionOffset*cameraToFollow.transform.forward;
            gameObject.transform.rotation = cameraToFollow.transform.rotation;
        }
	}
}
