#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class ToolbarRotationScript : MonoBehaviour {

    #region Events

    public delegate void MoveAction(float Speed);
    public static event MoveAction MoveEvent;


    #endregion

    #region Private Properties

    private float Speed = 0.00f;
    private float maxSpeed = 1.0f;
    private float decreasingFactor = 0.003f;

    private Vector2 touchpadPrevPosition;

    private object speedLock = new object();

    private object eventLock = new object();

    private int added = 0;

    public bool rotationLeft;
    public bool rotationRight;

    

    #endregion

    #region Methods
    void Start()
    {
        AddToRotationEvent();
        touchpadPrevPosition = new Vector2(0,0);
    }
    void Update()
    {
        lock(speedLock)
        {
            if (Speed != 0.0f)
            {
                DecreaseSpeed(decreasingFactor);
                MoveEvent(Speed);
            } 

        }
         
    }
    public void DecreaseSpeed(float decreasingFactor)
    {
        if (Speed < 0.0f)
        {
            Speed += decreasingFactor;
            if (Speed > 0.0f || !rotationRight) Speed = 0.0f;

        }
        else if (Speed > 0.0f)
        {
            Speed -= decreasingFactor;
            if (Speed < 0.0f || !rotationLeft) Speed = 0.0f;
        }   
    }

    public void SpeedStop()
    {
        Speed = 0.0f;
    }

    private void Rotation(Vector2 axis, bool first)
    {
        lock(speedLock)
        {
            if (first)
            {
                touchpadPrevPosition = axis;
            }
            else {
                Speed = axis.x - touchpadPrevPosition.x;
                if (Speed > maxSpeed) Speed = maxSpeed;
                touchpadPrevPosition = axis;
            }
        }
        
            
    }

    public void inverseSpeed()
    {
        lock(speedLock)
        {
            Speed = -Speed;
        }
    }

    public void AddToRotationEvent()
    {
        lock(eventLock)
        {
            added++;
            if (added > 0)
            {
                ControllerTouchpadScript.TouchpadSwipe += Rotation;
            }
        }
    }

    public void RemoveFromRotationEvent()
    {
        lock (eventLock)
        {
            added--;
            if (added == 0)
            {
                ControllerTouchpadScript.TouchpadSwipe -= Rotation;
            }
            
        }
        
    }

    #endregion
}
