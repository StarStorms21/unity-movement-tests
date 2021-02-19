using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{
    public float cameraSpeed = 120f;

    public GameObject cameraTarget;

    Vector3 followPos;
    public float clampYangle = 90f;
    public float inputSensitivity = 150f;
    public GameObject camera;
    public GameObject player;

    public float camDistanceXtoPlayer;
    public float camDistanceYtoPlayer;
    public float camDistanceZtoPlayer;

    public float mX,mY;

    public float finX, finZ;

    public float smouthX, smouthY;

    private float rotY = 0;
    private float rotX = 0;


    void Start() {
        Vector3 rot = transform.rotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate() {
        float inX = Input.GetAxis("Mouse X");
        float inY = Input.GetAxis("Mouse Y");
       
        //add joystick support soon?

        finX = inX;
        finZ = inY;

        rotY += finX * inputSensitivity * Time.deltaTime;
        rotX += -finZ * inputSensitivity * Time.deltaTime;


        rotX = Mathf.Clamp(rotX, -clampYangle, clampYangle);


        Quaternion localRot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = localRot;

        ZoomHandler();
    }


    void LateUpdate() {
        CameraUpdater();
    }

    private void CameraUpdater() {
        Transform target = cameraTarget.transform;

        //move to target
        float step = cameraSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position,target.position, step);
    }

    private void ZoomHandler(){
        float mouseWheel = Input.mouseScrollDelta.y;
        Transform cam = camera.transform;

      //todo lol
    }

}
