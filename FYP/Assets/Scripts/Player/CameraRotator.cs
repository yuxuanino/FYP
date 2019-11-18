using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{


    public static CameraRotator instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void RotateAroundCameraRotator(float x, float y, float z)
    {
         instance.transform.Rotate(x, y, z);
    }

    public static void RotateCameraRotator(float x, float y, float z)
    {
        instance.transform.localEulerAngles = new Vector3(x, y, z);
    }
}
