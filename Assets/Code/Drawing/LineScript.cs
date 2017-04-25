#region Usings

using UnityEngine;
using System.IO;

#endregion

public class LineScript : MonoBehaviour {

    #region Public Properties

    public LineObject lineObject;
    public bool remoteDrawing = false;

    #endregion

    #region Methods

    public void SetLineObject(int texNum)
    {
        lineObject = new LineObject(texNum);
    }

    public void SetLineObject(LineObject line)
    {
        lineObject = line;
    }

    public void UpdateLineObjectColor(Color color)
    {
        if (lineObject != null)
        {
            lineObject.color = color;
        }
    }

    public void UpdateLineObjectPoints(Vector3 [] points)
    {
        if (lineObject != null)
        {
            lineObject.points.points = points;
        }
    }

    public void LoadFromLineObject()
    {
        if (lineObject != null)
        {
            Texture tex = GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").GetComponent<LinesTexturesScript>().lines[lineObject.textureNumber];
            Material mat = GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").GetComponent<LinesTexturesScript>().linesMaterials[lineObject.textureNumber];

            if (mat != null)
            {
                GetComponent<LineRenderer>().material = mat;
                GetComponent<Renderer>().material = mat;
            }

            if (tex != null)
            {
                GetComponent<LineRenderer>().material.mainTexture = tex;
                GetComponent<Renderer>().material.mainTexture = tex;
            }

            GetComponent<Renderer>().material.SetColor("_Color", lineObject.color);
            GetComponent<Renderer>().material.SetColor("_EmissionColor", lineObject.color);

            GetComponent<LineRenderer>().material.SetColor("_Color", lineObject.color);
            GetComponent<LineRenderer>().material.SetColor("_EmissionColor", lineObject.color);


            GetComponent<LineRenderer>().startWidth = GetComponent<LineRenderer>().endWidth =  lineObject.tickness;
            GetComponent<LineRenderer>().numPositions =  lineObject.points.Length;
            GetComponent<LineRenderer>().SetPositions(lineObject.points.points);
            GetComponent<LineRenderer>().startColor = GetComponent<LineRenderer>().endColor = lineObject.color;

            transform.position = lineObject.GetSavedPosition();
            transform.rotation = lineObject.GetSavedRotation();
            transform.localScale = lineObject.GetSavedScale();

        }
    }

    public void CreateDeleteButton()
    {

        Vector3[] lines = lineObject.points.points;
            float xMax = -100.0f, xMin = 100.0f, yMax = -100.0f, yMin = 100.0f, zMax = -100.0f, zMin = 100.0f;
            foreach (Vector3 point in lines)
            {
                if (point.x > xMax) xMax = point.x;
                if (point.x < xMin) xMin = point.x;
                if (point.y > yMax) yMax = point.y;
                if (point.y < yMin) yMin = point.y;
                if (point.z > zMax) zMax = point.z;
                if (point.z < zMin) zMin = point.z;
            }
            Vector3 center = new Vector3((xMax + xMin) / 2.0f, (yMax + yMin) / 2.0f, (zMax + zMin) / 2.0f);
            GameObject button = (GameObject)Instantiate(GameObject.Find("Player").GetComponent<ToolsObjects>().deleteButton);
            button.GetComponent<LineDeleteScript>().line = gameObject;
            button.transform.position = center;
            button.GetComponent<Renderer>().material.SetColor("_Color", lineObject.color);

        }

    public string CreateJSONFromPoints()
    {
        string JSON = "";
        if (lineObject.pointsFileName == "")
        {
            lineObject.pointsFileName = Path.GetRandomFileName();
        }
        JSON = JsonUtility.ToJson(lineObject.points);
        return JSON;
    }

    public void DeleteJSONFileFromPoints()
    {
        lineObject.pointsFileName = "";
    }
    #endregion
}


