#region Usings

using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;

#endregion

[RequireComponent(typeof(WorldObjects))]
public class ObjectSpawnerScript : MonoBehaviour {

    #region Private Properties

    private ObjectSpawnerFromGame objectSpawner;

    public delegate void ObjectSpawn();
    public static event ObjectSpawn ObjectSpawned;

    public static GameObject lastSpawned = null;

    #endregion

    #region Methods
    void Start () {
        objectSpawner = new ObjectSpawnerFromGame(gameObject);
    }

    public GameObject SpawnObject(SceneObject objectToSpawn, Vector3 newObjectPosition, Quaternion rotation, Vector3 scale, GameObject controller, bool toSave = false)
    {
        GameObject result = null;
        if (objectToSpawn is LineDrawingObject)
        {
            result = objectSpawner.SpawnLineDrawingObject((LineDrawingObject)objectToSpawn, controller);
        }
        else if (objectToSpawn is ImageObject)
        {
            result = objectSpawner.SpawnImage((ImageObject)objectToSpawn, newObjectPosition, rotation, scale);
        }
        else if (objectToSpawn is ShapeObject)
        {
            result = objectSpawner.SpawnShape((ShapeObject)objectToSpawn, newObjectPosition, rotation, scale);
        }
        else if (objectToSpawn is TextGroupObject)
        {
            result = objectSpawner.SpawnTextGroupObject((TextGroupObject)objectToSpawn, newObjectPosition, rotation, scale);
        }
        else if (objectToSpawn is PhotoSphere)
        {
            result = objectSpawner.SpawnPhotoSphere((PhotoSphere)objectToSpawn, newObjectPosition, rotation, scale);
        }
        else if (objectToSpawn is Shape3DObject)
        {
            result = objectSpawner.SpawnShape3DObject((Shape3DObject)objectToSpawn, newObjectPosition, rotation, scale);
        }
        else
        {
            Debug.Log("Bad option!!");
        }

       

        if (result != null && result.GetComponent<SelectingObjectsScript>() != null)
        {
            controller.GetComponent<ControlObjects>().SetSelected(result);
        }

        if (result != null && toSave)
        {
            SaveManager.SaveGameObject(result);
        }

        if (result.GetComponent<TextGroupScript>() != null)
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform child in result.transform)
            {
                children.Add(child);
            }

            foreach (Transform child in children)
            {
                child.parent = null;
            }

            Destroy(result);
        }

        lastSpawned = result;

        if (result != null && ObjectSpawned != null)
        {
            ObjectSpawned();
        }

        

        return result;

       // result.GetComponent<PhotonView>().RequestOwnership();


    }


    public GameObject SpawnLineObject(string name, int number)
    {
        GameObject line = objectSpawner.SpawnLine(name, number);
        //if (line != null)
        //{
        //    SaveManager.SaveGameObject(line);
        //}
        
        return line;
    }


    public GameObject SpawnDuplicateObject(GameObject pattern, GameObject controller)
    {
        GameObject result = null;
        if (pattern.GetComponent<ImageScript>() != null)
        {
            result = objectSpawner.SpawnImageDuplicate(pattern);
        } else if (pattern.GetComponent<ShapeScript>() != null)
        {
            result = objectSpawner.SpawnShapeDuplicate(pattern);
        }
        else if (pattern.GetComponent<PhotoSphereScript>() != null)
        {
            result = objectSpawner.SpawnPhotoSphereDuplicate(pattern);
        }
        else if (pattern.GetComponent<TextScript>() != null)
        {
            result = objectSpawner.SpawnTextObjectDuplicate(pattern);
        }
        else if (pattern.GetComponent<Shape3DScript>() != null)
        {
            result = objectSpawner.SpawnShape3DObjectDuplicate(pattern);
        }
        else if (Regex.Match(pattern.name, "line[0-9]+").Success)
        {
            result = objectSpawner.SpawnLineObjectDuplicate(pattern);
        }
        else if (pattern.GetComponent<SeparatorScript>() != null)
        {
            result = objectSpawner.SpawnObjectDuplicate(pattern);
        }
        else
        {
            Debug.Log("Bad option!!");
        }

        controller.GetComponent<ControlObjects>().DeleteSelection();

        if (result != null)
        {
            SaveManager.SaveGameObject(result);
           // result.GetComponent<PhotonView>().RequestOwnership();
        }
        return result;

    }

    [PunRPC]
    public void InstantiateOnRemote(string prefabName, int photonVievID,  Vector3 pos, Quaternion rot, Vector3 scale, bool withScaleHandler)
    {
        GameObject oldObj = Resources.Load(prefabName) as GameObject;
        GameObject newObj = (GameObject)GameObject.Instantiate(oldObj, pos, rot);
        if (withScaleHandler)
        {
            Object scaleHandlerPrefab = GetComponent<WorldObjects>().scaleHandlerFor3DObjects;
            GameObject scalehandler = (GameObject)GameObject.Instantiate(scaleHandlerPrefab, pos, rot);
            scalehandler.transform.localScale = newObj.transform.localScale;
            scalehandler.name = "ScaleHandler";
            scalehandler.transform.parent = newObj.transform;
        }

        newObj.transform.localScale = scale;
        newObj.GetComponent<PhotonView>().viewID = photonVievID;
        foreach (Transform child in newObj.transform)
        {
            if (child.GetComponent<TextScript>() != null || child.GetComponent<SeparatorScript>() != null)
            {
                child.GetComponent<PhotonView>().instantiationId = photonVievID + child.GetSiblingIndex() + 1;
                child.GetComponent<PhotonView>().viewID = photonVievID + child.GetSiblingIndex() + 1;
            }
        }


        objectSpawner.AddSetObjectAdditionalFunctions(newObj);


        if (newObj.GetComponent<TextGroupScript>() != null)
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform child in newObj.transform)
            {
                children.Add(child);
                objectSpawner.AddSetObjectAdditionalFunctions(child.gameObject);
            }

            foreach (Transform child in children)
            {
                child.parent = null;
            }

            Destroy(newObj);
        }


    }


    public static void DestroyGameObject(GameObject objToDestroy, bool force = false)
    {
        if (objToDestroy.GetComponent<PhotonView>() != null && (objToDestroy.GetComponent<PhotonView>().isMine || force))
        {
            SaveManager.DeleteFromDatabase(objToDestroy.GetComponent<PhotonView>().viewID);
            if(objToDestroy.GetComponent<ImageMoveScript>() != null)
                {
                    objToDestroy.GetComponent<ImageMoveScript>().DeleteInNetwork();
                }
            if (objToDestroy.GetComponent<PhotoSphereWorldScript>() != null)
            {
                objToDestroy.GetComponent<PhotoSphereWorldScript>().DeleteInNetwork();
            }
        }
        Destroy(objToDestroy);
    }

    #endregion
}
