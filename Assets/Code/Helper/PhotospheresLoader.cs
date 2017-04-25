#region Usings

using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

#endregion

#region PhotoSphereInfo Struct

public struct PhotoSpheresInfo
{
    public string sphereName;
};

#endregion

public class PhotoSpheresInfoLoader : FilesInfoLoader{

    #region Constructors
    public PhotoSpheresInfoLoader(string imagesPath) : base(imagesPath)
    {
    }

    public PhotoSpheresInfoLoader() : base()
    {
    }

    #endregion

    #region Methods

    public List<PhotoSpheresInfo> LoadPhotoSpheresInfo(string[] filesExtensions)
    {
        List<PhotoSpheresInfo> spheresInfoList = new List<PhotoSpheresInfo>();
        foreach (string extension in filesExtensions)
        {
            string[] sphNames = Directory.GetFiles(filesPath, "*" + extension);
            foreach (string imageName in sphNames)
            {
                PhotoSpheresInfo sphInfo;
                sphInfo.sphereName = Path.GetFileNameWithoutExtension(imageName);
                spheresInfoList.Add(sphInfo);
            }
        }

        return spheresInfoList;
    }
    #endregion



}
