#region Usings

using UnityEngine;
using System.Collections;

#endregion


public abstract class ObjectSpawner {

    #region Private Properties

    public static string tagName = "GameObject";
    public static int objectIndex = 400000;

    #endregion

    #region Methods
    public static void DestroyObject(Object objectToDestroy)
    {
        GameObject.Destroy(objectToDestroy);
    }

    public static void SetRandomObjectId()
    {
        System.Random rnd = new System.Random();
        objectIndex = rnd.Next(100, 2000000000);
    }
    #endregion


}
