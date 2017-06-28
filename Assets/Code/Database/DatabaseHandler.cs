using System.Collections;
using UnityEngine;

public class DatabaseHandler: MonoBehaviour{

    private string message;

    public void UpdateUserRoomEntered(string ID, string roomEnteredName)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", ID);
        form.AddField("room", roomEnteredName);
        WWW w = new WWW(ApplicationStaticData.serverScriptsPath + "UpdateRoomEntered.php", form);
        StartCoroutine(request(w));
    }

    public void UpdateStats(string field, string userName)   
    {
        WWWForm form = new WWWForm();
        form.AddField("field", field);
        form.AddField("user", userName);
        WWW w = new WWW(ApplicationStaticData.serverScriptsPath + "UpdateStats.php", form);
        StartCoroutine(request(w));
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
}
