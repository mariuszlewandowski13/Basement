#region Usings

using UnityEngine;

#endregion


public class TextScript : MonoBehaviour, IResizable {

    #region Public Properties

    public TextObject textObject;
    public bool isSet = false;

    #endregion


    #region Methods

    private void SetComponentText()
    {
        GetComponent<TextMesh>().text = textObject.text;
    }

    public void SetTextObject(TextObject textObj)
    {
        isSet = true;
        this.textObject = new TextObject(textObj);
        SetComponentText();
        UpdateColor(textObj.color);

        if (GetComponent<TypingScript>() != null)
        {
            GetComponent<TypingScript>().UpdateTextBoxCollider();
            GetComponent<TypingScript>().UpdateScaleBoxSize();
        }
    }

    public void SetColor(Color color)
    {
        UpdateColor(color);
        UpdateColorByNetwork();
    }

    private void UpdateColor(Color color)
    {
        textObject.color = color;
        GetComponent<TextMesh>().color = textObject.color;
        GetComponent<Renderer>().material.color = textObject.color;
       
    }

    public void LoadFromTextObject()
    {
        if (textObject != null)
        {
            transform.position = textObject.GetSavedPosition();
            transform.rotation = textObject.GetSavedRotation();
            transform.localScale = textObject.GetSavedScale();
        }



    }

    public void SetTextByNetwork()
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("CreateAndSetTextObject", PhotonTargets.Others, textObject.text, textObject.prefabName);
        }
        
        UpdateColorByNetwork();
    }

    public void UpdateTextByNetwork()
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("SetText", PhotonTargets.Others, textObject.text);
        }
        
    }
    public void UpdateColorByNetwork()
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("UpdateColorFromNetwork", PhotonTargets.Others, textObject.color.r, textObject.color.g, textObject.color.b, textObject.color.a);
        }
        
    }

    [PunRPC]
    public void SetText(string text)
    {
        textObject.text = text;
        SetComponentText();
    }

    [PunRPC]
    public void UpdateColorFromNetwork(float r, float g, float b, float a)
    {
        Color color = new Color(r, g, b, a);

        UpdateColor(color);

    }
    [PunRPC]
    public void CreateAndSetTextObject(string text, string name)
    {
        SetTextObject(new TextObject(text, Color.white, name));
    }


    public string GetResizableObjectPrefabName()
    {
        return textObject.prefabName;
    }

    public void SetModyficableObject(ModyficableObject appearanceObject)
    {
        SetTextObject((TextObject)appearanceObject);
    }

    public ModyficableObject GetModyficableObject()
    {
        return textObject;
    }


    public void SetMaterial(Material mat)
    {

    }

    public void UpdateActualRatio(float width, float height)
    {
        textObject.UpdateActualRatio(width, height);
    }

    public void SetModyficableObjectByNetwork()
    {
        SetTextByNetwork();
    }

    #endregion
}
