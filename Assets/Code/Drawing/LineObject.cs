#region Usings

using UnityEngine;
using System;

#endregion

#region Point Class
[Serializable]
public class Points
{
    #region Public Properties

    public Vector3[] points;

    public int Length
    {
        get {
            return points.Length;
        }
    }

    #endregion

    #region Constructors
    public Points(Vector3[] points)
    {
        this.points = points;
    }

    public Points()
    {

    }

    #endregion
}

#endregion


[Serializable]
public class LineObject : ColorableObject {

    #region Public Properites

    public int textureNumber;
    public float  tickness;
    public Points points;
    public string pointsFileName = "";

    #endregion

    #region Constructors

    public LineObject(int texNum = 0) : base(1,1, Color.white)
    {
        textureNumber = texNum;
        points = new Points();
    }

    public LineObject(LineObject line) : base(1, 1, line.color)
    {
        this.textureNumber = line.textureNumber;
        this.tickness = line.tickness;
        this.points = line.points;
        SetSavedTransform(line.GetSavedPosition(), line.GetSavedRotation(), line.GetSavedScale());
        pointsFileName = line.pointsFileName;

        this.actualRatio = line.actualRatio;
        PhotonViewID = line.PhotonViewID;
        saved = line.saved;
    }

    public LineObject(string [] row) : base(1, 1, new Color(float.Parse(row[14]), float.Parse(row[15]), float.Parse(row[16]), float.Parse(row[17])))
    {
        this.textureNumber = Int32.Parse(row[18]);
        this.tickness = float.Parse(row[20]);
        this.SetSavedTransform(new Vector3(float.Parse(row[4]), float.Parse(row[5]), float.Parse(row[6])), new Quaternion(float.Parse(row[7]), float.Parse(row[8]), float.Parse(row[9]), float.Parse(row[10])), new Vector3(float.Parse(row[11]), float.Parse(row[12]), float.Parse(row[13])));
        actualRatio = float.Parse(row[24]);
        PhotonViewID = Int32.Parse(row[25]);
        pointsFileName = row[21];
        saved = true;
    }

    #endregion

    #region Methods

    public override string CreatSQLFromProperties()
    {
        return "null, 'LineObject', null,  null , " + base.CreatSQLFromProperties() + ", " + textureNumber.ToString() + ", null, " + tickness.ToString() + ", '"+ pointsFileName+  "'," + realWidth.ToString() + ", " + realHeight.ToString() + ", " + realRatio.ToString() + ", " + PhotonViewID.ToString() + ", 1";
    }

    public override string UpdateSQLProperties()
    {
        return base.UpdateSQLProperties() + ", TICKNESS = " + tickness;
    }

    public void SetPointsFromJSON(string json)
    {
        // Debug.Log(json);
        try
        {
            this.points = JsonUtility.FromJson<Points>(json);

        }
        catch (Exception) {
          //  Debug.Log(e.Message);
           // Debug.Log(json);
        }
        
    }

    #endregion

}
