#region Usings

using UnityEngine;
using System.Collections;
using System.IO;
using System;

#endregion

public static class SaveManager {
    #region Private Properties

    private static GameObject masterObj;

    #endregion


    public static void Save(GameObject master)
    {
        masterObj = master;
        //try
        //{
            //writeObject = File.CreateText(fileName);
            GameObject[] objectsToSave = FindAllObjects();

            foreach (GameObject gameObject in objectsToSave)
            {
               // Debug.Log(gameObject.name);
                SaveGameObject(gameObject);
            }

           // writeObject.Close();
            Debug.Log("Save succesful!");     
    }

    private static GameObject[] FindAllObjects()
    {
        return GameObject.FindGameObjectsWithTag(ObjectSpawner.tagName);
    }

    public static void SaveGameObject(GameObject gameObject)
    {

            if (masterObj == null) masterObj = GameObject.Find("Player");

            if (gameObject.GetComponent<ImageScript>() != null)
            {
                SaveImage(gameObject);
            }
            else if (gameObject.GetComponent<ShapeScript>() != null)
            {
                SaveShape(gameObject);
            }
            else if (gameObject.GetComponent<PhotoSphereScript>() != null)
            {
                SavePhotoSphere(gameObject);
            }
            else if (gameObject.GetComponent<Shape3DScript>() != null)
            {
                SaveShape3DObject(gameObject);
            }
            else if (gameObject.GetComponent<TextScript>() != null)
            {
                SaveTextObject(gameObject);
            }
            else if (gameObject.GetComponent<SeparatorScript>() != null)
            {
                SaveSeparatorObject(gameObject);
            }
            else if (gameObject.GetComponent<LineScript>() != null)
            {
                SaveLineObject(gameObject);
            }
            else if (gameObject.GetComponent<TextGroupScript>() != null)
            {
                foreach (Transform child in gameObject.transform)
                {
                    SaveGameObject(child.gameObject);
                }
            }
        
        
    }

    private static void SaveImage(GameObject gameObject)
    {
        gameObject.GetComponent<ImageScript>().imageObject.SaveActualTransformation(gameObject.transform);
        gameObject.GetComponent<ImageScript>().imageObject.PhotonViewID = gameObject.GetComponent<PhotonView>().viewID;

        if (!gameObject.GetComponent<ImageScript>().imageObject.saved)
        {
            string sql = gameObject.GetComponent<ImageScript>().imageObject.CreatSQLFromProperties();
            masterObj.GetComponent<ServerSaveLoadScript>().InsertIntoDatabase(ApplicationStaticData.roomToConnectName, sql);
            gameObject.GetComponent<ImageScript>().imageObject.saved = true;
        }
        else {
            string sql = gameObject.GetComponent<ImageScript>().imageObject.UpdateSQLProperties();
            masterObj.GetComponent<ServerSaveLoadScript>().UpdateInDatabase(ApplicationStaticData.roomToConnectName, sql, gameObject.GetComponent<ImageScript>().imageObject.PhotonViewID);
        }
    }

    private static void SaveShape(GameObject gameObject)
    {
        gameObject.GetComponent<ShapeScript>().shapeObject.SaveActualTransformation(gameObject.transform);
        gameObject.GetComponent<ShapeScript>().shapeObject.PhotonViewID = gameObject.GetComponent<PhotonView>().viewID;

        if (!gameObject.GetComponent<ShapeScript>().shapeObject.saved)
        {
            string sql = gameObject.GetComponent<ShapeScript>().shapeObject.CreatSQLFromProperties();
            masterObj.GetComponent<ServerSaveLoadScript>().InsertIntoDatabase(ApplicationStaticData.roomToConnectName, sql);
            gameObject.GetComponent<ShapeScript>().shapeObject.saved = true;
        }
        else {
            string sql = gameObject.GetComponent<ShapeScript>().shapeObject.UpdateSQLProperties();
            masterObj.GetComponent<ServerSaveLoadScript>().UpdateInDatabase(ApplicationStaticData.roomToConnectName, sql, gameObject.GetComponent<ShapeScript>().shapeObject.PhotonViewID);
        }

        // writeObject.WriteLine(jsonString);
    }

    private static void SaveShape3DObject(GameObject gameObject)
    {
        gameObject.GetComponent<Shape3DScript>().shapeObject.SaveActualTransformation(gameObject.transform);
        gameObject.GetComponent<Shape3DScript>().shapeObject.PhotonViewID = gameObject.GetComponent<PhotonView>().viewID;


        if (!gameObject.GetComponent<Shape3DScript>().shapeObject.saved)
        {
            string sql = gameObject.GetComponent<Shape3DScript>().shapeObject.CreatSQLFromProperties();
            masterObj.GetComponent<ServerSaveLoadScript>().InsertIntoDatabase(ApplicationStaticData.roomToConnectName, sql);
            gameObject.GetComponent<Shape3DScript>().shapeObject.saved = true;
        }
        else {
            string sql = gameObject.GetComponent<Shape3DScript>().shapeObject.UpdateSQLProperties();
            masterObj.GetComponent<ServerSaveLoadScript>().UpdateInDatabase(ApplicationStaticData.roomToConnectName, sql, gameObject.GetComponent<Shape3DScript>().shapeObject.PhotonViewID);
        }
    }

