#region Usings

using UnityEngine;
using System.Collections;
using Valve.VR;

#endregion

public class ToolbarMovementScript : MonoBehaviour {
    #region Public Properties

    public float positionYOffset = 0.8589862f;
    Vector3 newPosition;
    public Transform cameraEye;
    private Transform player;

    #endregion
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }
    #region Methods

    void Update()
    {
        newPosition = cameraEye.position;
        newPosition.y = player.position.y + positionYOffset;
        transform.position = newPosition;
    }
    #endregion
}
