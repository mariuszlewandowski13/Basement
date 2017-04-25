#region Usings

using UnityEngine;
using System.Collections;
using System;

#endregion

[RequireComponent(typeof(WorldObjects))]
public class SaveLoadManager : MonoBehaviour {
    #region Private Properties
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
        
    }
    #endregion

}
