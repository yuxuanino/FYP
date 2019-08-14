using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBridge : MonoBehaviour
{
    //Environment Triggers
    public PressurePlate[] triggersPP;  //Array to hold required PressurePlate
    private bool[] triggersPPBool;      //Bool to check if the PressurePlate is triggered.
    private int triggersPPCount;        //If no. of triggered matches, then something happen
    public Laser[] triggersLaser;
    private bool[] triggersBool;
    private int triggersLCount;

    public Animator anim;

    //Moving point.
    public GameObject point1;
    public GameObject point2;

    //Stasis.
    private float time = 3f;
    public bool isStasis = false;
    public GameObject stasisEffect;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (allPressurePlateTriggered() && allLaser())
        {
            //anim.SetBool("bridgeDraw", true);
            transform.position = Vector3.MoveTowards(transform.position, point2.transform.position, Time.deltaTime * time);
        }
        else
        {
            //anim.SetBool("bridgeDraw", false);
            transform.position = Vector3.MoveTowards(transform.position, point1.transform.position, Time.deltaTime * time);
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
