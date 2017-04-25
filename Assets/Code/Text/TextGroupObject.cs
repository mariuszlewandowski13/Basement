#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class TextGroupObject : VisualObject {
    #region Public Properties

    public int textGroupObjectNumber;
    public Color color;

    #endregion

    #region Constructors

    public TextGroupObject(int number): base(0, 0)
    {
        this.textGroupObjectNumber = number;
    }

    #endregion



}
