#region Usings

using System;
using UnityEngine;

#endregion

[Serializable]
public class SphereObject : ModyficableObject {


    #region Public Properties

    public float scaleX;
    public float scaleY;
    public float scaleZ;

    #endregion


    #region Constructors
    protected SphereObject(float scaleX, float scaleY, float scaleZ):base(1, 1)
    {
        this.scaleX = scaleX;
        this.scaleY = scaleY;
        this.scaleZ = scaleZ;               
    }


    #endregion

}
