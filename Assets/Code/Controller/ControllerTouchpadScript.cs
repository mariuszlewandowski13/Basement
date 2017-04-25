#region Usings

using UnityEngine;

#endregion

public class ControllerTouchpadScript : MonoBehaviour
{
    #region Public Events & Delegates

    public delegate void InteractionTouchpad(Vector2 axis, bool first);
    public static event InteractionTouchpad TouchpadSwipe;
    public event InteractionTouchpad TouchpadSwipeOnControllerAction;

    public delegate void TouchpadButtonInteraction();
    public event TouchpadButtonInteraction TouchpadDown;

    #endregion

    #region Private Properties

    private Valve.VR.EVRButtonId touchpad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    #endregion


    #region Methods

    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }
        Vector2 axis = controller.GetAxis();
        bool touch = controller.GetTouchDown(touchpad);

        bool touchpadDown = controller.GetPressDown(touchpad);

        if (TouchpadSwipe != null && controller.GetTouch(touchpad))
        {
            TouchpadSwipe(axis, touch);
        }

        if (TouchpadSwipeOnControllerAction != null && controller.GetTouch(touchpad))
        {
            TouchpadSwipeOnControllerAction(axis, touch);
        }

        if (touchpadDown && TouchpadDown != null)
        {
            TouchpadDown();
        }
    }

    #endregion
}