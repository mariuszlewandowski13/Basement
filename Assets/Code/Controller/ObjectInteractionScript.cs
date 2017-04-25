#region Using
using UnityEngine;
#endregion

public class ObjectInteractionScript : MonoBehaviour
{
    #region Public Events

    public delegate void Interaction(GameObject controller, bool enter);
    public event Interaction ControllerCollision;

    #endregion

    #region Private Properties

    public int isSelected = 0;
    public bool collision = false;

    private GameObject collidingController;

    //locks
    object isSelectedLock = new object();
    object collisionLock = new object();
    object controllerLock = new object();
    #endregion

    #region Methods

    public void SetIsSelected(bool value)
    {
        lock(isSelectedLock)
        {
            lock(collisionLock)
            {
                if (value) isSelected++;
                else isSelected--;
                if (isSelected < 0) isSelected = 0;
                if (ControllerCollision != null)
                {
                    ControllerCollision(collidingController, collision);
                }
            }
            
        }
    }

    public bool GetIsSelected()
    {
        bool result;
        lock (isSelectedLock)
        {
            result = (isSelected > 0 ? true : false);
        }
        return result;
    }

    public void SetCollision(bool value, GameObject collidingController)
    {
        lock (collisionLock)
        {
            collision = value;
            lock (controllerLock)
            {
                if (collision)
                {
                    this.collidingController = collidingController; 
                }
                if (ControllerCollision != null)
                {
                    ControllerCollision(collidingController, collision);
                }
            }
            
        }
    }

    public bool GetCollision()
    {
        bool result;
        lock (collisionLock)
        {
            result = collision;
        }
        return result;
    }

    public void Reset()
    {
        lock (collisionLock)
        {
            collision = false;
        }
        lock (isSelectedLock)
        {
            isSelected = 0;
        }
    }

    #endregion
}
