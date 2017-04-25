#region Usings

using System;
using UnityEngine;

#endregion

[Serializable]
public abstract class ModyficableObject : SceneObject{

    #region Private Properties

    public int realWidth;
    public int realHeight;

    //zmienne potrzebne przy serializacji
    [SerializeField]Vector3 position;
    [SerializeField]Quaternion rotation;
    [SerializeField]Vector3 scale;

    #endregion

    #region Public Properties

    public float realRatio;
    public float actualRatio;

    #endregion


    #region Constructors
    protected ModyficableObject(int width, int height)
    {
        this.realWidth = width;
        this.realHeight = height;

        this.realRatio = (float)width/(float)height;
        this.actualRatio = this.realRatio;

        scale = new Vector3();
        position = new Vector3();
        rotation = new Quaternion();
    }

    #endregion

    #region Methods
    public void SaveActualTransformation(Transform transform)
    {
        position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        scale = new Vector3(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
    }

    public void SetSavedTransform(Vector3 pos, Quaternion rot, Vector3 scl)
    {
        position = new Vector3(pos.x, pos.y, pos.z);
        rotation = new Quaternion(rot.x, rot.y, rot.z, rot.w);
        scale = new Vector3(scl.x, scl.y, scl.z);
    }


    public Vector3 GetSavedPosition()
    {
        return position;
    }

    public Quaternion GetSavedRotation()
    {
        return rotation;
    }

    public Vector3 GetSavedScale()
    {
        return scale;
    }

    public void UpdateActualRatio(float width, float height)
    {
        actualRatio = width / height;
    }

    public virtual string CreatSQLFromProperties()
    {
        Vector3 position = GetSavedPosition();
        Quaternion rotation = GetSavedRotation();
        Vector3 scale = GetSavedScale();
        return position.x.ToString() + ", " + position.y.ToString() + ", " + position.z.ToString() + ", " + rotation.x.ToString() + ", " + rotation.y.ToString() + ", " + rotation.z.ToString() + ", " + rotation.w.ToString() + ", " +
            scale.x.ToString() + ", " + scale.y.ToString() + ", " + scale.z.ToString();
    }

    public virtual string UpdateSQLProperties()
    {
        Vector3 position = GetSavedPosition();
        Quaternion rotation = GetSavedRotation();
        Vector3 scale = GetSavedScale();
        return "POSITION_X=" + position.x.ToString() + ", POSITION_Y=" + position.y.ToString() + ", POSITION_Z=" + position.z.ToString() + ", ROTATION_X=" + rotation.x.ToString() + ", ROTATION_Y=" + rotation.y.ToString() + ", ROTATION_Z=" + rotation.z.ToString() + ", ROTATION_W=" + rotation.w.ToString() + ", SCALE_X=" +
            scale.x.ToString() + ", SCALE_Y=" + scale.y.ToString() + ", SCALE_Z=" + scale.z.ToString();
    }
    #endregion

}
