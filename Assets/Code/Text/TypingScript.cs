#region Usings

using UnityEngine;
using System.Collections;
using Valve.VR;
using System;


#endregion


public class TypingScript : MonoBehaviour {

    #region Private Properties

    private string text;
    private GameObject keyboard;

    #endregion

    #region Methods


    public void StartTyping()
    {
        keyboard = KeyboardHandlerScript.InitializeKeyboard();
        keyboard.GetComponent<KeyboardScript>().textBoxActive = true;
        
        keyboard.GetComponent<KeyboardScript>().NewCharAdded += GetChar;
        text = GetComponent<TextMesh>().text;
        keyboard.GetComponent<KeyboardScript>().text = GetComponent<TextMesh>().text;
    }

    private void GetChar(string newChar)
    {
        if (newChar == "back")
        {
            if (text.Length > 0)
            {
                text = text.Substring(0, text.Length - 1);
            }
        }
        else if (newChar == "newLine")
        {
            text += Environment.NewLine;
        }
        else if (newChar == "done")
        {
            EndTyping();
        }
        else
        {
            text += newChar;
        }

        GetComponent<TextMesh>().text = text;
        GetComponent<TextScript>().textObject.text = text;
        GetComponent<TextScript>().UpdateTextByNetwork();

        //GetComponent<TextScript>().UpdateColor();
        UpdateTextBoxCollider();

        UpdateScaleBoxSize();


        UpdateColliderByNetwork();
    }


    //private void OnKeyboard(object[] args)
    //{
    //    VREvent_t evt = (VREvent_t)args[0];
    //    //Debug.Log(evt.data.keyboard.cNewInput0);
    //    //Debug.Log(evt.data.keyboard.cNewInput1);
    //    //Debug.Log(evt.data.keyboard.cNewInput2);
    //    //Debug.Log(evt.data.keyboard.cNewInput3);
    //    //Debug.Log(evt.data.keyboard.cNewInput4);
    //    //Debug.Log(evt.data.keyboard.cNewInput5);
    //    //Debug.Log(evt.data.keyboard.cNewInput6);
    //    //Debug.Log(evt.data.keyboard.cNewInput7);


    //    if ((char)evt.data.keyboard.cNewInput0 == '\b')
    //    {
    //        if (text.Length > 0)
    //        {
    //            text = text.Substring(0, text.Length - 1);
    //        }
    //    }
    //    else if ((char)evt.data.keyboard.cNewInput0 == '\x1b')
    //    {
    //        SteamVR.instance.overlay.HideKeyboard();
    //    }
    //    else if (evt.data.keyboard.cNewInput0 < 128)
    //    {
    //        text += (char)evt.data.keyboard.cNewInput0;
    //    }

    //    if (evt.data.keyboard.cNewInput1 != 0 && evt.data.keyboard.cNewInput1 < 128)
    //    {
    //        text += (char)evt.data.keyboard.cNewInput1;
    //    }
    //    if (evt.data.keyboard.cNewInput2 != 0 && evt.data.keyboard.cNewInput2 < 128)
    //    {
    //        text += (char)evt.data.keyboard.cNewInput2;
    //    }
    //    if (evt.data.keyboard.cNewInput3 != 0 && evt.data.keyboard.cNewInput3 < 128)
    //    {
    //        text += (char)evt.data.keyboard.cNewInput3;
    //    }
    //    if (evt.data.keyboard.cNewInput4 != 0 && evt.data.keyboard.cNewInput4 < 128)
    //    {
    //        text += (char)evt.data.keyboard.cNewInput4;
    //    }
    //    if (evt.data.keyboard.cNewInput5 != 0 && evt.data.keyboard.cNewInput5 < 128)
    //    {
    //        text += (char)evt.data.keyboard.cNewInput5;
    //    }
    //    if (evt.data.keyboard.cNewInput6 != 0 && evt.data.keyboard.cNewInput6 < 128)
    //    {
    //        text += (char)evt.data.keyboard.cNewInput6;
    //    }
    //    if (evt.data.keyboard.cNewInput7 != 0 && evt.data.keyboard.cNewInput7 < 128)
    //    {
    //        text += (char)evt.data.keyboard.cNewInput7;
    //    }

    //    GetComponent<TextMesh>().text = text;
    //    GetComponent<TextScript>().textObject.text = text;
    //    GetComponent<TextScript>().UpdateTextByNetwork();

    //    //GetComponent<TextScript>().UpdateColor();
    //    UpdateTextBoxCollider();

    //    UpdateScaleBoxSize();
    //}

    public void UpdateTextBoxCollider()
    {
        Destroy(gameObject.GetComponent<BoxCollider>());
        gameObject.AddComponent<BoxCollider>();
    }

    public void UpdateScaleBoxSize()
    {
        if (transform.FindChild("ScaleHandler") != null)
        {
            Vector3 size = GetComponent<BoxCollider>().size;
            Vector3 center = GetComponent<BoxCollider>().center;
            transform.FindChild("ScaleHandler").transform.localScale = new Vector3(size.x / 10.0f, transform.FindChild("ScaleHandler").transform.localScale.y, size.y / 10.0f);
            transform.FindChild("ScaleHandler").localPosition = center;
        }   
    }

    private void EndTyping()
    {
        keyboard.GetComponent<KeyboardScript>().NewCharAdded -= GetChar;
        keyboard.GetComponent<KeyboardScript>().textBoxActive = false;
        KeyboardHandlerScript.CloseKeyBoard();
        SaveManager.SaveGameObject(gameObject);
    }

    public void UpdateColliderByNetwork()
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("UpdateColliderFromNetwork", PhotonTargets.Others);
        }

    }

    [PunRPC]
    public void UpdateColliderFromNetwork()
    {
        UpdateTextBoxCollider();
        UpdateScaleBoxSize();
    }




    #endregion
}
