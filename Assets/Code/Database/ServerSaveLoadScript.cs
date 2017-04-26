#region Usings

using UnityEngine;
using System.Collections;
using System;

#endregion

public class ServerSaveLoadScript : MonoBehaviour {
    #region Private Properties

    private string message;

    #endregion

    #region Delegates

    public delegate void ResultMethod(string[] x);
    public delegate void EndCreateLines(LineObject line);
    public delegate void LineOnServer(int photonId);

    #endregion

    #region Methods


    public void LoadDb(ResultMethod f, string roomName)   
    {
        WWWForm form = new WWWForm();
        form.AddField("room", roomName);
        WWW w = new WWW("http://serwer1642668.home.pl/BASEMENT/scripts/GetRoomsOS.php", form);
        StartCoroutine(loader(w, f));
    }

    public void LoadLinesFile(LineObject lineObject, string filename, EndCreateLines line)   
    {
        WWWForm form = new WWWForm();
        form.AddField("filename", filename);
        WWW w = new WWW("http://serwer1642668.home.pl/BASEMENT/scripts/GetFileAsString.php", form);
        StartCoroutine(LineLoader(w, line, lineObject));
    }

    public void InsertIntoDatabase(string roomName, string values)  
    {
        WWWForm form = new WWWForm();
        string values2 = values + ", "+ GetAdditionalInsertionData();
        form.AddField("values", values2);
        form.AddField("room", roomName);
        form.AddField("operation", 0);
        WWW w = new WWW("http://serwer1642668.home.pl/BASEMENT/scripts/SaveChangesOS.php", form);
        StartCoroutine(request(w));
    }

    public void SendJSONPoints(string JSON, string filename)   
    {
        WWWForm form = new WWWForm();

        form.AddField("filename", filename);
        form.AddField("JSON", JSON);
        WWW w = new WWW("http://serwer1642668.home.pl/BASEMENT/scripts/CreateLineFile.php", form);
        StartCoroutine(request(w));
    }

    public void UpdateInDatabase(string roomName, string values, int photonView)   
    {
        WWWForm form = new WWWForm();
        form.AddField("values", values);
        form.AddField("room", roomName);
        form.AddField("ID", photonView.ToString());
        form.AddField("operation", 1);
        WWW w = new WWW("http://serwer1642668.home.pl/BASEMENT/scripts/SaveChangesOS.php", form);
        StartCoroutine(request(w));
    }

    public void RemoveFromDatabase(string roomName, int photonView, string filename = "")   
    {
        WWWForm form = new WWWForm();

        form.AddField("ID", photonView.ToString());
        form.AddField("filename", filename);
        form.AddField("room", roomName);
        WWW w = new WWW("http://serwer1642668.home.pl/BASEMENT/scripts/RemoveFromDbOS.php", form);
        StartCoroutine(request(w));
    }



    IEnumerator loader(WWW w, ResultMethod f)
    {
        yield return w;
        if (w.error == null)
        {
            message = w.text;
        }
        else {
            message = "ERROR: " + w.error + "\n";
        }

        Debug.Log(message);
        string[] msg = null;
        string[] res = null;

        msg = message.Split(new string[] { "@@@@@" }, StringSplitOptions.None);
        foreach (string row in msg)
        {
            res = row.Split(new string[] { "#####" }, StringSplitOptions.None);
            if (res.Length > 1)
            {

                f(res);
            }
        }
        SaveLoadManager.isLoaded = true;

    }

    IEnumerator request(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            message = w.text;
        }
        else {
            message = "ERROR: " + w.error + "\n";
        }

        Debug.Log(message);  
    }

    IEnumerator LineLoader(WWW w, EndCreateLines f, LineObject lineObj)
    {
        yield return w;
        try
        {
            if (w.error == null)
            {
                message = w.text;
                lineObj.SetPointsFromJSON(message);

                if (lineObj.points != null && lineObj.points.Length > 0)
                {
                    f(lineObj);
                }
                else {
                    SaveManager.DeleteFromDatabase(lineObj.PhotonViewID, lineObj.pointsFileName);
                }
            }
            else {
                message = "ERROR: " + w.error + "\n";
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private string GetAdditionalInsertionData()
    {
        return ApplicationStaticData.userID +", now(), now()";
    }

    #endregion

}
