#region Usings

using UnityEngine;
using System.Collections.Generic;

#endregion

public class ObjectsFadeScript: MonoBehaviour {

    #region Public Properties

    public bool canFade = true;

    #endregion

    #region Private Properties

    private bool fadeIn = false;
    private bool fadeOut = false;

    private object fadeInLock = new object();
    private bool fade = false;

    private List<Transform> children;

    #endregion

    #region Methods

    void Start()
    {
        children = new List<Transform>();
        foreach (Transform child in transform)
        {
            if(child.childCount > 0)children.Add(child);
        }

        FadeOut();

    }


    void Update() {
        lock (fadeInLock)
        {
            if (canFade)
            {
                if (!fade && !fadeOut)
                {
                    fadeIn = false;
                    FadeOut();
                }
                else if (fade && !fadeIn)
                {
                    fadeOut = false;
                    FadeIn();
                }
            }
        }

    }

    private void FadeIn()
    {
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(true);
        }
        fadeIn = true;
        
    }

    private void FadeOut()
    {
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(false);
        }
        fadeOut = true;
        
    }

    public void DecreaseFadeIn()
    {
        lock (fadeInLock) {
            fade = false;
            fadeOut = false;
        }
    }

    public void IncreaseFadeIn()
    {
        lock (fadeInLock)
        {
            fade = true;
            fadeIn = false;
        }
    }
    #endregion
}
