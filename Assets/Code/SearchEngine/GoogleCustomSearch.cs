using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using SimpleJSON;
using System.Threading;
using System.Collections.Generic;


public class GoogleCustomSearch : MonoBehaviour {

    public string query;
    public List<ImageObject> images;

    public delegate void Result();
    public event Result resultReady;

    private int treshold = 25;

    private object imagesLock = new object();

    private int searchCount;

    public void Search(string text)
    {
        images = new List<ImageObject>();
        query = text;
        StartCoroutine(SendRequest());
    }


    IEnumerator SendRequest()
    {
        query = query.Replace(" ", "+");
        WWW request = new WWW("https://www.googleapis.com/customsearch/v1?key=AIzaSyDDLq3MmRCRdlroJo8PDS-Qa0Xy_O6K0_k&cx=014885167294475863976:fcnmgtf8cmc&q=" + query + "&searchType=image&fileType=jpg&imgSize=medium&alt=json");
        yield return request;
        if (request.error == null || request.error == "")
        {
            string output = fromatJSON(request);
            var N = JSON.Parse(output);
            searchCount = N["queries"]["request"][0]["count"].AsInt;
            for (int i = 0; i < (searchCount >= treshold ? treshold : searchCount); i++)
            {
                string url = N["items"][i]["link"];   // Full Size 
                string val = N["items"][i]["image"]["thumbnailLink"];
                StartCoroutine(fetch(val, url, true)); 
            }
        }
        else
        {
            Debug.Log("WWW error: " + request.error);
            if (resultReady != null)
            {
                resultReady();
            }
        }
    }
    
    IEnumerator fetch(string thumbUrl,string url, bool last)
    {
        WWW www = new WWW(thumbUrl);        
        yield return www;

        WWW www2 = new WWW(url);
        yield return www2;

        lock (imagesLock)
        {
            if ((www.error == null || www.error == "")  && (www2.error == null || www2.error == "") && www.texture != null && www.error == null && www2.texture != null && www2.error == null)
            {
                try
                {
                    images.Add(new ImageObject(www2.texture.width, www2.texture.height, url, "", LoadingType.remote, thumbUrl));
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    throw;
                }
            }

            if (images.Count == searchCount && resultReady != null)
            {
                resultReady();
            }
        }

    }

    private string fromatJSON(WWW request)
    {
        int n = 2;
        string[] lines = request.text
            .Split(Environment.NewLine.ToCharArray())
            .Skip(n)
            .ToArray();
        string output = string.Join(Environment.NewLine, lines);
        output = "{" + output;
        output = output.Remove(output.LastIndexOf(Environment.NewLine));
        return output;
    }

}
