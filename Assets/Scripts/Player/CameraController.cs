
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    // enable other scripts to access this script
    public static CameraController instance;

    // reference to the camera's rig position transform component
    public Transform cameraRigPosition;

    private float startFOV;
    private float targetFOV;

    public float zoomSpeed = 1f;

    // reference to the player camera
    public Camera playerCamera;




    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        startFOV = playerCamera.fieldOfView;

        targetFOV = startFOV;
    }


    // move the camera
    void LateUpdate()
    {
        MoveCamera();
    }


    private void MoveCamera()
    {
        // set the camera position to the position of the camera rig
        transform.position = cameraRigPosition.position;

        // set the camera rotation to the rotation of the camera rig
        transform.rotation = cameraRigPosition.rotation;


        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
    }


    public void ZoomIn(float newZoom)
    {
        targetFOV = newZoom;
    }


    public void ZoomOut()
    {
        targetFOV = startFOV;
    }


} // end of class
