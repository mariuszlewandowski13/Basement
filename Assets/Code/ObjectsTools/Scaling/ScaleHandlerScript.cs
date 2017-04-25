using System.Collections.Generic;
using UnityEngine;

public class ScaleHandlerScript : MonoBehaviour {

    #region Private Properties

    private GameObject parentObject;

    private List<GameObject> resizeObjectsQueue;

    private List<Transform> children;

    public object tempObjectLock = new object();
    private object queueLock = new object();

    private GameObject actualResizeObject = null;

    #endregion

    #region Public Properties

    public Material transparentMaterial;

    #endregion

    #region Methods

    void Start()
    {
        parentObject = gameObject.transform.parent.gameObject;
        resizeObjectsQueue = new List<GameObject>();
        children = new List<Transform>();

        SetChildren();
    }

    public GameObject GetParentObject()
    {
        return parentObject;
    }

    private void SetChildren()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.GetComponent<Renderer>() != null) children.Add(child);
        }
    }

    public GameObject AddNewResizingObject(GameObject newObject)
    {
        lock (queueLock)
        {
            resizeObjectsQueue.Add(newObject);
            if (resizeObjectsQueue.Count == 1)
            {
                parentObject.GetComponent<PhotonView>().RequestOwnership();
                GetComponent<ObjectsFadeScript>().canFade = false;
                CreateResizingImage();
                UpdateChildrenParenting(actualResizeObject.transform);

                if (parentObject.GetComponent<ImageMoveScript>() != null)
                {
                    parentObject.GetComponent<ImageMoveScript>().canMove = false;
                }
            }
        }

        return actualResizeObject;
    }
    public void RemoveResizingObject(GameObject removedObject)
    {
        lock (queueLock)
        {
            resizeObjectsQueue.Remove(removedObject);
            if (resizeObjectsQueue.Count == 0)
            {
                UpdateParentImageObject();
                GetComponent<ObjectsFadeScript>().canFade = true;
                UpdateImageObjectScale();
                UpdateChildrenParenting(transform);
                //UpdateChildrenScale(removedObject);
                PhotonNetwork.Destroy(actualResizeObject);

                if (parentObject.GetComponent<ImageMoveScript>() != null)
                {
                    parentObject.GetComponent<ImageMoveScript>().canMove = true;
                }
            }
        }
    }

    private void CreateResizingImage()
    {
        string name = parentObject.GetComponent<IResizable>().GetResizableObjectPrefabName();
        actualResizeObject = PhotonNetwork.Instantiate(name, parentObject.transform.position, parentObject.transform.rotation, 0, null);

        if (actualResizeObject.GetComponent<IResizable>() != null)
        {
            if (transparentMaterial != null)
            {
                actualResizeObject.GetComponent<IResizable>().SetMaterial(transparentMaterial);
            }
            actualResizeObject.GetComponent<IResizable>().SetModyficableObject(parentObject.GetComponent<IResizable>().GetModyficableObject());
            actualResizeObject.GetComponent<IResizable>().SetModyficableObjectByNetwork();
        }

        actualResizeObject.transform.position = parentObject.transform.position;
        actualResizeObject.transform.rotation = parentObject.transform.rotation;
        actualResizeObject.transform.localScale = parentObject.transform.lossyScale;

        actualResizeObject.transform.Translate(-Vector3.down * 0.005f, parentObject.transform);

        if (actualResizeObject.GetComponent<ImageMoveScript>() != null)
        {
            DestroyImmediate(actualResizeObject.GetComponent<ImageMoveScript>());
        }
        
    }

    private void UpdateParentImageObject()
    {
        parentObject.transform.localScale = actualResizeObject.transform.localScale;
        parentObject.transform.position = actualResizeObject.transform.position;
        parentObject.transform.Translate(Vector3.down * 0.005f, parentObject.transform);
        SaveManager.SaveGameObject(parentObject);
    }

    public void UpdateChildrenParenting(Transform newParent)
    {
        foreach (Transform child in children)
        {
            child.parent = newParent;
        }
    }

    private void UpdateImageObjectScale()
    {
        float width = parentObject.transform.localScale.x;
        float height = parentObject.transform.localScale.z;
        parentObject.GetComponent<IResizable>().UpdateActualRatio(width, height);
    }

    #endregion
}
