#region Usings

using UnityEngine;
using System;
using System.IO;

#endregion


#region LoadingType enum
public enum LoadingType
{
    local = 1,
    remote = 2
};

#endregion


[Serializable]
public class ImageObject : VisualObject{

    #region Private Properties

    public string imgDirectoryPath = "Assets/Artwork/";
    public string imgName;
    public string tumbnailName = "";
    public Texture2D texture;
    public LoadingType loadType;

    #endregion

    #region Constructors
    public ImageObject(int width, int height, string imgName) : base(width, height)
    {
        this.imgName = imgName;
    }

    public ImageObject(int width, int height, string imgName, string path, LoadingType loadType = LoadingType.local, string tumbnailName = "", bool saved = false) : base(width, height)
    {
        this.imgDirectoryPath = path;
        this.loadType = loadType;
        this.imgName = imgName;
        this.tumbnailName = tumbnailName;
        this.saved = saved;
    }

    public ImageObject(ImageObject img) : base(img.realWidth, img.realHeight)
    {
        this.imgDirectoryPath = img.imgDirectoryPath;
        this.imgName = img.imgName;
        loadType = img.loadType;
        tumbnailName = img.tumbnailName;
        CopyTexture(img.texture);

        this.SetSavedTransform(img.GetSavedPosition(), img.GetSavedRotation(), img.GetSavedScale());
        this.actualRatio = img.actualRatio;
        PhotonViewID = img.PhotonViewID;
        saved = img.saved;

    }

    public ImageObject(string[] row) : base(Int32.Parse(row[22]), Int32.Parse(row[23])) //loading From SQL
    {
        this.imgDirectoryPath = ApplicationStaticData.imagesPath;
        this.loadType = (LoadingType)Int32.Parse(row[26]);
        this.imgName = row[2];
        this.SetSavedTransform(new Vector3(float.Parse(row[4]), float.Parse(row[5]), float.Parse(row[6])), new Quaternion(float.Parse(row[7]), float.Parse(row[8]), float.Parse(row[9]), float.Parse(row[10])), new Vector3(float.Parse(row[11]), float.Parse(row[12]), float.Parse(row[13])));
        actualRatio = float.Parse(row[24]);
        PhotonViewID = Int32.Parse(row[25]);
        saved = true;
    }

    #endregion

    #region Methods

    public byte[] LoadImgFromBytes()
    {
        byte[] bytes= null;
        try
        {
            bytes = File.ReadAllBytes(imgDirectoryPath + imgName);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    return bytes;   
    }

    public override string CreatSQLFromProperties()
    {
        return "null, 'ImageObject', '" + imgName + "', '" + imgDirectoryPath + "', " +  base.CreatSQLFromProperties() + ", null, null, null, null, null, null, null, null, " + realWidth.ToString() + ", " + realHeight.ToString() + ", " + realRatio.ToString() + ", " + PhotonViewID.ToString() + ", " + ((int)loadType).ToString();
    }

    public override string UpdateSQLProperties()
    {
        return base.UpdateSQLProperties() + ", COLOR_R= null, COLOR_G = null, COLOR_B = null, COLOR_A = null";
    }

    private void CopyTexture(Texture2D tex)
    {
        texture = tex;
    }

    public string GetMiniatureTextureName()
    {
        if (tumbnailName == "") return imgName;
        else return tumbnailName;
    }

    #endregion
}
