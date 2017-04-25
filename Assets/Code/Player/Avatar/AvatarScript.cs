using UnityEngine;
using System.Collections;

public class AvatarScript : MonoBehaviour {

    public void SetLayer(string name)
    {
        gameObject.layer = LayerMask.NameToLayer(name);
        Renderer[] renderersInChildren = GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in renderersInChildren)
        {
            childRenderer.gameObject.layer = LayerMask.NameToLayer(name);
        }
    }

    public void SetAvatarName(string name)
    {
        //transform.FindChild("avatarName").GetComponent<CubeLerp>().serializeRotation = false;
        transform.FindChild("avatarName").GetComponent<TextMesh>().text = name;
    }


    public void SetAvatarColor(Color color)
    {
        
        Renderer[] childrenRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in childrenRenderers)
        {
            renderer.material.SetColor("_Solid_Color", color);
            //renderer.material.SetColor("_Color", color);
        }

        if (GetComponent<Renderer>() != null)
        {
            GetComponent<Renderer>().material.SetColor("_Solid_Color", color);
        }
    }

    public void SetAvatarColorByNetwork(Color color)
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("LoadAvatarColorFromNetwork", PhotonTargets.OthersBuffered, color.r, color.g, color.b, color.a);
        }
        
    }

    [PunRPC]
    public void LoadAvatarColorFromNetwork(float r, float g, float b, float a)
    {
        SetAvatarColor(new Color(r, g, b, a));
    }

    public void SetAvatarNameByNetwork(string name)
    {
       // gameObject.transform.FindChild("avatarName").GetComponent<CubeLerp>().serializeRotation = false;
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("LoadAvatarNameFromNetwork", PhotonTargets.OthersBuffered, name);
        }

    }

    [PunRPC]
    public void LoadAvatarNameFromNetwork(string name)
    {
        SetAvatarName(name);
        gameObject.transform.FindChild("avatarName").GetComponent<FFollowScript>().ObjectToLookAt = GameObject.Find("Camera (eye)");
        gameObject.transform.FindChild("avatarName").GetComponent<FFollowScript>().LookAtRot = true;
      // gameObject.transform.FindChild("avatarName").GetComponent<CubeLerp>().serializeRotation = false;
    }


}
