#region Usings

using System;
using UnityEngine;

#endregion

[Serializable]
public class TextObject : ColorableObject {

    #region Public Properties

    public string text;
    public string prefabName;

    #endregion

    #region Constructors

    public TextObject(int width, int height, string text, Color color, string name) : base(width, height, color)
    {
        this.text = text;
        prefabName = name;    
    }

    public TextObject(TextObject textObject) : base(textObject.realWidth, textObject.realHeight, textObject.color)
    {
        this.text = textObject.text;
        this.prefabName = textObject.prefabName;
        SetSavedTransform(textObject.GetSavedPosition(), textObject.GetSavedRotation(), textObject.GetSavedScale());
        PhotonViewID = textObject.PhotonViewID;
        saved = textObject.saved;
    }

    public TextObject(string [] row) : base(Int32.Parse(row[22]), Int32.Parse(row[23]), new Color(float.Parse(row[14]), float.Parse(row[15]), float.Parse(row[16]), float.Parse(row[17])))
    {
        this.text = row[19];
        this.SetSavedTransform(new Vector3(float.Parse(row[4]), float.Parse(row[5]), float.Parse(row[6])), new Quaternion(float.Parse(row[7]), float.Parse(row[8]), float.Parse(row[9]), float.Parse(row[10])), new Vector3(float.Parse(row[11]), float.Parse(row[12]), float.Parse(row[13])));
        this.prefabName = row[2];
        PhotonViewID = Int32.Parse(row[25]);
        saved = true;
    }

    public TextObject(string text, Color color, string name, int PhotonId = 0) : base(1, 1, color)
    {
        this.text = text;
        this.prefabName = name;
        PhotonViewID = PhotonId;

    }

    public TextObject() : base(1, 1, Color.white)
    {
        this.text = "";
    }

    public override string CreatSQLFromProperties()
    {
        return "null, 'TextObject', '" + prefabName + "',null, " + base.CreatSQLFromProperties() + ", null, '" + text + "', null, null, " + realWidth.ToString() + ", " + realHeight.ToString() + ", " + realRatio.ToString() + ", " + PhotonViewID.ToString() + ", 1";
    }

    public override string UpdateSQLProperties()
    {
        return base.UpdateSQLProperties() + ", TEXT = '" +text+ "'";
    }

    #endregion



}
