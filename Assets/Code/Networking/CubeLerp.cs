//
// A (very) simple network interpolation script, using Lerp().
//
// This will lag-behind, compared to the moving cube on the controlling client.
// Actually, we deliberately lag behing a bit more, to avoid stops, if updates arrive late.
//
// This script does not hide loss very well and might stop the local cube.
//

using UnityEngine;


[RequireComponent(typeof (PhotonView))]
public class CubeLerp : Photon.MonoBehaviour, IPunObservable
{
    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;

    private Quaternion latestCorrectRot;
    private Quaternion onUpdateRot;

    private Vector3 latestCorrectScale;

    private float fraction;
    private float fractionRot;

    public bool serializeRotation = true;

    public void Start()
    {
        this.latestCorrectPos = transform.position;
        this.onUpdatePos = transform.position;

        this.latestCorrectRot = transform.rotation;
        this.onUpdateRot = transform.rotation;

        this.latestCorrectScale = transform.localScale;

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;
            Vector3 scale = transform.localScale;

            //if (pos != lastSentPosition || rot != lastSentRotation || scale != lastSentScale)
            //{
                stream.Serialize(ref pos);
                stream.Serialize(ref rot);
                stream.Serialize(ref scale);
            //    Debug.Log(gameObject.name + ": " + GetComponent<PhotonView>().viewID);

            //    lastSentScale = scale;
            //    lastSentPosition = pos;
            //    lastSentRotation = rot;
            //}
        }
        else
        {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;
            Vector3 scale = Vector3.zero;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref scale);

            this.latestCorrectPos = pos;                // save this to move towards it in FixedUpdate()
            this.onUpdatePos = transform.position; // we interpolate from here to latestCorrectPos
            this.fraction = 0;         // reset the fraction we alreay moved. see Update()

            this.latestCorrectRot = rot;                // save this to move towards it in FixedUpdate()
            this.onUpdateRot = transform.rotation; // we interpolate from here to latestCorrectPos
            this.fractionRot = 0;

            this.latestCorrectScale = scale;                // save this to move towards it in FixedUpdate()

            //transform.localRotation = rot;              // this sample doesn't smooth rotation
        }
    }


    public void Update()
    {
        if (this.photonView.isMine)
        {
            return;     // if this object is under our control, we don't need to apply received position-updates 
        }

        if (this.onUpdatePos != this.latestCorrectPos)
        {
            this.fraction = this.fraction + Time.deltaTime * 9;
            transform.position = Vector3.Lerp(this.onUpdatePos, this.latestCorrectPos, this.fraction); // set our pos between A and B
        }


        if (this.onUpdateRot != this.latestCorrectRot)
        {
            this.fractionRot = this.fractionRot + Time.deltaTime * 9;
            transform.rotation = Quaternion.Lerp(this.onUpdateRot, this.latestCorrectRot, this.fractionRot); 
        }

        if (transform.localScale != this.latestCorrectScale)
        {
            transform.localScale = this.latestCorrectScale; 
        }
    }




}