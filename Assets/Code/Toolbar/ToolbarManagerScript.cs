#region Usings

using UnityEngine;
using System.Collections.Generic;
using System;

#endregion

public class ToolbarManagerScript : MonoBehaviour {

    #region Public Properties

    public GameObject imagesIcon;
    public GameObject shapeIcon;
    public GameObject spheresIcon;
    public GameObject brushIcon;
    public GameObject Shapes3DIcon;
    public GameObject textsIcon;
    public GameObject googleIcon;
    public GameObject teleport;

    public GameObject keyboard;

    public ToolsButtons[] toolsObjects;
    #endregion

    #region Private Properties

    private SceneObject[] sceneObjects;

    public int leftObjectToFetch;
    public int rightObjectToFetch;
    public short numberOfTiles = 5;

    private GameObject backButton;
    private GameObject colorsBar;

    private bool toolbarRotation;


    private bool filled;
    #endregion

    #region Methods

    void Start()
    {
        backButton = transform.FindChild("buttons").FindChild("BackButton").gameObject;
        backButton.SetActive(false);
        colorsBar = transform.FindChild("paski").gameObject;
        colorsBar.SetActive(false);

        CreateToolsObjects();
        

        MainBar();
        keyboard.SetActive(false);
        
    }

    private void Update()
    {
        if (!filled && ApplicationLoadScript.isLoaded)
        {
            FillToolsObjects();
            MainBar();
            filled = true;
        }
    }


    private void CreateToolsObjects()
    {
        toolsObjects = new ToolsButtons[8];
    }

    private void FillToolsObjects()
    {
        SceneObject[] table = LoadImages();
        SceneObject[] table2 = LoadShapes();
        SceneObject[] table3 = LoadPhotoSpheres();
        SceneObject[] table4 = LoadBrushes();
        SceneObject[] table7 = LoadTextObjects();
        SceneObject[] table8 = Load3DShapes();

        toolsObjects[0] = new ToolsButtons(table, imagesIcon, false, true, 0);
        toolsObjects[1] = new ToolsButtons(table2, shapeIcon, true, true, 1);
        toolsObjects[2] = new ToolsButtons(table3, spheresIcon, false, true,2);
        toolsObjects[3] = new ToolsButtons(table4, brushIcon, true,  false,3);
        toolsObjects[4] = new ToolsButtons(table8, Shapes3DIcon, true, true,4);
        toolsObjects[5] = new ToolsButtons(table7, textsIcon, true, true,5);
        toolsObjects[6] = new ToolsButtons(null, googleIcon, false, true,6, 2);
        toolsObjects[7] = new ToolsButtons(null, teleport, false, false, 9, 1);
    }

    private SceneObject[] LoadImages()
    {
        ImageObject[] imageObjects = null;

        imageObjects = ApplicationStaticData.GetImages();

        return imageObjects;
    }



    private SceneObject[] LoadTextObjects()
    {
        TextGroupObject[] textGroupObjects = new TextGroupObject[GameObject.Find("Player").GetComponent<MiniaturesObjects>().txtMiniatures.Length];
        for (int i = 0; i < textGroupObjects.Length; ++i)
        {
            textGroupObjects[i] = new TextGroupObject(i);
        }
        return textGroupObjects;
    }

    private SceneObject[] LoadShapes()
    {
        ShapeObject[] shapeObjects = null;

        shapeObjects = ApplicationStaticData.GetShapes();

        return shapeObjects;
    }

    private SceneObject[] LoadGrids()
    {
        ShapeObject[] gridsObjects = null;

        gridsObjects = ApplicationStaticData.GetGrids();
        
        return gridsObjects;
    }

    private SceneObject[] LoadPhotoSpheres()
    {
        PhotoSphere[] sphereObjects = null;

        sphereObjects = ApplicationStaticData.GetPhotoSpheres();

        return sphereObjects;
    }

    private SceneObject[] Load3DShapes()
    {
        GameObject[] shapes = GameObject.Find("Player").GetComponent<WorldObjects>().shapes3DObjects;

        Shape3DObject[] shapes3DObjects = new Shape3DObject[shapes.Length];
        for (int i = 0; i < shapes.Length; ++i)
        {
            shapes3DObjects[i] = new Shape3DObject(i);
        }

        return shapes3DObjects;
    }

    private SceneObject[] LoadBrushes()
    {
        SceneObject[] brushes = new SceneObject[6];
        int index = 0;
        brushes[index++] = new LineDrawingObject(Color.red, 0);
        brushes[index++] = new LineDrawingObject(Color.red, 1);
        brushes[index++] = new LineDrawingObject(Color.red, 2);
        brushes[index++] = new LineDrawingObject(Color.red, 3);
        brushes[index++] = new LineDrawingObject(Color.red, 4);
        brushes[index++] = new LineDrawingObject(Color.red, 5);
        return brushes;
    }

    public SceneObject GetNextSceneObject()
    {
        SceneObject objectToReturn = null;
        if (sceneObjects != null)
        {
            if (rightObjectToFetch < sceneObjects.Length)
            {
                objectToReturn = sceneObjects[rightObjectToFetch];
                rightObjectToFetch++;
                leftObjectToFetch++;
                GetComponent<ToolbarRotationScript>().rotationRight = true;
                GetComponent<ToolbarRotationScript>().rotationLeft = true;
            }
            else
            {
                GetComponent<ToolbarRotationScript>().rotationRight = false;
            }
            if (toolbarRotation)
            {
                if (rightObjectToFetch >= sceneObjects.Length) rightObjectToFetch = 0;
                if (leftObjectToFetch >= sceneObjects.Length) leftObjectToFetch = 0;
            }
        }
  
            return objectToReturn;  
    }

