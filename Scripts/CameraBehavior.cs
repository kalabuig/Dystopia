using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private Camera cam;
    private Func<Vector3> GetCamFocusPosFunc;
    private Func<float> GetCamZoomFunc;

    private float camMoveSpeed = 2f;
    private float camZoomSpeed = 1f;

    //Method to set the focus of the camera
    public void setFocus(Func<Vector3> camFocusPos) {
        this.GetCamFocusPosFunc = camFocusPos;
    }

    public void setZoom(Func<float> camZoomFunc) {
        this.GetCamZoomFunc = camZoomFunc;
    }

    private void Start() {
        cam = transform.GetComponent<Camera>();
    }

    void Update()
    {
        HandleCamMovement();
        HandleCamZoom();
    }

    private void HandleCamMovement() {
        //Getting the focus of the camera
        Vector3 camFocusPos = GetCamFocusPosFunc(); 
        camFocusPos.z = transform.position.z;
        //Smoothing the movement of the camera
        Vector3 camMoveDir = (camFocusPos - transform.position).normalized;
        float distance = Vector3.Distance(camFocusPos, transform.position);
        //Taking care of low framerate issues with the camera
        if(distance > 0) {
            Vector3 newCamPos = transform.position + (camMoveDir * distance * camMoveSpeed * Time.deltaTime);
            float distAfterMoving = Vector3.Distance(newCamPos, camFocusPos);
            if(distAfterMoving > distance) { // It's a problem, take care of it
                newCamPos = camFocusPos;
            }
            transform.position = newCamPos; //Moving the camera
        }
    }

    private void HandleCamZoom() {
        float camZoom = GetCamZoomFunc();

        float camZoomDiff = camZoom - cam.orthographicSize;

        cam.orthographicSize += camZoomDiff * camZoomSpeed * Time.deltaTime;

        if(camZoomDiff > 0) {
            if(cam.orthographicSize > camZoom) { //Checking Zoom overshot (to correct low framerate issues)
                cam.orthographicSize = camZoom;
            }
        }
    }
}
