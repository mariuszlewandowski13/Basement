#region Usings
using System.Collections.Generic;
using System;
using UnityEngine;
#endregion

public static class ApplicationStaticData {

    #region Public Properties

    static private string _roomToConnectName = "";
    static public string roomToConnectName
    {
        get {
            return _roomToConnectName;
        }

        set {
            if (_roomToConnectName == worldRoomName)
            {
                SavePlayerPosition();
            }

            _roomToConnectName = value;
        }
    }

    static public string userName = "";
    static public string userID = "";
    static public string userRoom = "";
    static public string userScene = "";
    static public string firstPage = "http://www.google.com";
    static public string creatorsPage = "http://www.basement-vr.com/";


    static public string worldRoomName = "default3";
    static public string worldSceneName = "CommonRoom";
    static public Vector3 worldPosition = new Vector3();

    static public Color objectsHighligthing = Color.red;
    static public Color toolsHighligthing = Color.red;
    static public Color doorsHighligthing = Color.red;

    public static string serverScriptsPath = "http://vrowser.e-kei.pl/BasementVR/scripts/";


#if UNITY_EDITOR

    static public string imagesPath = "C:/files/";
    static public string tagsPath = "C:/files/";
    static public string shapesPath = "C:/files/Shapes/";
    static public string gridsPath = "C:/files/Shapes/";
    static public string photoSpheresPath = "C:/files/PhotoSphere/";
    static public string userImagesPath = "C:/files/userImages/";
    static public string screenshotsPath = "C:/screenshots/";

#elif UNITY_STANDALONE
    static public string imagesPath = Application.dataPath + "/media/appData/images/";
    static public string tagsPath = Application.dataPath + "/media/appData/images/";
    static public string shapesPath = Application.dataPath + "/media/appData/shapes/";
    static public string gridsPath = Application.dataPath + "/media/appData/shapes/";
    static public string photoSpheresPath = Application.dataPath + "/media/appData/sfery/";
    static public string userImagesPath = Application.dataPath + "/media/upload/";
    static public string screenshotsPath = Application.dataPath + "/media/screenshots/";
#endif

    static public bool adminMode = true;
    #endregion

    #region Private Properties

    private static List<ImagesInfo> imagesInfo;
    private static List<ImagesInfo> tagsInfo;
    private static List<ImagesInfo> shapesInfo;
    private static List<ImagesInfo> gridsInfo;
    private static List<PhotoSpheresInfo> photoSpheresInfo;
    private static List<ImagesInfo> userImagesInfo;

    private static ImageObject[] imageObjects;
    private static ImageObject[] tagsObjects;
    private static ShapeObject[] shapeObjects;
    private static ShapeObject[] gridObjects;
    private static PhotoSphere[] photoSphereObjects;
    private static ImageObject[] userImageObjects;

    private static Color avatarColor;

    #endregion

    #region Static Methods
    public static List<ImagesInfo> GetImagesInfo()
    {
        if (imagesInfo == null)
        {
            LoadImagesInfo();
        }

        return imagesInfo;
    }

    public static List<ImagesInfo> GetUserImagesInfo()
    {
        if (userImagesInfo == null)
        {
            LoadUserImagesInfo();
        }

        return userImagesInfo;
    }

    public static List<ImagesInfo> GetTagsInfo()
    {
        if (tagsInfo == null)
        {
            LoadTagsInfo();
        }

        return tagsInfo;
    }

    public static List<ImagesInfo> GetShapesInfo()
    {
        if (shapesInfo == null)
        {
            LoadShapesInfo();
        }

        return shapesInfo;
    }

    public static List<ImagesInfo> GetGridsInfo()
    {
        if (gridsInfo == null)
        {
            LoadGridsInfo();
        }

        return gridsInfo;
    }

    public static List<PhotoSpheresInfo> GetPhotoSpheresInfo()
    {
        if (photoSpheresInfo == null)
        {
            LoadPhotoSpheresInfo();
        }

        return photoSpheresInfo;
    }

