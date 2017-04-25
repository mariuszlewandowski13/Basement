#region Usings
using UnityEngine;
using System;
#endregion

/*
    Shape3DObject class represents a 3D objects like cubes, cones etc.
    For built-in 3D object we only have to know its index (from the table in WorldObjects)

*/
[Serializable]
public class Shape3DObject : ColorableObject {

    #region Public Properties

    public int shape3DObjectNumber;//3D object number in the table in WorldObjects script

    #endregion

    #region Constructors

    public Shape3DObject(int number): base(1, 1, Color.white)
    {
         shape3DObjectNumber = number;
    }

    public Shape3DObject(int number, Color color) : base(1, 1, color)
    {
        shape3DObjectNumber = number;
    }

    public Shape3DObject(Shape3DObject shape) : base(1, 1, shape.color)
    {
        shape3DObjectNumber = shape.shape3DObjectNumber;
        SetSavedTransform(shape.GetSavedPosition(), shape.GetSavedRotation(), shape.GetSavedScale());
        actualRatio = shape.actualRatio;
        PhotonViewID = shape.PhotonViewID;
        saved = shape.saved;
    }

    /*
        Constructor used when we load 3DShapeObject from Databse
    */
    public Shape3DObject(string [] row) : base(1, 1, new Color(float.Parse(row[14]), float.Parse(row[15]), float.Parse(row[16]), float.Parse(row[17])))
    {
        shape3DObjectNumber = Int32.Parse(row[18]);
        SetSavedTransform(new Vector3(float.Parse(row[4]), float.Parse(row[5]), float.Parse(row[6])), new Quaternion(float.Parse(row[7]), float.Parse(row[8]), float.Parse(row[9]), float.Parse(row[10])), new Vector3(float.Parse(row[11]), float.Parse(row[12]), float.Parse(row[13])));
        actualRatio = float.Parse(row[24]);
        PhotonViewID = Int32.Parse(row[25]);
        saved = true;
    }
    #endregion

    #region Methods
    public override string CreatSQLFromProperties()
    {
        return "null ,'Shape3DObject', null, null,  " + base.CreatSQLFromProperties() + ", " + shape3DObjectNumber.ToString() + ", null, null, null, " + realWidth.ToString() + ", " + realHeight.ToString() + ", " + realRatio.ToString() + ", " + PhotonViewID.ToString() + ", 1";
    }
    public override string UpdateSQLProperties()
    {
        return base.UpdateSQLProperties();
    }

    #endregion
}
