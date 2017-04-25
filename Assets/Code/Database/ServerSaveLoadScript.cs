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


    public void LoadDb(ResultMethod f, string roomName)   // WITH CHAT MG
    {
        WWWForm form = new WWWForm();
        string sqlCommand = "SELECT * FROM " + roomName;
        form.AddField("sqlCommand", sqlCommand);
        WWW w = new WWW("http://serwer1642668.home.pl/BASEMENT/scripts/GetRooms.php", form);
        StartCoroutine(loader(w, f));
    }

    public void LoadLinesFile(LineObject lineObject, string filename, EndCreateLines line)   // WITH CHAT MG
    {
        WWWForm form = new WWWForm();
        form.AddField("filename", filename);
        WWW w = new WWW("http://serwer1642668.home.pl/BASEMENT/scripts/GetFileAsString.php", form);
        StartCoroutine(LineLoader(w, line, lineObject));
    }

    public void InsertIntoDatabase(string roomName, string values)   // WITH CHAT MG
    {
        WWWForm form = new WWWForm();
        string sqlCommand = "INSERT INTO " + roomName + "  VALUES (" + values + ", "+ GetAdditionalInsertionData() +")";
        //Debug.Log(sqlCommand);
        form.AddField("sqlCommand", sqlCommand);
        form.AddField("room", roomName);
        WWW w = new WWW("http://serwer1642668.home.pl/BASEMENT/scripts/SaveChanges.php", form);
        StartCoroutine(request(w));
    }

    public void SendJSONPoints(string JSON, string filename)   // WITH CHAT MG
    {
        WWWForm form = new WWWForm();

        form.AddField("filename", filename);
        form.AddField("JSON", JSON);
        WWW w = new WWW("http://serwer1642668.home.pl/BASEMENT/scripts/CreateLineFile.php", form);
        StartCoroutine(request(w));
    }

    public void UpdateInDatabase(string roomName, string values, int photonView)   // WITH CHAT MG
    {
        WWWForm form = new WWWForm();
        string sqlCommand = "UPDATE " + roomName + "  SET " + values + " WHERE PHOTON_VIEW_ID = " + photonView.ToString();
        //Debug.Log(sqlCommand);
        form.AddField("sqlCommand", sqlCommand);
        WWW w = new WWW("http://serwer1642668.home.pl/BASEMENT/scripts/SaveChanges.php", form);
        StartCoroutine(request(w));
    }

    public void RemoveFromDatabase(string roomName, int photonView, string filename = "")   // WITH CHAT MG
    {
        WWWForm form = new WWWForm();
        //Debug.Log("Destroy " + photonView);
        string sqlCommand = "DELETE FROM " + roomName + " WHERE PHOTON_VIEW_ID = " + photonView.ToString();
        form.AddField("sqlCommand", sqlCommand);
        form.AddField("filename", filename);
        form.AddField("room", roomName);
        WWW w = new WWW("http://serwer1642668.home.pl/BASEMENT/scripts/RemoveFromDb.php", form);
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

       // Debug.Log(message);  
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
