#region Usings

using UnityEngine;

#endregion

public class ToolsButtons : ToolsObject {

    #region Public Properties

    public SceneObject[] sceneObjects;
    public GameObject toolIcon;
    public bool needColorsbar;
    public bool rotation;
    public int textureNumber;

    public int sourceForSceneObjects;

    public int statsNum;

    #endregion

    #region Private Properties
    private string searchText;
    private GameObject searchBox;
    private GameObject keyboard;

    public delegate void SceneObjectsLoaded();
    public event SceneObjectsLoaded newSceneObjectsList;

    private bool searchGoogle;

    private object searchGoogleLock = new object();
    #endregion

    #region Constructors

    public ToolsButtons(SceneObject[] sceneObj, GameObject icon, bool needColorsbar, bool rotation, int statsNumber, int source = 0)
    {
        sceneObjects = sceneObj;
        toolIcon = icon;
        this.needColorsbar = needColorsbar;
        this.rotation = rotation;
        sourceForSceneObjects = source;
        statsNum = statsNumber;
        //LoadSceneObjects();
    }

    public ToolsButtons(GameObject icon, bool needColorsbar)
    {
        sceneObjects = null;
        toolIcon = icon;
        this.needColorsbar = needColorsbar;
        this.rotation = false;
        sourceForSceneObjects = 0;
    }

    #endregion

    #region Methods

    public void LoadSceneObjects(GameObject controller = null)
    {
        if (sourceForSceneObjects > 0 && sourceForSceneObjects < 3)
        {
            keyboard = KeyboardHandlerScript.InitializeKeyboard();
            searchText = "";
            searchBox = KeyboardHandlerScript.InitalizeSearchBox(searchText);
            searchBox.GetComponent<TextMesh>().text = searchText;
            keyboard.GetComponent<KeyboardScript>().NewCharAdded += OnCharAdded;
        }

    }

    public void OnCharAdded(string newChar)
    {
        if (newChar == "back")
        {
            if (searchText.Length > 0)
            {
                searchText = searchText.Substring(0, searchText.Length - 1);
            }
        }
        else if (newChar == "newLine")
        {

        }
        else if (newChar == "done")
        {
            keyboard.GetComponent<KeyboardScript>().NewCharAdded -= OnCharAdded;
            
             if (sourceForSceneObjects == 2)
            {
                SearchGoogle();
            }

        }
        else if (searchText.Length < 30)
        {
            searchText += newChar;
        }

        searchBox.GetComponent<TextMesh>().text = searchText;
    }


    public void SearchGoogle()
    {
        lock (searchGoogleLock)
        {
            searchGoogle = true;
            KeyboardHandlerScript.CloseKeyBoard();
            KeyboardHandlerScript.CloseSearchBox();

            GameObject.Find("Player").transform.FindChild("Toolbar").GetComponent<GoogleCustomSearch>().resultReady += GetResultGoogle;
            GameObject.Find("Player").transform.FindChild("Toolbar").GetComponent<GoogleCustomSearch>().Search(searchText);
        }
    }

    public void GetResultGoogle()
    {
        lock (searchGoogleLock)
        {
            searchGoogle = false;
            GameObject.Find("Player").transform.FindChild("Toolbar").GetComponent<GoogleCustomSearch>().resultReady -= GetResultGoogle;

            sceneObjects = GameObject.Find("Player").transform.FindChild("Toolbar").GetComponent<GoogleCustomSearch>().images.ToArray();
            if (newSceneObjectsList != null)
            {
                newSceneObjectsList();
            }
        }
    }

    public void AbortSearching()
    {
        lock(searchGoogleLock)
        {
            if (searchGoogle)
            {
                searchGoogle = false;
                GameObject.Find("Player").transform.FindChild("Toolbar").GetComponent<GoogleCustomSearch>().resultReady -= GetResultGoogle;
            }
        }
    }

    #endregion

}
