using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstableFloor : MonoBehaviour
{
    Rigidbody rb;
    public Collider floorCollider;
    public float destroyDelay = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Fall();
        }
    }

    void Fall()
    {
        rb.useGravity = true;
        floorCollider.isTrigger = true;
        Destroy(this, destroyDelay);
    }
}
