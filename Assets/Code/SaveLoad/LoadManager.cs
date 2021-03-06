﻿#region Usings

using UnityEngine;
using System.IO;
using System;

#endregion

public static class LoadManager {
    #region Private Properties

    private static StreamReader readObject;
    private static ObjectSpawnerOnLoadGame objectSpawner;
    private static GameObject masterObj;

    #endregion

    public static void Load(string fileName, GameObject master)
    {
        try
        {
            readObject = new StreamReader(fileName);
            objectSpawner = new ObjectSpawnerOnLoadGame(master);

            string objectType;

            ClearEnviroment();

            while ((objectType = readObject.ReadLine()) != null)
            {
                try
                {
                    //LoadGameObject(objectType);
                }
                catch (Exception e)
                {
                    Debug.Log("Load error! ON: " + objectType);
                    Debug.Log(e.Message);
                }

                
            }
            readObject.Close();
            Debug.Log("Load succesful!");
        }
        catch (Exception e)
        {
            Debug.Log("Load unsuccesful!");
            Debug.Log(e.Message);

        }
        
    }

    public static void LoadFromServer(GameObject master)
    {
        objectSpawner = new ObjectSpawnerOnLoadGame(master);
        if (master.GetComponent<ServerSaveLoadScript>() != null && ApplicationStaticData.roomToConnectName != null)
        {
            masterObj = master;
            master.GetComponent<ServerSaveLoadScript>().LoadDb(LoadResultFromString, ApplicationStaticData.roomToConnectName);
        }
        else {
            Debug.Log("Missing ServerSaveLoadScript!!!");
        }

    }

    public static void LoadResultFromString(string[] row)
    {
        try
        {
            switch (row[1])
            {
                case "ImageObject":
                    LoadImage(row);
                    break;
                case "ShapeObject":
                    LoadShape(row);
                    break;
                case "PhotoSphere":
                    LoadPhotoSphere(row);
                    break;
                case "Shape3DObject":
                    LoadShape3DObject(row);
                    break;
                case "TextObject":
                    LoadTextObject(row);
                    break;
                case "SeparatorObject":
                    LoadSeparatorObject(row);
                    break;
                case "LineObject":
                    StartLoadLineObject(row);
                    break;
                default:
                   // Debug.Log("Bad option! ");
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.Log("Failed to Load " + row[1] + ", object ID " + row[0]);
            Debug.Log(e.Message);
        }
       

    }


    private static GameObject[] FindAllObjects()
    {
        return GameObject.FindGameObjectsWithTag(ObjectSpawner.tagName);
    }

    public static void ClearEnviroment()
    {
        GameObject[] objectsToDestroy = FindAllObjects();
        
        foreach (GameObject gameObject2 in objectsToDestroy)
        {
            GameObject.Destroy(gameObject2);

        }
        SaveLoadManager.isLoaded = false;
    }

    private static void LoadImage(string [] row)
    {
        //ImageObject imageObject = JsonUtility.FromJson<ImageObject>(jsonObject);

        ImageObject imageObject = new ImageObject(row);

        GameObject result =  objectSpawner.SpawnImage(imageObject);
        result.GetComponent<PhotonView>().viewID = imageObject.PhotonViewID;
        result.GetComponent<PhotonView>().instantiationId = imageObject.PhotonViewID;
        imageObject.saved = true;
    }


    private static void LoadShape(string [] row)
    {
        //ShapeObject shapeObject = JsonUtility.FromJson<ShapeObject>(jsonObject);
        ShapeObject shapeObject = new ShapeObject(row);
        GameObject result = objectSpawner.SpawnShape(shapeObject);
        result.GetComponent<PhotonView>().viewID = shapeObject.PhotonViewID;
        result.GetComponent<PhotonView>().instantiationId = shapeObject.PhotonViewID;
        shapeObject.saved = true;
    }

    private static void LoadPhotoSphere(string [] row)
    {
        //PhotoSphere photoSphereObject = JsonUtility.FromJson<PhotoSphere>(jsonObject);
        PhotoSphere photoSphereObject = new PhotoSphere(row);
        GameObject result = objectSpawner.SpawnPhotoSphere(photoSphereObject);
        result.GetComponent<PhotonView>().viewID = photoSphereObject.PhotonViewID;
        result.GetComponent<PhotonView>().instantiationId = photoSphereObject.PhotonViewID;
        photoSphereObject.saved = true;
    }


    private static void LoadShape3DObject(string [] row)
    {
        //Shape3DObject shape3DObject = JsonUtility.FromJson<Shape3DObject>(jsonObject);
        Shape3DObject shape3DObject = new Shape3DObject(row);
        GameObject result = objectSpawner.SpawnShape3DObject(shape3DObject);
        result.GetComponent<PhotonView>().viewID = shape3DObject.PhotonViewID;
        result.GetComponent<PhotonView>().instantiationId = shape3DObject.PhotonViewID;
        shape3DObject.saved = true;
    }

    private static void LoadTextObject(string []  row)
    {
        //TextObject textObject = JsonUtility.FromJson<TextObject>(jsonObject);
        TextObject textObject = new TextObject(row);
        GameObject result = objectSpawner.SpawnTextObject(textObject);
        result.GetComponent<PhotonView>().viewID = textObject.PhotonViewID;
        result.GetComponent<PhotonView>().instantiationId = textObject.PhotonViewID;
        textObject.saved = true;
    }

    private static void LoadSeparatorObject(string [] row)
    {
        //SeparatorObject separatorObject = JsonUtility.FromJson<SeparatorObject>(jsonObject);
        SeparatorObject separatorObject = new SeparatorObject(row);

        GameObject result = objectSpawner.SpawnSeparatorObject(separatorObject);
        if (result != null)
        {
            result.GetComponent<PhotonView>().viewID = separatorObject.PhotonViewID;
            result.GetComponent<PhotonView>().instantiationId = separatorObject.PhotonViewID;
            separatorObject.saved = true;
        }
       
    }

    private static void StartLoadLineObject(string [] row)
    {
        LineObject lineObject = new LineObject(row);
        masterObj.GetComponent<ServerSaveLoadScript>().LoadLinesFile(lineObject, row[21], EndLoadLineObject);
    }

    private static void EndLoadLineObject(LineObject lineObject)
    {
        GameObject result = objectSpawner.SpawnLineObject(lineObject);
        if (PhotonView.Find(lineObject.PhotonViewID) == null)
        {
            result.GetComponent<PhotonView>().viewID = lineObject.PhotonViewID;
            result.GetComponent<PhotonView>().instantiationId = lineObject.PhotonViewID;
            lineObject.saved = true;
        }
        else {
            GameObject.Destroy(result);
        }  
    }
}
