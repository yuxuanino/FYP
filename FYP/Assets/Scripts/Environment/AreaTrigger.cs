using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public bool playerDetected;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerDetected = true;
            Debug.Log("Player detected = " + playerDetected);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playerDetected = false;
        }
    }
}
