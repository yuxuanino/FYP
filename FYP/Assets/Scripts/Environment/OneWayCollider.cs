using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OneWayCollider : MonoBehaviour
{
    public Collider toggleableCollider;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Physics.IgnoreCollision(toggleableCollider, other.GetComponent<CapsuleCollider>(), true); //Allows player to pass through as long as they are on the side with the trigger.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(toggleableCollider, other.GetComponent<CapsuleCollider>(), false); //If player leaves the trigger, they can't pass through the collider.
    }
}
