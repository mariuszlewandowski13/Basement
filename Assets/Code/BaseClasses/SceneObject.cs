#region Usings

using System;

#endregion


[Serializable]
public abstract class SceneObject : PhysicalObject{

    #region Public Properties

    public int PhotonViewID = 0;
    public bool saved = false;

    #endregion

    #region Constructors
    protected SceneObject()
    {

    }
    #endregion

    

}
