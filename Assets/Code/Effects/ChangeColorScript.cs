using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorScript : MonoBehaviour {

    private Color myColor;
    private Color newColor;

    public void ChangeColor(Color col)
    {
        if (GetComponent<Renderer>() != null)
        {
            myColor = GetComponent<Renderer>().material.color;
            newColor = col;
            GetComponent<Renderer>().material.SetColor("_Color", newColor);
        }
    }

    public void RestoreColor()
    {
        if (GetComponent<Renderer>() != null)
        {
            GetComponent<Renderer>().material.SetColor("_Color", myColor);
        }
    }
}
