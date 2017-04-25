#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class ChangeParentOnRemoteScript : MonoBehaviour {

    #region private Properties

    private Transform prevParent;

    #endregion

    #region Methods

    [PunRPC]
    private void SetParentFromNetwork(int number)
    {
        if (number == -1)
        {
            transform.parent = null;
        }
        else
        {
            transform.parent = PhotonView.Find(number).transform;
        }
    }

    [PunRPC]
    private void SaveParentFromNetwork()
    {
        prevParent = transform.parent;
    }

    [PunRPC]
    private void LoadParentFromNetwork()
    {
        transform.parent = prevParent;
    }

    #endregion
}
