#region Usings

using UnityEngine;
using System.Collections.Generic;

#endregion

public class LinesGL : MonoBehaviour {

    #region Private Properties

    private GameObject line;
    public int pointsCounter;
    private float lineTickness;
    private float minDistanceBetweenPoints = 0.02f;

    private Color lineColor = Color.blue;
    private Vector3 lastPoint;

    private static int lineNumebr = 0;

    private GameObject parentController;

    private List<Vector3> lines;
    private List<Vector3> lastLine;

    private LineDrawingObject myLineObject;

    private int actualFirst;

    private RemoteDrawingScript remoteDrawingScript;


    #endregion

    #region Public Properties

    public bool drawing = false;


    #endregion

    #region Methods

    void Start () {
        lineTickness = 0.1f;
        lines = new List<Vector3>();
        remoteDrawingScript = GameObject.Find("RemoteDrawingObject").GetComponent<RemoteDrawingScript>();
        pointsCounter = 0;
    }


    private void triggerDown(GameObject controller)
    {
        parentController.transform.parent.FindChild("kontroler_pisak").FindChild("kontroler_pisak").FindChild("markerEnding").gameObject.GetComponent<TrailRenderer>().enabled = false;

        if ((myLineObject.straight == true && line == null) || (myLineObject.straight == false))
        {
            drawing = true;
            lines = new List<Vector3>();
            line = GameObject.Find("Player").GetComponent<ObjectSpawnerScript>().SpawnLineObject("line" + lineNumebr.ToString(), 0);
            line.GetComponent<LineScript>().SetLineObject(myLineObject.textureNumber);

            //rysowanie u innych u¿ytkowników
            remoteDrawingScript.StartDrawingOnNetwork(lineTickness, line.GetComponent<PhotonView>().viewID);
            remoteDrawingScript.ChangeColorOnNetwork(lineColor, line.GetComponent<PhotonView>().viewID);
            //remoteDrawingScript.SetLineIndexByNetwork(line.GetComponent<PhotonView>().instantiationId);

            lastPoint = new Vector3();

            SetLineTickness();
            LoadTexture();
            UpdateLineColor();
            

            AddPoint(controller);
            //UpdateLine();
            lineNumebr++;
        }

        if (myLineObject.straight == true) {

            controller.GetComponent<ControlObjects>().TriggerUp += triggerUpStraight;
            controller.GetComponent<ControlObjects>().ControllerMove += UpdateStraightLine;
            actualFirst = lines.Count - 1;
            lastPoint = new Vector3();
        }
        else {
            controller.GetComponent<ControlObjects>().TriggerUp += triggerUp;
            controller.GetComponent<ControlObjects>().ControllerMove += AddPoint;
        }
        
        controller.transform.parent.GetComponent<ControllerTouchpadScript>().TouchpadSwipeOnControllerAction += ChangeLineTickness;


    }

    private void AddPoint(GameObject controller)
    {
        Vector3 newPointPosition = controller.transform.parent.FindChild("kontroler_pisak").FindChild("kontroler_pisak").FindChild("markerEnding").position;
      

        if ((lastPoint == new Vector3()) || (Vector3.Distance(lastPoint, newPointPosition)>minDistanceBetweenPoints ))
        {
            lines.Add(newPointPosition);
            lastPoint = newPointPosition;
            if (lines.Count > 1)
            {
                UpdateLine();
            }

            pointsCounter++;

        }
        
    }

    private void triggerUp(GameObject controller)
    {
        controller.GetComponent<ControlObjects>().ControllerMove -= AddPoint;
        
        controller.transform.parent.GetComponent<ControllerTouchpadScript>().TouchpadSwipeOnControllerAction -= ChangeLineTickness;
        drawing = false;
        parentController.transform.parent.FindChild("kontroler_pisak").FindChild("kontroler_pisak").FindChild("markerEnding").gameObject.GetComponent<TrailRenderer>().enabled = true;
        //AddNormalObjectComponents();
        controller.GetComponent<ControlObjects>().TriggerUp -= triggerUp;

        AddDeleteButton();

        SaveManager.SaveGameObject(line);
    }

