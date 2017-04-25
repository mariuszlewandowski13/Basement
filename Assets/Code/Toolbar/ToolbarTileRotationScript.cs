#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class ToolbarTileRotationScript : MonoBehaviour {

    #region Events

    public delegate void Number();
    public static event Number Increase;
    public static event Number Decrease;
    #endregion

    #region Private Properties

    private float multiplier = 10.0f;

    #endregion

    void Start () {
        ToolbarRotationScript.MoveEvent += Move;
        float ile;
        Vector3 vec;

        transform.localRotation.ToAngleAxis(out ile, out vec);

        //SetAlpha(ile);
    }

    private void Move(float Speed)
    {
        transform.Rotate(new Vector3(0.0f, Speed*multiplier));
        float ile;
        Vector3 vec;
        GameObject miniature;

        transform.localRotation.ToAngleAxis(out ile, out vec);

        

        if (ile < 15.0f)
        {
            miniature = transform.FindChild("ToolbarTile").GetComponent<ToolbarTileMiniatureScript>().GetRightSceneObject();
            if (miniature != null)
            {
                transform.Rotate(new Vector3(0.0f, 150.0f, 0.0f));
                if (Decrease != null)
                {
                    Decrease();
                }
            }
            
            //if (miniature.GetComponent<Renderer>() != null)
            //{
            //    Color color = miniature.GetComponent<Renderer>().material.color;
            //    color.a = 0.0f;
            //    miniature.GetComponent<Renderer>().material.color = color;
            //}
        }
        else if (ile > 165.0f)
        {
            miniature = transform.FindChild("ToolbarTile").GetComponent<ToolbarTileMiniatureScript>().GetLeftSceneObject();

            if (miniature != null)
            {
                transform.Rotate(new Vector3(0.0f, -150.0f, 0.0f));
                if (Increase != null)
                {
                    Increase();
                }
            }

            


            //if (miniature.GetComponent<Renderer>() != null)
            //{
            //    Color color = miniature.GetComponent<Renderer>().material.color;
            //    color.a = 0.0f;
            //    miniature.GetComponent<Renderer>().material.color = color;
            //}
        }
        // SetAlpha(ile);

    }

    private void SetAlpha(float angle)
    {

        if (angle < 30.0f)
        {
            Color color = transform.FindChild("ToolbarTile").GetComponent<Renderer>().material.color;
            color.a = (angle - 10.0f) / 20.0f;
            transform.FindChild("ToolbarTile").GetComponent<Renderer>().material.color = color;
            if (transform.FindChild("ToolbarTile").FindChild("TileMiniature") != null && transform.FindChild("ToolbarTile").FindChild("TileMiniature").GetComponent<Renderer>() != null)
            {
                transform.FindChild("ToolbarTile").FindChild("TileMiniature").GetComponent<Renderer>().material.color = color;
            }
        }
        else if (angle > 110.0f)
        {
            Color color = transform.FindChild("ToolbarTile").GetComponent<Renderer>().material.color;
            color.a = (130.0f - angle) / 20.0f;
            transform.FindChild("ToolbarTile").GetComponent<Renderer>().material.color = color;
            if (transform.FindChild("ToolbarTile").FindChild("TileMiniature") != null && transform.FindChild("ToolbarTile").FindChild("TileMiniature").GetComponent<Renderer>() != null)
            {
                transform.FindChild("ToolbarTile").FindChild("TileMiniature").GetComponent<Renderer>().material.color = color;
            }

        }
    }

    void OnDestroy()
    {
        ToolbarRotationScript.MoveEvent -= Move;
    }

}
