#region Usings

using UnityEngine;

#endregion

public class LineDrawingScript : MonoBehaviour {

    #region Private Properties

    private LineDrawingObject marker;
    private GameObject controller;
    private int deactivatedController;

    private bool toActivate = false;
    private bool toDestroy = false;

    #endregion

    #region Methods

    public void SetMarker(LineDrawingObject newLineDrawer, GameObject controller)
    {
        marker = newLineDrawer;
        this.controller = controller;
        TurnOn();
    }

    void Update()
    {
        if (toActivate)
        {
            ActiveTeleport();
            toActivate = false;
        }
        if (toDestroy)
        {
            Destroy(gameObject);
        }
    }

    public void TurnOn()
    {
        DeactiveTeleport();
        controller.transform.parent.GetComponent<ControllerTouchpadScript>().TouchpadDown += BackButtonClicked;
            
        if (GameObject.Find("Toolbar") != null)
        {
            GameObject.Find("Toolbar").transform.FindChild("buttons").FindChild("BackButton").GetComponent<BackButtonScript>().ButtonDown += BackButtonClicked;
        }
        

        GetComponent<LinesGL>().SetDrawLinesOn(transform.parent.FindChild("ControlObject").gameObject, marker);
        GetComponent<LinesGL>().SetLineColor(marker.color);
    }

    public void TurnOff()
    {
        controller.transform.parent.GetComponent<ControllerTouchpadScript>().TouchpadDown -= BackButtonClicked;

        toActivate = true;
        GetComponent<LinesGL>().SetDrawLinesOff(transform.parent.FindChild("ControlObject").gameObject);

        if (GameObject.Find("Toolbar") != null)
        {
            GameObject.Find("Toolbar").transform.FindChild("buttons").FindChild("BackButton").GetComponent<BackButtonScript>().ButtonDown -= BackButtonClicked;
        }
            
    }

    public void BackButtonClicked()
    {
        if (!GetComponent<LinesGL>().drawing)
        {
            TurnOff();
            toDestroy = true;
        }
    }

    private void ActiveTeleport()
    {
        GameObject.Find("Player").transform.FindChild("Camera (eye)").GetComponent<TeleportVive>().canControllerTeleport[deactivatedController] = true;
    }

    private void DeactiveTeleport()
    {
        Transform cameraEye = GameObject.Find("Player").transform.FindChild("Camera (eye)");

        if (cameraEye.GetComponent<TeleportVive>().Controllers[0] == controller.transform.parent.GetComponent<SteamVR_TrackedObject>())
        {
            deactivatedController = 0;
            cameraEye.GetComponent<TeleportVive>().canControllerTeleport[0] = false;
        }
        else {
            deactivatedController = 1;

            cameraEye.GetComponent<TeleportVive>().canControllerTeleport[1] = false;
        }
    }

    #endregion
}
