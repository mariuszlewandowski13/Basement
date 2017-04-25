#region Usings

using UnityEngine;
using System.Collections.Generic;

#endregion

public class RemoteDrawingScript : MonoBehaviour {

    #region Private Properties

    private static int lineNumebr = 0;

    #endregion

    #region Methods

    void Start()
    {
        lineNumebr = 0;
    }

    public void StartDrawingOnNetwork(float tickness, int number)
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("StartDrawing", PhotonTargets.OthersBuffered, tickness, number);
        }
        
    }

    [PunRPC]
    private void StartDrawing(float lineTick, int number)
    {
        if (PhotonView.Find(number) == null)
        {
            GameObject line = GameObject.Find("Player").GetComponent<ObjectSpawnerScript>().SpawnLineObject("line" + lineNumebr.ToString(), number);
            line.GetComponent<LineRenderer>().startWidth = line.GetComponent<LineRenderer>().endWidth = lineTick;
            line.GetComponent<LineScript>().remoteDrawing = true;
            lineNumebr++;
        }
    }

    public void ChangeTicknessOnNetwork(float tickness, int number)
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("ChangeTickness", PhotonTargets.OthersBuffered, tickness, number);
        } 
    }

    [PunRPC]
    private void ChangeTickness(float lineTick, int number)
    {
        if (PhotonView.Find(number) != null && PhotonView.Find(number).GetComponent<LineScript>().remoteDrawing == true)
        {
            GameObject line = PhotonView.Find(number).gameObject;
            line.GetComponent<LineRenderer>().startWidth = line.GetComponent<LineRenderer>().endWidth = lineTick;
            line.GetComponent<LineScript>().lineObject.tickness = lineTick;
        }
        
    }

    public void DrawPointsOnNetwork(Vector3 point, int number)
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("DrawPoint", PhotonTargets.OthersBuffered, point, number);
        }
        
    }

    [PunRPC]
    private void DrawPoint(Vector3 point, int number)
    {
        if (PhotonView.Find(number) != null && PhotonView.Find(number).GetComponent<LineScript>().remoteDrawing == true)
        {
            GameObject line = PhotonView.Find(number).gameObject;
            List<Vector3> newPoints;
            if (line.GetComponent<LineScript>().lineObject.points.points != null)
            {
                newPoints = new List<Vector3>(line.GetComponent<LineScript>().lineObject.points.points);
            }
            else {
                newPoints = new List<Vector3>();
            }

            
            newPoints.Add(point);
            line.GetComponent<LineScript>().lineObject.points.points = newPoints.ToArray();
            line.GetComponent<LineRenderer>().numPositions = newPoints.Count;
            line.GetComponent<LineRenderer>().SetPositions(line.GetComponent<LineScript>().lineObject.points.points);
        }
        
    }


    public void ChangeColorOnNetwork(Color color, int number)
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("ChangeColorOnNetwork", PhotonTargets.OthersBuffered, color.r, color.g, color.b, color.a, number);
        }
       
    }

    [PunRPC]
    private void ChangeColorOnNetwork(float r, float g, float b, float a, int number)
    {
        if (PhotonView.Find(number) != null && PhotonView.Find(number).GetComponent<LineScript>().remoteDrawing == true)
        {
            GameObject line = PhotonView.Find(number).gameObject;

            Color color = new Color(r, g, b, a);
            line.GetComponent<LineRenderer>().material.SetColor("_Color", color);
            line.GetComponent<LineRenderer>().receiveShadows = false;
            line.GetComponent<LineRenderer>().startColor = line.GetComponent<LineRenderer>().endColor = color;
            line.GetComponent<LineScript>().lineObject.color = color;
        }
        

    }

    public void ChangeTextureOnNetwork(int texNumber, int number)
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("ChangeTextureByNetwork", PhotonTargets.OthersBuffered, texNumber, number);
        }
        
    }

    [PunRPC]
    private void ChangeTextureByNetwork(int texNumber, int number)
    {
        if (PhotonView.Find(number) != null && PhotonView.Find(number).GetComponent<LineScript>().remoteDrawing == true)
        {
            GameObject line = PhotonView.Find(number).gameObject;

            Texture tex = LoadTexture(texNumber);
            Material mat = LoadMaterial(texNumber);

            if (mat != null)
            {
                line.GetComponent<LineRenderer>().material = mat;
                line.GetComponent<Renderer>().material = mat;
            }

            if (tex != null)
            {
                line.GetComponent<LineRenderer>().material.mainTexture = tex;
                line.GetComponent<Renderer>().material.mainTexture = tex;
            }
            line.GetComponent<LineScript>().lineObject.textureNumber = texNumber;
        }
    }

    private Texture LoadTexture(int num)
    {
        return GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").GetComponent<LinesTexturesScript>().lines[num];
    }

    private Material LoadMaterial(int num)
    {
        return GameObject.Find("Player").transform.FindChild("Toolbar").FindChild("paski").GetComponent<LinesTexturesScript>().linesMaterials[num];
    }


    public void DestroyLine(int number)
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("DestroyLineByNetwork", PhotonTargets.OthersBuffered, number);
        } 
    }

    [PunRPC]
    private void DestroyLineByNetwork(int number)
    {
        if (PhotonView.Find(number) != null)
        {
            Destroy(PhotonView.Find(number).gameObject);
        }
        
    }

    public void AddDeleteButton(int number, Vector3 center)
    {
        if (PhotonNetwork.inRoom )
        {
            GetComponent<PhotonView>().RPC("AddDeleteButtonByNetwork", PhotonTargets.OthersBuffered, number, center);
        }
    }

    [PunRPC]
    private void AddDeleteButtonByNetwork(int number, Vector3 center)
    { 
        if(PhotonView.Find(number) != null && PhotonView.Find(number).GetComponent<LineScript>().remoteDrawing == true)
        {
            GameObject line = PhotonView.Find(number).gameObject;
            GameObject button = (GameObject)Instantiate(GameObject.Find("Player").GetComponent<ToolsObjects>().deleteButton);
            button.GetComponent<LineDeleteScript>().line = line;
            button.transform.position = center;
            button.GetComponent<Renderer>().material.SetColor("_Color", line.GetComponent<LineRenderer>().material.color);
        }
    }


    #endregion
}
