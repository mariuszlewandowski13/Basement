#region Usings

using UnityEngine;
using System;
using System.IO;

#endregion

[Serializable]
public class ShapeObject : ColorableObject {

    #region Private Properties

    public string imgDirectoryPath = "Assets/Artwork/";
    public string imgName;
    
    private Texture2D texture;
    #endregion

    #region Constructors
    public ShapeObject(int width, int height, string imgName) : base(width, height, Color.black)
    {
        this.imgName = imgName;
        LoadShapeToTexture();
    }

    public ShapeObject(int width, int height, string imgName, string path) : base(width, height, Color.black)
    {
        this.imgDirectoryPath = path;
        this.imgName = imgName;
        LoadShapeToTexture();
    }

    public ShapeObject(int width, int height, string imgName, string path, Color32 color) : base(width, height, color)
    {
        this.imgDirectoryPath = path;
        this.imgName = imgName;
        LoadShapeToTexture();
    }

    public ShapeObject(ShapeObject shape) : base(shape.realWidth, shape.realHeight, shape.color)
    {
        this.imgDirectoryPath = shape.imgDirectoryPath;
        this.imgName = shape.imgName;
        CopyTexture(shape.GetShapeAsTexture());
        SetSavedTransform(shape.GetSavedPosition(), shape.GetSavedRotation(), shape.GetSavedScale());
        this.actualRatio = shape.actualRatio;
        PhotonViewID = shape.PhotonViewID;
        saved = shape.saved;
    }

    public ShapeObject(string [] row) : base(Int32.Parse(row[22]), Int32.Parse(row[23]), new Color(float.Parse(row[14]), float.Parse(row[15]), float.Parse(row[16]), float.Parse(row[17])))
    {
        this.imgDirectoryPath = ApplicationStaticData.shapesPath;
        this.imgName = row[2];
        LoadShapeToTexture();
        this.SetSavedTransform(new Vector3(float.Parse(row[4]), float.Parse(row[5]), float.Parse(row[6])), new Quaternion(float.Parse(row[7]), float.Parse(row[8]), float.Parse(row[9]), float.Parse(row[10])), new Vector3(float.Parse(row[11]), float.Parse(row[12]), float.Parse(row[13])));
        actualRatio = float.Parse(row[24]);
        PhotonViewID = Int32.Parse(row[25]);
        saved = true;
    }

    #endregion

    #region Methods

    private byte[] LoadShapeFromBytes()
    {
        return File.ReadAllBytes(imgDirectoryPath + imgName);
    }

    private void LoadShapeToTexture()
    {
        texture = new Texture2D(2, 2);
        texture.LoadImage(LoadShapeFromBytes());
    }

    public Texture2D GetShapeAsTexture()
    {
        if (texture == null) LoadShapeToTexture();
        return texture;
    }

    private void CopyTexture(Texture2D tex)
    {
        texture = tex;
    }

    public override string CreatSQLFromProperties()
    {
        return "null, 'ShapeObject', '" + imgName + "', '" + imgDirectoryPath + "', " + base.CreatSQLFromProperties() +", null, null, null, null, " + realWidth.ToString() + ", " + realHeight.ToString() + ", " + realRatio.ToString() + ", " + PhotonViewID.ToString() + ", 1";
    }

    public override string UpdateSQLProperties()
    {
        return base.UpdateSQLProperties();
    }

    #endregion

}
