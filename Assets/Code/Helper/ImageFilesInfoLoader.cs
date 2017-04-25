#region Usings

using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System;

#endregion

#region ImagesInfo Struct
public struct ImagesInfo
{
    public string name;
    public int width;
    public int height;
    public string path;
    public string extension;

    public static bool operator ==(ImagesInfo img1, ImagesInfo img2)
    {
        return img1.name == img2.name;
    }

    public static bool operator !=(ImagesInfo img1, ImagesInfo img2)
    {
        return img1.name != img2.name;
    }
};

#endregion

public class ImageFilesInfoLoader : FilesInfoLoader {


    #region Constructors
    public ImageFilesInfoLoader(string imagesPath) : base(imagesPath)
    {
    }

    public ImageFilesInfoLoader() : base()
    {
    }

    #endregion

    #region Methods

    public List<ImagesInfo> LoadImagesInfo(string [] filesExtensions, string additionalPath = "", int counterLimit = -1, long sizeLimit = -1)
    {
        List<ImagesInfo> imagesInfoList = new List<ImagesInfo>();

        foreach (string extension in filesExtensions)
        {
            string [] imagesNames = Directory.GetFiles(filesPath+additionalPath, "*" + extension);
            
            foreach (string imageName in imagesNames)
            {
                try {
                    Image img = Image.FromFile(imageName);
                    long fileLength = new FileInfo(imageName).Length;

                    if ((sizeLimit > 0 && fileLength < sizeLimit) || sizeLimit == -1)
                    {
                        //if (sizeLimit > 0)
                        //{
                        //    Debug.Log(fileLength);
                        //}
                        ImagesInfo imgInfo;
                        imgInfo.name = additionalPath + Path.GetFileName(imageName);
                        imgInfo.width = img.Width;
                        imgInfo.height = img.Height;
                        imgInfo.path = filesPath;
                        imgInfo.extension = extension;
                        imagesInfoList.Add(imgInfo);
                    }

                   
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to load: " + imageName);
                    Debug.Log(e);
                }
                if (counterLimit > 0 && imagesInfoList.Count >= counterLimit) break;
            }
            if (counterLimit > 0 && imagesInfoList.Count >= counterLimit) break;
        }

        return imagesInfoList;
    }
    #endregion
}
