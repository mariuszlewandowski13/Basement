#region Usings

using System;
using UnityEngine;

#endregion

[Serializable]
public abstract class VisualObject : ModyficableObject {

    #region Constructors
    protected VisualObject(int width, int height) : base(width, height)
    {

    }
    #endregion

}
