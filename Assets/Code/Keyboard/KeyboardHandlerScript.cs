#region Usings

using UnityEngine;
using System.Collections;

#endregion

public static class KeyboardHandlerScript  {

    #region Private Properties

    private static GameObject keyboard;
    private static GameObject toolbar;
    public static GameObject searchBox;
    public static bool keyboardActive;

    #endregion

    #region Methods
    public static GameObject InitializeKeyboard()
    {
        if (toolbar == null)
        {
            toolbar = GameObject.Find("Toolbar");
        }
       if(keyboard == null)
            {
            keyboard = toolbar.GetComponent<ToolbarManagerScript>().keyboard;
        }
        ShowKeyboard();
        ShowTextControllers();
        InitalizeSearchBox("");
        return keyboard;
    }


    public static void CloseKeyBoard()
    {
        HideKeyboard();
        HideTextControllers();
    }

    private static void ShowKeyboard()
    {
        
        toolbar.SetActive(false);
        keyboard.SetActive(true);
        keyboardActive = true;
    }

    private static void HideKeyboard()
    {
        toolbar.SetActive(true);
        keyboard.SetActive(false);
        keyboardActive = false;
    }


    private static void ShowTextControllers()
    {
        GameObject.Find("Player").transform.FindChild("Controller (left)").FindChild("kontroler nowy").gameObject.SetActive(false);
        GameObject.Find("Player").transform.FindChild("Controller (right)").FindChild("kontroler nowy").gameObject.SetActive(false);
        GameObject.Find("Player").transform.FindChild("Controller (left)").FindChild("ControlObject").gameObject.SetActive(false);
        GameObject.Find("Player").transform.FindChild("Controller (right)").FindChild("ControlObject").gameObject.SetActive(false);

        GameObject.Find("Player").transform.FindChild("Controller (left)").FindChild("controler_text2").gameObject.SetActive(true);
        GameObject.Find("Player").transform.FindChild("Controller (right)").FindChild("controler_text2").gameObject.SetActive(true);
    }

    private static void HideTextControllers()
    {
        GameObject.Find("Player").transform.FindChild("Controller (left)").FindChild("kontroler nowy").gameObject.SetActive(true);
        GameObject.Find("Player").transform.FindChild("Controller (right)").FindChild("kontroler nowy").gameObject.SetActive(true);
        GameObject.Find("Player").transform.FindChild("Controller (left)").FindChild("ControlObject").gameObject.SetActive(true);
        GameObject.Find("Player").transform.FindChild("Controller (right)").FindChild("ControlObject").gameObject.SetActive(true);

        GameObject.Find("Player").transform.FindChild("Controller (left)").FindChild("controler_text2").gameObject.SetActive(false);
        GameObject.Find("Player").transform.FindChild("Controller (right)").FindChild("controler_text2").gameObject.SetActive(false);
    }

    public static GameObject InitalizeSearchBox(string content)
    {
        if (searchBox == null)
        {
            searchBox = keyboard.transform.FindChild("SearchBox").gameObject;
        }
        //searchBox.SetActive(true);
        searchBox.GetComponent<TextMesh>().text = content;
        return searchBox;
    }

    public static void CloseSearchBox()
    {
        searchBox.GetComponent<TextMesh>().text = "";
        
    }

    #endregion

}
