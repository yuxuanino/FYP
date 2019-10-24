using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public float mouseSensitivity = 10f;
    public float dstFromTarget = 2;
    public Vector2 pitchMinMax = new Vector2(-20, 85);

    public float rotationSmoothTime = 0.12f;
    private Vector3 rotationSmoothVelocity;
    public Vector3 currentRotation;

    public float yaw;
    public float pitch;

    //Camera Throw mode position.
    public Player360Movement thePlayer;
    public Transform walkMode;
    public Transform throwModePos;

    //Throw Mode camera setting.
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    //private float yawThrow = 0.0f;
    //private float pitchThrow = 0.0f;
    public float cameraMaxY = 90f;
    public float cameraMinY = -90f;

    public float switchSpeed = 4;


    void Start()
    {
        Player360Movement thePlayer = GetComponent<Player360Movement>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!thePlayer.throwMode)//Walk Mode
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
            transform.eulerAngles = currentRotation;
            transform.position = walkMode.position - transform.forward * dstFromTarget;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, throwModePos.position, switchSpeed * Time.deltaTime);
            
        }
    }
}