    public static void LoadAllData()
    {
        LoadImagesInfo();
        LoadShapesInfo();
        LoadTagsInfo();
        LoadPhotoSpheresInfo();

        LoadImages();
        LoadTags();
        LoadShapes();
        LoadPhotoSpheres();
    }

    private static void LoadImagesInfo()
    {
        if (imagesInfo == null)
        {
            try
            {
                //ImageFilesInfoLoader loader = new ImageFilesInfoLoader(Application.dataPath + "/media/images/");
                ImageFilesInfoLoader loader = new ImageFilesInfoLoader(imagesPath);
                string[] extensions = { ".jpg", ".bmp", ".jpeg", ".png" };
                imagesInfo = loader.LoadImagesInfo(extensions);
            }
            catch (Exception e)
            {
                Debug.Log("Unable to load images!");
                Debug.Log(e);
            }
        }
        
    }
    private static void LoadUserImagesInfo()
    {
        if (userImagesInfo == null)
        {
            try
            {
                //ImageFilesInfoLoader loader = new ImageFilesInfoLoader(Application.dataPath + "/media/images/");
                ImageFilesInfoLoader loader = new ImageFilesInfoLoader(userImagesPath);
                string[] extensions = { ".jpg", ".bmp", ".jpeg", ".png" };
                userImagesInfo = loader.LoadImagesInfo(extensions, "", 50, 2000000);
            }
            catch (Exception e)
            {
                Debug.Log("Unable to load user images!");
                Debug.Log(e);
            }
        }

    }

    private static void LoadTagsInfo()
    {
        if (tagsInfo == null)
        {
            try
            {
                //ImageFilesInfoLoader loader = new ImageFilesInfoLoader(Application.dataPath + "/media/images/");
                ImageFilesInfoLoader loader = new ImageFilesInfoLoader(tagsPath);
                string[] extensions = {".png" };
                tagsInfo = loader.LoadImagesInfo(extensions, "tags/");
            }
            catch (Exception e)
            {
                Debug.Log("Unable to load tags!");
                Debug.Log(e);
            }
        }

    }

    private static void LoadShapesInfo()
    {
        if (shapesInfo == null)
        {
            try
            {
                ImageFilesInfoLoader loader = new ImageFilesInfoLoader(shapesPath);
                //ImageFilesInfoLoader loader = new ImageFilesInfoLoader(Application.dataPath + "/media/Shapes/");

                string[] extensions = { ".png" };
                shapesInfo = loader.LoadImagesInfo(extensions);
            }
            catch (Exception e)
            {
                Debug.Log("Unable to load shapes!");
                Debug.Log(e);
            }
        }
        
    }

    private static void LoadGridsInfo()
    {
        if (gridsInfo == null)
        {
            try
            {
                ImageFilesInfoLoader loader = new ImageFilesInfoLoader(gridsPath);
                //ImageFilesInfoLoader loader = new ImageFilesInfoLoader(Application.dataPath + "/media/");

                string[] extensions = { ".png" };
                gridsInfo = loader.LoadImagesInfo(extensions, "grids/");
            }
            catch (Exception e)
            {
                Debug.Log("Unable to load grids!");
                Debug.Log(e);
            }
        }
    }

    private static void LoadPhotoSpheresInfo()
    {
        if (photoSpheresInfo == null)
        {
            try
            {
                PhotoSpheresInfoLoader loader = new PhotoSpheresInfoLoader(photoSpheresPath);
                //PhotoSpheresInfoLoader loader = new PhotoSpheresInfoLoader(Application.dataPath + "/media/photospheres/");

                string[] extensions = { ".mat" };
                photoSpheresInfo = loader.LoadPhotoSpheresInfo(extensions);
            }
            catch (Exception e)
            {
                Debug.Log("Unable to load photospheres!");
                Debug.Log(e);
            }
        }
        
    }

    public static ImageObject[] GetImages()
    {
        LoadImages();
        ImageObject [] z = new ImageObject[imageObjects.Length + userImageObjects.Length];
        imageObjects.CopyTo(z, 0);
        userImageObjects.CopyTo(z, imageObjects.Length);
        return z;
    }