    private void triggerUpStraight(GameObject controller)
    {
        controller.GetComponent<ControlObjects>().ControllerMove -= UpdateStraightLine;
        controller.transform.parent.GetComponent<ControllerTouchpadScript>().TouchpadSwipeOnControllerAction -= ChangeLineTickness;
        drawing = false;
        parentController.transform.parent.FindChild("kontroler_pisak").FindChild("kontroler_pisak").FindChild("markerEnding").gameObject.GetComponent<TrailRenderer>().enabled = true;
        controller.GetComponent<ControlObjects>().TriggerUp -= triggerUpStraight;
        AddToLines(lastLine);
    }

    private void UpdateLine(List<Vector3> lin = null)
    {
        lin = (lin == null ? lines : lin);
        line.GetComponent<LineRenderer>().numPositions = lin.Count;
        line.GetComponent<LineRenderer>().SetPositions(lin.ToArray());
        SaveLineObject(lin);

        remoteDrawingScript.DrawPointsOnNetwork(lin[lin.Count - 1], line.GetComponent<PhotonView>().viewID);
    }

    private void UpdateStraightLine(GameObject controller)
    {
        Vector3 e = controller.transform.parent.FindChild("kontroler_pisak").FindChild("kontroler_pisak").FindChild("markerEnding").position;
        if ((lastPoint == new Vector3()) || (Vector3.Distance(lastPoint, e) > minDistanceBetweenPoints))
        {
            List<Vector3> list2 = new List<Vector3>();

            Vector3 first = lines[actualFirst]; 

            list2.Add(first);

            float distance = Vector3.Distance(first, e)*10.0f;

            for (int i = 1; i < (int)distance; ++i)
            {
                Vector3 newPoint = new Vector3();
                newPoint = Vector3.Lerp(first, e, (float)i / distance);
                list2.Add(newPoint);
            }

            Vector3 newPoint2 = new Vector3();
            newPoint2= e;

            list2.Add(newPoint2);

            UpdateLine(GetWholeLine(list2));

            lastLine = list2;
            lastPoint = e;
        }
            
    }

    private void ChangeLineTickness(Vector2 axis, bool first)
    {
        lineTickness = (axis.x + 1.0f) / 10.0f;

        SetLineTickness();

        remoteDrawingScript.ChangeTicknessOnNetwork(lineTickness, line.GetComponent<PhotonView>().viewID);
    }

    public void SetDrawLinesOn(GameObject control, LineDrawingObject line)
    {
        parentController = control;
        myLineObject = line;
        //previousControllerColor = control.transform.parent.FindChild("kontroler nowy").FindChild("colormarker").gameObject.GetComponent<Renderer>().material.color;
        control.transform.parent.FindChild("kontroler nowy").gameObject.SetActive(false);
        control.transform.parent.FindChild("kontroler_pisak").gameObject.SetActive(true);

        control.GetComponent<ControlObjects>().TriggerDown += triggerDown;

        GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").FindChild("panel_color").GetComponent<ToolbarMainColorPickerScript>().colorChange += SetLineColor;
        control.transform.parent.FindChild("kontroler_pisak").FindChild("kontroler_pisak").FindChild("markerEnding").gameObject.GetComponent<TrailRenderer>().enabled = true;

        control.GetComponent<ControlObjects>().LockEntering(true);
    }

    public void SetDrawLinesOff(GameObject control)
    {
        //if(myLineObject.straight) AddNormalObjectComponents();
        control.GetComponent<ControlObjects>().TriggerDown -= triggerDown;

        if (drawing)
        {
            control.GetComponent<ControlObjects>().ControllerMove -= AddPoint;
            control.transform.parent.GetComponent<ControllerTouchpadScript>().TouchpadSwipeOnControllerAction -= ChangeLineTickness;
            drawing = false;
            control.GetComponent<ControlObjects>().TriggerUp -= triggerUp;
        }

        parentController.transform.parent.FindChild("kontroler_pisak").FindChild("kontroler_pisak").FindChild("markerEnding").gameObject.GetComponent<TrailRenderer>().enabled = false;

        control.GetComponent<ControlObjects>().LockEntering(false);
        GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").FindChild("panel_color").GetComponent<ToolbarMainColorPickerScript>().colorChange -= SetLineColor;
       // AddDeleteButton();

        if (line != null && myLineObject.straight == true )
        {
            SaveManager.SaveGameObject(line);
        }
    }

    public void SetLineColor(Color color)
    {
        lineColor = color;
        //if (remoteDrawingScript != null)
        //{
        //    remoteDrawingScript.ChangeColorOnNetwork(color, line.GetComponent<PhotonView>().viewID);
        //}
       
        SetMarkerColor();
        SetMarkerTrailColor();  
    }

