using UnityEngine;
using System.Collections;

public class SeparatorScript : MonoBehaviour {

    #region Public Properties

    public SeparatorObject separatorObject;

    #endregion

    public void UpdateColorInSeparatorObject()
    {
        separatorObject.color = GetComponent<Renderer>().material.color;
    }

    public void UpdateColorInRenderer()
    {
       GetComponent<Renderer>().material.SetColor("_Color", separatorObject.color);
    }

    public void SetSeparatorObject(SeparatorObject sep)
    {
        separatorObject = new SeparatorObject(sep);
        UpdateColorInRenderer();
    }

    public void LoadFromSeparatorObject()
    {
        if (separatorObject != null)
        {
            transform.position = separatorObject.GetSavedPosition();
            transform.rotation = separatorObject.GetSavedRotation();
            transform.localScale = separatorObject.GetSavedScale();
        }
    }


    public void SetSeparatorByNetwork()
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("SetSeparatorByNetwork", PhotonTargets.Others, separatorObject.prefabName, separatorObject.color.r, separatorObject.color.g, separatorObject.color.b, separatorObject.color.a);
        }
    }

    [PunRPC]
    private void SetSeparatorByNetwork(string name, float r, float g, float b, float a)
    {
        separatorObject.prefabName = name;
        separatorObject.color = new Color(r, g, b, a);
        UpdateColorInRenderer();
    }

}
