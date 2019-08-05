    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //Environment Trigger GameObjects - Laser and PressurePlate.
    public enum LockingMechanism {
        pressurePlate, laser
    };
    public LockingMechanism lockingMechanism;

    //Array
    public PressurePlate[] triggersPP;  //Array to hold required PressurePlate
    private bool[] triggersPPBool;      //Bool to check if the PressurePlate is triggered.
    private int triggersPPCount;        //If no. of triggered matches, then something happen
    public Laser[] triggersLaser;
    private bool[] triggersLBool;
    private int triggersLCount;


    public Animator doorAnimator;

    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {


        if (allPressurePlateTriggered() && allLaser())
        {
            doorAnimator.SetBool("doorOpen", true);
        }
        else
        {
            doorAnimator.SetBool("doorOpen", false);
        }
    }

    public bool allPressurePlateTriggered()
    {
        for (int i = 0; i < triggersPP.Length; i++)
        {
            if (triggersPP[i].pressed == false)
            {
                return false;
            }
        }
        return true;
    }

    public bool allLaser()
    {
        for (int i = 0; i < triggersLaser.Length; i++)
        {
            if (triggersLaser[i].laserDetected == false)
            {
                return false;
            }
        }
        return true;

    }
}

