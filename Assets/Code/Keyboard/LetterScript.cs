#region Usings

using UnityEngine;

#endregion

public class LetterScript : MonoBehaviour {

    #region Public Properties
    public string character;
    public KeyCode keyCode;
    #endregion

    #region Private Properties
    private Color lightColor;
    private Color darkColor;

    private Renderer _renderer;
    private Renderer renderer2
    {
        get {
            if (_renderer == null)
            {
                _renderer = GetComponent<Renderer>();
                _renderer.material.EnableKeyword("_EMISSION");
            }
            return _renderer;
        }
    }
    #endregion

    #region Methods
    void Start()
    {
        lightColor = new Color(0.6597f, 0.357f, 0.0f);
        darkColor = Color.black;
    }

    void OnEnable()
    {
        EndAnimate();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<TextControllerHead>() != null)
        {
            if (character != "done")
            {
                StartAnimate();
            }
            
            transform.parent.GetComponent<KeyboardScript>().AddLetter(character, keyCode);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.GetComponent<TextControllerHead>() != null)
        {
            EndAnimate();
        }
    }

    private void StartAnimate()
    {
        renderer2.material.SetColor("_EmissionColor", lightColor);
    }

    private void EndAnimate()
    {
        renderer2.material.SetColor("_EmissionColor", darkColor);
    }

    #endregion
}
