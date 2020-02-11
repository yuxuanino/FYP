using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelDoor : MonoBehaviour
{
    public AreaTrigger areaTrigger;

    //Target point to move towards.
    public GameObject point1;
    public GameObject point2;

    private bool lvlCompletedd;


    void Update()
    {
        //if (lvlCrystal.lvlCompleted == true)
        //{         
        //    transform.position = Vector3.MoveTowards(transform.position, point1.transform.position, Time.deltaTime * 3f);
        //}
        //if(areaTrigger.playerDetected == true)
        //{ 
        //    transform.position = Vector3.MoveTowards(transform.position, point2.transform.position, Time.deltaTime * 3f);
        //}

        if (areaTrigger.playerDetected == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, point1.transform.position, Time.deltaTime * 3f);
        }
        else if (areaTrigger.playerDetected == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, point2.transform.position, Time.deltaTime * 3f);
        }

    }

}