    private static void SaveSeparatorObject(GameObject gameObject)
    {
        gameObject.GetComponent<SeparatorScript>().separatorObject.SaveActualTransformation(gameObject.transform);
        gameObject.GetComponent<SeparatorScript>().UpdateColorInSeparatorObject();
        gameObject.GetComponent<SeparatorScript>().separatorObject.PhotonViewID = gameObject.GetComponent<PhotonView>().viewID;

        if (!gameObject.GetComponent<SeparatorScript>().separatorObject.saved)
        {
            string sql = gameObject.GetComponent<SeparatorScript>().separatorObject.CreatSQLFromProperties();
            masterObj.GetComponent<ServerSaveLoadScript>().InsertIntoDatabase(ApplicationStaticData.roomToConnectName, sql);
            gameObject.GetComponent<SeparatorScript>().separatorObject.saved = true;
        }
        else {
            string sql = gameObject.GetComponent<SeparatorScript>().separatorObject.UpdateSQLProperties();
            masterObj.GetComponent<ServerSaveLoadScript>().UpdateInDatabase(ApplicationStaticData.roomToConnectName, sql, gameObject.GetComponent<SeparatorScript>().separatorObject.PhotonViewID);
        }

    }

    private static void SavePhotoSphere(GameObject gameObject)
    {
        gameObject.GetComponent<PhotoSphereScript>().photoSphere.SaveActualTransformation(gameObject.transform);
        gameObject.GetComponent<PhotoSphereScript>().photoSphere.PhotonViewID = gameObject.GetComponent<PhotonView>().viewID;

        if (!gameObject.GetComponent<PhotoSphereScript>().photoSphere.saved)
        {
            string sql = gameObject.GetComponent<PhotoSphereScript>().photoSphere.CreatSQLFromProperties();
            masterObj.GetComponent<ServerSaveLoadScript>().InsertIntoDatabase(ApplicationStaticData.roomToConnectName, sql);
            gameObject.GetComponent<PhotoSphereScript>().photoSphere.saved = true;
        }
        else {
            string sql = gameObject.GetComponent<PhotoSphereScript>().photoSphere.UpdateSQLProperties();
            masterObj.GetComponent<ServerSaveLoadScript>().UpdateInDatabase(ApplicationStaticData.roomToConnectName, sql, gameObject.GetComponent<PhotoSphereScript>().photoSphere.PhotonViewID);
        }
    }

    private static void SaveTextObject(GameObject gameObject)
    {
        gameObject.GetComponent<TextScript>().textObject.SaveActualTransformation(gameObject.transform);
        gameObject.GetComponent<TextScript>().textObject.PhotonViewID = gameObject.GetComponent<PhotonView>().viewID;

        if (!gameObject.GetComponent<TextScript>().textObject.saved)
        {
            string sql = gameObject.GetComponent<TextScript>().textObject.CreatSQLFromProperties();
            masterObj.GetComponent<ServerSaveLoadScript>().InsertIntoDatabase(ApplicationStaticData.roomToConnectName, sql);
            gameObject.GetComponent<TextScript>().textObject.saved = true;
        }
        else {
            string sql = gameObject.GetComponent<TextScript>().textObject.UpdateSQLProperties();
            masterObj.GetComponent<ServerSaveLoadScript>().UpdateInDatabase(ApplicationStaticData.roomToConnectName, sql, gameObject.GetComponent<TextScript>().textObject.PhotonViewID);
        }
    }

    private static void SaveLineObject(GameObject gameObject)
    {
        gameObject.GetComponent<LineScript>().lineObject.SaveActualTransformation(gameObject.transform);
        gameObject.GetComponent<LineScript>().lineObject.PhotonViewID = gameObject.GetComponent<PhotonView>().viewID;
        string JSONPoints = gameObject.GetComponent<LineScript>().CreateJSONFromPoints();

        if (!gameObject.GetComponent<LineScript>().lineObject.saved)
        {
            string sql = gameObject.GetComponent<LineScript>().lineObject.CreatSQLFromProperties();
            masterObj.GetComponent<ServerSaveLoadScript>().InsertIntoDatabase(ApplicationStaticData.roomToConnectName, sql);
            masterObj.GetComponent<ServerSaveLoadScript>().SendJSONPoints(JSONPoints, gameObject.GetComponent<LineScript>().lineObject.pointsFileName);
            gameObject.GetComponent<LineScript>().lineObject.saved = true;
        }
        else {
            string sql = gameObject.GetComponent<LineScript>().lineObject.UpdateSQLProperties();
            masterObj.GetComponent<ServerSaveLoadScript>().UpdateInDatabase(ApplicationStaticData.roomToConnectName, sql, gameObject.GetComponent<LineScript>().lineObject.PhotonViewID);
        }

    }

    public static void DeleteFromDatabase(int photonNumber, string filename = "")
    {
        if (masterObj == null) masterObj = GameObject.Find("Player");
        masterObj.GetComponent<ServerSaveLoadScript>().RemoveFromDatabase(ApplicationStaticData.roomToConnectName, photonNumber, filename);
    }



}
