using UnityEngine;
using System;


[Serializable]
public class SeparatorObject : ColorableObject {

    #region Public Properties

    public string prefabName;

    #endregion

    public SeparatorObject(string name) : base(1, 1, Color.white)
        {
        prefabName = name;
    }

    public SeparatorObject(string name, Color color) : base(1, 1, color)
    {
        prefabName = name;
    }

    public SeparatorObject(SeparatorObject sep) : base(1, 1, sep.color)
    {
        prefabName = sep.prefabName;
        SetSavedTransform(sep.GetSavedPosition(), sep.GetSavedRotation(), sep.GetSavedScale());
        PhotonViewID = sep.PhotonViewID;
        saved = sep.saved;
    }

    public SeparatorObject(string [] row) : base(1, 1, new Color(float.Parse(row[14]), float.Parse(row[15]), float.Parse(row[16]), float.Parse(row[17])))
    {
        prefabName = row[2];
        this.SetSavedTransform(new Vector3(float.Parse(row[4]), float.Parse(row[5]), float.Parse(row[6])), new Quaternion(float.Parse(row[7]), float.Parse(row[8]), float.Parse(row[9]), float.Parse(row[10])), new Vector3(float.Parse(row[11]), float.Parse(row[12]), float.Parse(row[13])));
        PhotonViewID = Int32.Parse(row[25]);
        saved = true;
    }

    public override string CreatSQLFromProperties()
    {
        return "null, 'SeparatorObject', '" + prefabName + "',null, " + base.CreatSQLFromProperties() + ", null, null, null, null, " + realWidth.ToString() + ", " + realHeight.ToString() + ", " + realRatio.ToString() + ", " + PhotonViewID.ToString() + ", 1";
    }

    public override string UpdateSQLProperties()
    {
        return base.UpdateSQLProperties();
    }
}
