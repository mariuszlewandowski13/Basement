#region Usings

using UnityEngine;
using System;

#endregion

public class PanoramaCaptureWrapper : MonoBehaviour {

    #region Private Properties

    private CapturePanorama.CapturePanorama capturePanorama;
    private string screenShotsFolderPath = "Assets/Artwork/CapturedScreenshots/";

    #endregion

    #region Methods
    void Start () {
        capturePanorama = gameObject.GetComponent<CapturePanorama.CapturePanorama>();
    }

    void CapturePanorama()
    {
        if (ChechErrors())
        {
            string filenameBase = String.Format("{0}{1}_{2:yyyy-MM-dd_HH-mm-ss-fff}", screenShotsFolderPath, capturePanorama.panoramaName, DateTime.Now);
            Debug.Log("Panorama capture key pressed, capturing " + filenameBase);
            capturePanorama.CaptureScreenshotAsync(filenameBase);
        }
                
    }
    /*
     * Sprawdza ewentualne błędy, pobrane prawie bezpośrednio z pluginu
     * false - są błędy
     * true - nie ma błędów
     * */
    bool ChechErrors()
    {
        if (capturePanorama.panoramaWidth < 4 || (capturePanorama.captureStereoscopic && capturePanorama.numCirclePoints < 8)) // Can occur temporarily while modifying properties in editor
        {
            
                if (capturePanorama.panoramaWidth < 4)
                    Debug.LogError("Panorama Width must be at least 4. No panorama captured.");
                if (capturePanorama.captureStereoscopic && capturePanorama.numCirclePoints < 8)
                    Debug.LogError("Num Circle Points must be at least 8. No panorama captured.");
            return false;
        }

        return true;
    }

    #endregion


}
