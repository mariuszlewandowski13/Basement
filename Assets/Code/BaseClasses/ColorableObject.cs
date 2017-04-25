#region Usings

using UnityEngine;

#endregion

public class ColorableObject : VisualObject {

    #region Public Properties

    public Color color;

    #endregion

    #region Constructors

    protected ColorableObject(int width, int height, Color color) : base(width, height)
    {
        this.color = color;
    }

    #endregion


    #region Methods

    public override string CreatSQLFromProperties()
    {
        return base.CreatSQLFromProperties() + ", " + color.r.ToString() + ", " + color.g.ToString() + ", " + color.b.ToString() + ", " + color.a.ToString();
    }


    public override string UpdateSQLProperties()
    {
        return base.UpdateSQLProperties() + ", COLOR_R=" + color.r + ", COLOR_G = " + color.g + ", COLOR_B = " + color.b + ", COLOR_A = " + color.a;
    }

    #endregion

}