    public static ImageObject[] GetTags()
    {
        LoadTags();
        return tagsObjects;
    }

    public static ShapeObject[] GetShapes()
    {
       LoadShapes();
        return shapeObjects;
    }

    public static ShapeObject[] GetGrids()
    {
        LoadGrids();
        return gridObjects;
    }

    public static PhotoSphere[] GetPhotoSpheres()
    {
        LoadPhotoSpheres();
        return photoSphereObjects;
    }

    private static void LoadImages()
    {
        if (imageObjects == null)
        {
            if (imagesInfo != null)
            {
                imageObjects = new ImageObject[imagesInfo.Count];
                int index = 0;
                foreach (ImagesInfo img in imagesInfo)
                {
                    imageObjects[index] = new ImageObject(img.width, img.height, img.name, img.path);
                    index++;
                }
            }
        }
    }

    public static void LoadUserImagesFromServer(List<ImagesInfo> serverList)
    {
        if (userImageObjects == null)
        {
            if (serverList != null)
            {
                userImageObjects = new ImageObject[serverList.Count];
                int index = 0;
                foreach (ImagesInfo img in serverList)
                {
                    userImageObjects[index] = new ImageObject(img.width, img.height, img.path+"/"+ img.name, "", LoadingType.remote, img.path+ "/miniatures/" + img.name, false);
                    index++;
                }
            }
        }
    }

    private static void LoadTags()
    {
        if (tagsObjects == null)
        {
            if (tagsInfo != null)
            {
                tagsObjects = new ImageObject[tagsInfo.Count];
                int index = 0;
                foreach (ImagesInfo img in tagsInfo)
                {
                    tagsObjects[index] = new ImageObject(img.width, img.height, img.name, img.path);
                    index++;
                }
            }
        }
    }

    private static void LoadShapes()
    {
        if (shapeObjects == null)
        {
            if (shapesInfo != null)
            {
                shapeObjects = new ShapeObject[shapesInfo.Count];
                int index = 0;
                Color color = Color.white;
                foreach (ImagesInfo shape in shapesInfo)
                {
                    shapeObjects[index] = new ShapeObject(shape.width, shape.height, shape.name, shape.path, color);
                    index++;
                }
            }
        }   
    }

    private static void LoadGrids()
    {
        if (gridObjects == null)
        {
            if (gridsInfo != null)
            {
                gridObjects = new ShapeObject[gridsInfo.Count];
                int index = 0;
                foreach (ImagesInfo shape in gridsInfo)
                {
                    gridObjects[index] = new ShapeObject(shape.width, shape.height, shape.name, shape.path);
                    index++;
                }
            }
        }
        
    }

    private static void LoadPhotoSpheres()
    {
        if (photoSphereObjects == null)
        {
            if (photoSpheresInfo != null)
            {
                photoSphereObjects = new PhotoSphere[photoSpheresInfo.Count];
                int index = 0;
                foreach (PhotoSpheresInfo sphere in photoSpheresInfo)
                {
                    photoSphereObjects[index] = new PhotoSphere(sphere.sphereName);
                    index++;
                }
            }
        }
    }

    public static Color GetAvatarColor()
    {
        if (avatarColor == new Color(0.0f, 0.0f, 0.0f, 0.0f))
        {
            avatarColor = HelperFunctionsScript.GetRandomColor();
        }

        return avatarColor;
    }

    private static void SavePlayerPosition()
    {
        worldPosition = GameObject.Find("Player").transform.position;
    }

    public static bool IsRoomMine()
    {
        if (userRoom == roomToConnectName) return true;
        else return false;
    }

    public static bool IsRoomMainHall()
    {
        if (worldRoomName == roomToConnectName) return true;
        else return false;
    }

    public static bool CanInteract()
    {
        return IsRoomMainHall() || IsRoomMine() || adminMode;
    }

    #endregion
}
