using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFollowScript : MonoBehaviour {

    public bool posX;
    public bool posY;
    public bool posZ;

    public bool rotX;
    public bool rotY;
    public bool rotZ;

    public bool LookAtRot;

    public GameObject parentObject;
    public GameObject ObjectToLookAt;

    public float posTreshold = 0.007f;
    public float rotTreshold = 1.0f;

    public float posYOffset;
    public float rotYOffset;

    void Update()
    {
        if (parentObject != null)
        {
            if (Vector3.Distance(gameObject.transform.position, parentObject.transform.position) > posTreshold)
            {
                Vector3 newPos = new Vector3();
                if (posX) newPos.x = parentObject.transform.position.x;

                if (posY) newPos.y = parentObject.transform.position.y;
                else newPos.y = posYOffset;

                if (posZ) newPos.z = parentObject.transform.position.z;

                gameObject.transform.position = newPos;
            }
            if (LookAtRot && ObjectToLookAt != null)
            {
                gameObject.transform.LookAt(ObjectToLookAt.transform);
                gameObject.transform.Rotate(0.0f, rotYOffset, 0.0f);
            } 
                else {
                    gameObject.transform.rotation = parentObject.transform.rotation;
                }
        }

    }

}
