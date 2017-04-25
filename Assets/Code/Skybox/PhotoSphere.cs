#region Usings

using System;
using UnityEngine;
using System.IO;

#endregion

[Serializable]
public class PhotoSphere : SphereObject {
    #region private Properties

    private static string materialPath = "Materials/PhotoSphere/";

    #endregion

    #region Public Properties

    public string materialName;

    #endregion


    #region Constructors

    public PhotoSphere(float scaleX, float scaleY, float scaleZ, string materialName) : base(scaleX, scaleY, scaleZ)
    {
        this.materialName = materialName;
    }

    public PhotoSphere(string materialName) : base(0.15f, 0.15f, 0.15f)
    {
        this.materialName = materialName;
    }

    public PhotoSphere(PhotoSphere sphere) : base(0.15f, 0.15f, 0.15f)
    {
        this.materialName = sphere.materialName;
        SetSavedTransform(sphere.GetSavedPosition(), sphere.GetSavedRotation(), sphere.GetSavedScale());
        PhotonViewID = sphere.PhotonViewID;
        saved = sphere.saved;
    }

    public PhotoSphere(string [] row) : base(0.15f, 0.15f, 0.15f)
    {
        this.materialName = row[2];
        this.SetSavedTransform(new Vector3(float.Parse(row[4]), float.Parse(row[5]), float.Parse(row[6])), new Quaternion(float.Parse(row[7]), float.Parse(row[8]), float.Parse(row[9]), float.Parse(row[10])), new Vector3(float.Parse(row[11]), float.Parse(row[12]), float.Parse(row[13])));
        PhotonViewID = Int32.Parse(row[25]);
        saved = true;
    }

    #endregion

    #region Methods
    public string GetMaterialFullPath()
    {
        return materialPath + materialName;
    }

    public override string CreatSQLFromProperties()
    {
        return "null, 'PhotoSphere', '" + materialName + "', '" + materialPath + "', " + base.CreatSQLFromProperties()+ ", null, null, null, null, null, null, null, null, " + realWidth.ToString() + ", " + realHeight.ToString() + ", " + realRatio.ToString() + ", " + PhotonViewID.ToString()+ ", 1";
    }

    public override string UpdateSQLProperties()
    {
        return base.UpdateSQLProperties();
    }

    #endregion

}
