#region Usings

using UnityEngine;
using System.Collections;
using System;

#endregion

[RequireComponent(typeof(WorldObjects))]
public class SaveLoadManager : MonoBehaviour {
    #region Private Properties

    //private string savePath = /*Application.dataPath + "/media/saved"*/"Assets/Saved/";
    ////private string savePath = /*Application.dataPath + "/media/saved"*/"C:/files/Assets/Saved/";
    //public string fileName = "saved.txt";
    public static bool isLoaded;

    #endregion

    #region Methods

    void Start()
    {
        isLoaded = false;
    }

    public void SaveGame()
    {
        SaveManager.Save(gameObject);
    }


    public void LoadGame()
    {
            LoadManager.LoadFromServer(gameObject);
           // LoadManager.Load(GetFileFullPath(), gameObject);
        
    }

    //private string GetFileFullPath()
    //{
    //    return savePath + fileName;
    //}
    #endregion

}
