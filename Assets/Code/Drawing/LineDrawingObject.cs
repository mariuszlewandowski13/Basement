#region Usings

using UnityEngine;

#endregion

public class LineDrawingObject : ToolsObject {

    #region Public Properties

    public Color color;
    public bool straight;
    public int textureNumber;
    #endregion

    #region Constructors

    public LineDrawingObject(Color color, int textureNumber)
    {
        this.color = color;
        straight = false;
        this.textureNumber = textureNumber;

    }

    public LineDrawingObject(Color color, bool straight,  int textureNumber)
    {
        this.color = color;
        this.straight = straight;
        this.textureNumber = textureNumber;
    }

    #endregion

    #region Methods

    public Texture LoadTexture()
    {
        return GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").GetComponent<LinesTexturesScript>().lines[textureNumber];
    }

    public Material LoadMaterial()
    {
        return GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").GetComponent<LinesTexturesScript>().linesMaterials[textureNumber];
    }

    #endregion

}
