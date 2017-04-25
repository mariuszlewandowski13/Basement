using UnityEngine;
using System.Collections;

public class ToolbarMainColorPickerScript : MonoBehaviour {

    #region Public Properties

    public Color color ;
    public float brightness = 0.5f;

    public delegate void ColorChange(Color color);
    public event ColorChange colorChange;

    #endregion


    #region Methods
    void Awake()
    {
        color = Color.white;
    }

    void Start()
    {
        gameObject.GetComponent<Renderer>().sortingLayerName = "ToolsSortingLayer";
    }

    public void SetColor(Color color)
    {
        this.color = color;
        double h, s, l;
        HelperFunctionsScript.RGB2HSL(color, out h, out s, out l);
        l = brightness;
        color = HelperFunctionsScript.HSL2RGB(h, s, l);
        color.a = 1.0f;
        if (colorChange != null)
        {
            colorChange(color);
        }
    }

    public void SetBrightness(float brightness)
    {
        this.brightness = brightness;
        double h, s, l;
        HelperFunctionsScript.RGB2HSL(color, out h, out s, out l);
        l = brightness;
        color = HelperFunctionsScript.HSL2RGB(h, s, l);
        color.a = 1.0f;
        if (colorChange != null)
        {
            colorChange(color);
        }
    }


    #endregion
}
