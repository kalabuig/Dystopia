using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private CameraBehavior cameraBehavior;
    private Transform playerTransform;

    private float zoom=100f; //Zoom level
    private float zoomSpeed = 150f; //Zoom in and out speed
    private float minZoom = 40f; //More close zoom
    private float maxZoom = 200f; //More far zoom
    private float zoomWheelSensibility = 20f;
    
    private void Awake() {
        cameraBehavior = Camera.main.GetComponent<CameraBehavior>();
        playerTransform = GameObject.Find("Player").transform;
    }
    
    void Start()
    {
        cameraBehavior.setFocus(() => playerTransform.position);
        cameraBehavior.setZoom(() => zoom);
    }

    private void Update() {
        HandleZoom();
    }

    private void HandleZoom() {
        //Keyboard zoom control
        if(Input.GetKey(KeyCode.KeypadPlus)) { //Zoom in
            zoom -= zoomSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.KeypadMinus)) { //Zoom out
            zoom += zoomSpeed * Time.deltaTime;
        };
        //Mouse wheel zoom control
        if(Input.mouseScrollDelta.y > 0) {
            zoom -= zoomSpeed * Time.deltaTime * zoomWheelSensibility;
        }
        if(Input.mouseScrollDelta.y < 0) {
            zoom += zoomSpeed * Time.deltaTime * zoomWheelSensibility;
        }
        //Zoom limits
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
    }
}
