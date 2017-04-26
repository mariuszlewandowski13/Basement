using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour {

    public delegate void Result(bool decision);
    public event Result canTeleportChecked;

    private string lastCheckedDestination;

    private bool toTeleport;

    public void CheckCanTeleport(string teleportDestination)
    {
        lastCheckedDestination = teleportDestination;
        WWWForm form = new WWWForm();
        form.AddField("destination", teleportDestination);
        WWW w = new WWW("http://serwer1642668.home.pl/BASEMENT/scripts/CheckCanTeleport.php", form);
        StartCoroutine(request(w));
        
    }

    public void StartTeleport()
    {
        if (lastCheckedDestination != null)
        {
            if (PhotonNetwork.inRoom)
            {
                PhotonNetwork.Disconnect();
            }
            toTeleport = true;
        }
    }

    void Update()
    {
        if (toTeleport && !PhotonNetwork.inRoom)
        {
            EndTeleport();
            toTeleport = false;
        }
    }

    IEnumerator request(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            
        }
        else {
            Debug.Log(w.error);
            if (canTeleportChecked != null)
            {
                canTeleportChecked(false);
            }
        }

        

        if (canTeleportChecked != null)
        {
            if (w.text == "0")
            {
                canTeleportChecked(false);
            }
            else {
                canTeleportChecked(true);
            }
            
        }

    }

    private void EndTeleport()
    {
        ApplicationStaticData.roomToConnectName = lastCheckedDestination + "_ROOM";
        GameObject.Find("Player").GetComponent<SaveLoadManager>().ClearEnviroment();
        GameObject.Find("Player").GetComponent<ConnectAndJoinRandom>().PrepareForReconnect();
        GameObject.Find("Player").GetComponent<OnLoadRoomLoadingScript>().created = false;
        lastCheckedDestination = null;
    }
}