    public void UpdateLineColor()
    {
        line.GetComponent<LineRenderer>().material.SetColor("_Color", lineColor);
        line.GetComponent<LineRenderer>().material.SetColor("_EmissionColor", lineColor);
        line.GetComponent<LineRenderer>().receiveShadows = false;
        line.GetComponent<LineRenderer>().startColor = line.GetComponent<LineRenderer>().endColor =  lineColor;
    }

    private void SetMarkerColor()
    {
        parentController.transform.parent.FindChild("kontroler_pisak").FindChild("kontroler_pisak").FindChild("color_pencil").gameObject.GetComponent<Renderer>().material.color = lineColor;
        parentController.transform.parent.FindChild("kontroler_pisak").FindChild("kontroler_pisak").FindChild("colorpanel").gameObject.GetComponent<Renderer>().material.color = lineColor;
        parentController.transform.parent.FindChild("kontroler_pisak").FindChild("kontroler_pisak").FindChild("side_color_line").gameObject.GetComponent<Renderer>().material.color = lineColor;
        parentController.transform.parent.FindChild("kontroler_pisak").FindChild("kontroler_pisak").FindChild("zatyczka").gameObject.GetComponent<Renderer>().material.color = lineColor;
    }

    void OnDestroy()
    {
        parentController.transform.parent.FindChild("kontroler nowy").gameObject.SetActive(true);
        parentController.transform.parent.FindChild("kontroler_pisak").gameObject.SetActive(false);
    }

    private void SetMarkerTrailColor()
    {
        Color color = lineColor;
        color.a = 1.0f;
        parentController.transform.parent.FindChild("kontroler_pisak").FindChild("kontroler_pisak").FindChild("markerEnding").gameObject.GetComponent<TrailRenderer>().material.SetColor("_TintColor", color);
    }

    private List<Vector3> GetWholeLine(List<Vector3> newList)
    {
        List<Vector3> result = new List<Vector3>();

        foreach (Vector3 point in lines)
        {
            result.Add(point);
        }
        foreach (Vector3 point in newList)
        {
            if (result.Count == 0 || !CheckLinePointTheSame(point, result[result.Count - 1]))
            {
                result.Add(point);
            }
            
        }
        return result;
    }

    private void AddToLines(List<Vector3> newList)
    {
        foreach (Vector3 point in newList)
        {
            if (lines.Count == 0 || !CheckLinePointTheSame(point, lines[lines.Count-1]))
            {
                lines.Add(point);
            }
        }
    }

    private bool CheckLinePointTheSame(Vector3 point1, Vector3 point2)
    {
        if (point1 == point2) return true;
        else return false;
    }

    private void SetLineTickness()
    {
        if (line != null)
        {
            line.GetComponent<LineRenderer>().startWidth = line.GetComponent<LineRenderer>().endWidth =  lineTickness;
        }
    }

    private void LoadTexture()
    {
        Texture tex = myLineObject.LoadTexture();
        Material mat = myLineObject.LoadMaterial();

        if (mat != null)
        {
            line.GetComponent<LineRenderer>().material = myLineObject.LoadMaterial();
            line.GetComponent<Renderer>().material = myLineObject.LoadMaterial();
        }
        

        if (tex != null)
        {
            line.GetComponent<LineRenderer>().material.mainTexture = tex;
            line.GetComponent<Renderer>().material.mainTexture = tex;
        }
        


        remoteDrawingScript.ChangeTextureOnNetwork(myLineObject.textureNumber, line.GetComponent<PhotonView>().viewID);
    }

    private void AddDeleteButton()
    {
        if (line != null)
        {

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

            remoteDrawingScript.AddDeleteButton(line.GetComponent<PhotonView>().viewID, center);

            GameObject button =  (GameObject)Instantiate(GameObject.Find("Player").GetComponent<ToolsObjects>().deleteButton);
            button.GetComponent<LineDeleteScript>().line = line;
            button.transform.position = center;
            button.GetComponent<Renderer>().material.SetColor("_Color", lineColor);
        }
    }

    private void SaveLineObject(List<Vector3> lin)
    {
        if (line != null)
        {
            line.GetComponent<LineScript>().lineObject.color = lineColor;
            line.GetComponent<LineScript>().lineObject.tickness = lineTickness;
            line.GetComponent<LineScript>().lineObject.points.points = lin.ToArray();
        }
    }

    #endregion

}
