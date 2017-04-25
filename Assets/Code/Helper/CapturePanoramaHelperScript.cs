using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CapturePanoramaHelperScript : MonoBehaviour {

    public CapturePanorama.CapturePanorama capturePanorama;

    public bool capture;

    private int frameNum = 1;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (capture)
        {
            if (capturePanorama != null)
            {
                string filenameBase = "test" + frameNum.ToString();
                capturePanorama.CaptureScreenshotAsync(filenameBase);
                frameNum++;
            }
        }
	}
}