    public SceneObject GetPreviousSceneObject()
    {
        SceneObject objectToReturn = null;
        if (sceneObjects != null)
        {
            if (leftObjectToFetch >= 0)
            {
                objectToReturn = sceneObjects[leftObjectToFetch];
                leftObjectToFetch--;
                rightObjectToFetch--;
                GetComponent<ToolbarRotationScript>().rotationLeft = true;
                GetComponent<ToolbarRotationScript>().rotationRight = true;
            }
            else
            {
                GetComponent<ToolbarRotationScript>().rotationLeft = false;
            }

            if (toolbarRotation)
            {
                if (leftObjectToFetch < 0) leftObjectToFetch = sceneObjects.Length - 1;
                if (rightObjectToFetch < 0) rightObjectToFetch = sceneObjects.Length - 1;
            }
        }
        return objectToReturn;
    }

    public SceneObject GetNextSceneObjectOnAppLoading(int number)
    {
        int objectToFetch;
        SceneObject objectToReturn = null;

        if (sceneObjects != null)
        {
            if (toolbarRotation)
            {
                objectToFetch = number % sceneObjects.Length;
            }
            else {
                objectToFetch = number;
            }

            if (objectToFetch < sceneObjects.Length)
            {
                objectToReturn = sceneObjects[objectToFetch];
                rightObjectToFetch++;
            }

            if (toolbarRotation)
            {
                if (rightObjectToFetch >= sceneObjects.Length) rightObjectToFetch = 0;
            }
        }
        return objectToReturn;
    }

    public void LoadNewSceneObjectsList(ToolsButtons tool, bool enableColors)
    {
        if (tool.sceneObjects == null || tool.sceneObjects.Length == 0)
        {
            tool.LoadSceneObjects();
        }

        if (tool.sceneObjects != null)
        {
            toolbarRotation = tool.rotation;

            sceneObjects = tool.sceneObjects;
            rightObjectToFetch = 0;
            if (toolbarRotation)
            {
                leftObjectToFetch = sceneObjects.Length - 1;
            }
            else {
                leftObjectToFetch = -1;
            }


            foreach (Transform child in transform)
            {
                if (child.childCount > 0)
                {
                    if (child.GetChild(0).GetComponent<ToolbarTileMiniatureScript>() != null) child.GetChild(0).GetComponent<ToolbarTileMiniatureScript>().LoadFirstSceneObject();

                }
            }

            if (backButton != null)
            {
                backButton.SetActive(true);
            }

            colorsBar.SetActive(enableColors);

            SetToolbarRotation();
        }
        else  {
            
        }

       
    }

    public void MainBar()
    {
        sceneObjects = toolsObjects;
        leftObjectToFetch = -1;
        rightObjectToFetch = 0;

        foreach (Transform child in transform)
        {
            if (child.childCount > 0)
            {
                if (child.GetChild(0).GetComponent<ToolbarTileMiniatureScript>() != null) child.GetChild(0).GetComponent<ToolbarTileMiniatureScript>().LoadFirstSceneObject();
            }
        }
        colorsBar.SetActive(false);
        backButton.SetActive(false);
        toolbarRotation = false;
        SetToolbarRotation();

    }

    public void LoadTutorialShapes3D()
    {
        LoadNewSceneObjectsList(toolsObjects[4], false);
        backButton.SetActive(false);  
   }


    public Color GetColorFromToolbar()
    {
        Color color = colorsBar.transform.FindChild("panel_color").GetComponent<ToolbarMainColorPickerScript>().color;
        color.a = 1.0f;
        return color;
    }

    public void ClearToolbar()
    {
        foreach (Transform child in transform)
        {
            if (child.childCount > 0)
            {
                if (child.GetChild(0).GetComponent<ToolbarTileMiniatureScript>() != null) child.GetChild(0).GetComponent<ToolbarTileMiniatureScript>().ClearMiniature();
            }
        }
        toolbarRotation = false;
        sceneObjects = null;
        SetToolbarRotation();
        backButton.SetActive(true);

    }


    private void SetToolbarRotation()
    {
        if (sceneObjects == null)
        {
            GetComponent<ToolbarRotationScript>().rotationLeft = false;
            GetComponent<ToolbarRotationScript>().rotationRight = false;
        }else if (!toolbarRotation)
        {
            if (sceneObjects.Length <= numberOfTiles)
            {
                GetComponent<ToolbarRotationScript>().rotationLeft = false;
                GetComponent<ToolbarRotationScript>().rotationRight = false;
            }
            else
            {
                GetComponent<ToolbarRotationScript>().rotationLeft = true;
                GetComponent<ToolbarRotationScript>().rotationRight = true;
            }
        }
        else
        {
            GetComponent<ToolbarRotationScript>().rotationLeft = true;
            GetComponent<ToolbarRotationScript>().rotationRight = true;
        }
    }
    

    #endregion
}
