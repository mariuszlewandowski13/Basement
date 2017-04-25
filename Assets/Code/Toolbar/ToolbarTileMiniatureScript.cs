#region Usings

using UnityEngine;
using System.Collections;
using Valve.VR;


#endregion

public class ToolbarTileMiniatureScript : MonoBehaviour {

    #region Private Properties

    private GameObject toolbar;
    private GameObject miniature;

    #endregion

    #region Public Properties

    public int number;

    #endregion
    void Awake()
    {
        toolbar = gameObject.transform.parent.parent.gameObject;
    }


    void Start () {
        
        //LoadFirstSceneObject();
        ToolbarTileRotationScript.Decrease += DecreaseNumber;
        ToolbarTileRotationScript.Increase += IncreaseNumber;
    }

    public GameObject GetRightSceneObject()
    {
        SceneObject newSceneObject = toolbar.GetComponent<ToolbarManagerScript>().GetNextSceneObject();
        if (miniature != null)
        {
            if (miniature.GetComponent<MiniatureSpawnScript>() != null) miniature.GetComponent<MiniatureSpawnScript>().parent = null;
            if (miniature.GetComponent<PhotoSphereMiniatureWorldScript>() != null) miniature.GetComponent<PhotoSphereMiniatureWorldScript>().parent = null;
        }
        if (newSceneObject != null) 
        {
            miniature = ToolbarMiniatureManager.CreateMiniature(gameObject, newSceneObject);

        }
        else miniature = null;
        return miniature;      
    }

    public GameObject GetLeftSceneObject()
    {
        SceneObject newSceneObject = toolbar.GetComponent<ToolbarManagerScript>().GetPreviousSceneObject();
        if (miniature != null)
        {
            if (miniature.GetComponent<MiniatureSpawnScript>() != null) miniature.GetComponent<MiniatureSpawnScript>().parent = null;
            if (miniature.GetComponent<PhotoSphereMiniatureWorldScript>() != null) miniature.GetComponent<PhotoSphereMiniatureWorldScript>().parent = null;
        }


        if (newSceneObject != null)
        {
            miniature = ToolbarMiniatureManager.CreateMiniature(gameObject, newSceneObject);
        } 
        else miniature = null;

        return miniature;
    }

    public void LoadFirstSceneObject()
    {
        if (miniature != null)
        {
           // Debug.Log(miniature.name);
            if (miniature.GetComponent<MiniatureSpawnScript>() != null) miniature.GetComponent<MiniatureSpawnScript>().parent = null;
            if (miniature.GetComponent<PhotoSphereMiniatureWorldScript>() != null) miniature.GetComponent<PhotoSphereMiniatureWorldScript>().parent = null;
        }
        SceneObject newSceneObject = toolbar.GetComponent<ToolbarManagerScript>().GetNextSceneObjectOnAppLoading(number);
        if (newSceneObject != null)
        {
            miniature = ToolbarMiniatureManager.CreateMiniature(gameObject, newSceneObject);
        }
        else
        {
            if (transform.childCount > 0)
            {
                GameObject.Destroy(transform.Find("TileMiniature").gameObject);
            }
            miniature = null;
        }
    }

    public void IncreaseNumber()
    {
        number++;
        if (number >= toolbar.GetComponent<ToolbarManagerScript>().numberOfTiles) number = 0;
    }

    public void DecreaseNumber()
    {
        number--;
        if (number < 0) number = toolbar.GetComponent<ToolbarManagerScript>().numberOfTiles - 1;
    }

    void OnDestroy()
    {
        ToolbarTileRotationScript.Decrease -= DecreaseNumber;
        ToolbarTileRotationScript.Increase -= IncreaseNumber;
    }

    public void ClearMiniature()
    {
        if (miniature != null)
        {
            Destroy(miniature);
        }

        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

    }

}
