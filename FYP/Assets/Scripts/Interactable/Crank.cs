using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crank : MonoBehaviour
{
    private Quaternion originalRotation;
    private Quaternion currentRotation;

    private float angle;
    public int halfRotation = 0;

    public bool crankTriggered;

    private void Start()
    {
        originalRotation = transform.rotation;

    }

    private void Update()
    {
        currentRotation = transform.rotation;

        angle = Quaternion.Angle(originalRotation, currentRotation);

        if(angle >= 179)
        {
            halfRotation = 1;
        }
        if(halfRotation == 1)
        {
            if(angle <= 1)
            {
                halfRotation = 2;
            }
        }

        if(halfRotation == 2)
        {
            crankTriggered = true;
            print("Full rotation");
        }


    }

}
