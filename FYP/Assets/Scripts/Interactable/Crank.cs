using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crank : MonoBehaviour
{
    //private Quaternion originalRotation;
    //private Quaternion currentRotation;

    //private float angle;
    //public int halfRotation = 0;

    public bool crankTriggered;

    //private void Start()
    //{
    //    originalRotation = transform.rotation;

    //}

    //private void Update()
    //{
    //    currentRotation = transform.rotation;

    //    angle = Quaternion.Angle(originalRotation, currentRotation);

    //    if(angle >= 179)
    //    {
    //        halfRotation = 1;
    //    }
    //    if(halfRotation == 1)
    //    {
    //        if(angle <= 1)
    //        {
    //            halfRotation = 2;
    //        }
    //    }

    //    if(halfRotation == 2)
    //    {
    //        crankTriggered = true;
    //        print("Full rotation");
    //    }


    //}
     private Vector3 lastFwd;
     private float curAngleX = 0;
 
     
     void Start () {
     lastFwd = transform.up;
     }
     
     void Update () {
         
         Vector3 curFwd = transform.up;
         // measure the angle rotated since last frame:
         float ang = Vector3.Angle(curFwd, lastFwd);
         if (ang > 0.01){ // if rotated a significant angle...
         // fix angle sign...
         if (Vector3.Cross(curFwd, lastFwd).x < 0) ang = -ang;
         curAngleX += ang; // accumulate in curAngleX...
         lastFwd = curFwd; // and update lastFwd
         }
         
         //print (curAngleX);

        if(curAngleX >= 360)
        {
            crankTriggered = true;

        }
     }
}
