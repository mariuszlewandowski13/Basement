using UnityEngine;

public static class ControlObjectsHelper  {

    private static Transform rightController;
    private static Transform rightControlObject;
    private static Transform leftController;
    private static Transform leftControlObject;
    private static Transform cameraEye;

    public static GameObject LeftController
    {
        get {
            return leftController.gameObject;
        }
        
    }


    public static GameObject RightController
    {
        get {
            if (rightController != null)
            {
                return rightController.gameObject;
            }
            else {
                return null;
            }
            
        }
        
    }

    public static GameObject LeftControlObject
    {
        get {
            return leftControlObject.gameObject;
        }
        
    }

    public static GameObject RightControlObject
    {
        get {
            return rightControlObject.gameObject;
        } 
    }


    public static Ray RightControllerRay
    {
        get
        {
            if (rightController != null)
            {
                if (rightControlObject != null)
                {
                    return new Ray(rightControlObject.position, rightControlObject.forward);
                }
            }
            return new Ray();
        }

    }


    public static GameObject CameraEye
    {
        get
        {
            return cameraEye.gameObject;
        }

    }

    public static void UpdateControllers()
    {
        rightController = GameObject.Find("Player").transform.FindChild("Controller (right)");
        rightControlObject = GameObject.Find("Player").transform.FindChild("Controller (right)").FindChild("ControlObject");
        leftController = GameObject.Find("Player").transform.FindChild("Controller (left)");
        leftControlObject = GameObject.Find("Player").transform.FindChild("Controller (left)").FindChild("ControlObject");
        cameraEye = GameObject.Find("Player").transform.FindChild("Camera (eye)");
    }


}
