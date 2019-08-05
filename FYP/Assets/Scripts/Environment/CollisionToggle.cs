using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionToggle : MonoBehaviour
{
    public GameObject laserOne;
    public GameObject laserTwo;

    Collider thisCollider;

    Laser laser1;
    Laser laser2;

    // Start is called before the first frame update
    void Start()
    {
        laser1 = laserOne.GetComponent<Laser>();
        laser2 = laserTwo.GetComponent<Laser>();
    }

    // Update is called once per frame
    void Update()
    {
        if (laser1.laserDetected == false && laser2.laserDetected == false)
        {
            GetComponent<Collider>().enabled = false;
        }
    }
}
