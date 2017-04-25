#region Usings

using UnityEngine;
using System;

#endregion

public class KeyboardScript : MonoBehaviour
{

    #region Private Properties
    private object addingLetterLock = new object();

    #endregion

    #region Events

    public delegate void NewChar(string newChar);
    public event NewChar NewCharAdded;

    public delegate void NewKey(Event ev);
    public event NewKey NewKeyPressed;

    #endregion

    #region Public Properties

    public string lastChar;
    public bool textBoxActive = false;
    private string _text;
    public string text {
        get { return _text; }
        set {
            _text = value;
            KeyboardHandlerScript.searchBox.GetComponent<TextMesh>().text = value;
        }
    }

    #endregion

    #region Methods

    public void AddLetter(string newLetter, KeyCode keyCode)
    {
        lock (addingLetterLock)
        {
            lastChar = newLetter;
            if (NewCharAdded != null)
            {
                NewCharAdded(newLetter);
            }

            if (NewKeyPressed != null && keyCode != KeyCode.None)
            {
                Event newEvent = new Event();
                newEvent.type = EventType.KeyDown;
                newEvent.keyCode = keyCode;

                if (newLetter == "done" || newLetter == "newLine") newEvent.character = '\n';
                else if (newLetter == "back") newEvent.character = (char)0;
                else newEvent.character = (char)keyCode;
               // Debug.Log("KeyboardScript: " + (int)keyCode);
               NewKeyPressed(newEvent);

                if (newLetter != "done")
                {
                    Event buttonUp = new Event();
                    buttonUp.type = EventType.KeyUp;
                    buttonUp.character = newEvent.character;
                    buttonUp.keyCode = keyCode;
                    NewKeyPressed(buttonUp);
                }
                
            }
            if (textBoxActive)
            {
                AddLetterToSearchBox();
            }
        }
    }

    public void ClearSearchBox()
    {
            KeyboardHandlerScript.searchBox.GetComponent<TextMesh>().text = "";
            text = "";
    }

    private void AddLetterToSearchBox()
    {
            if (lastChar == "back")
            {
                if (text.Length > 0)
                {
                    text = text.Substring(0, text.Length - 1);
                }
            }
            else if (lastChar == "newLine" || lastChar == "done")
            {
                text += Environment.NewLine;
            }
            else
            {
                text += lastChar;
            }

            KeyboardHandlerScript.searchBox.GetComponent<TextMesh>().text = text;
    }



    #endregion
}