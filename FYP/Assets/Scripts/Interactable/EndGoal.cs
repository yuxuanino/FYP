using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGoal : MonoBehaviour
{
    public GameObject endScreen;
    public GameObject reticleHolder;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerTPS playerScript = other.GetComponent<PlayerTPS>();
            playerScript.canMove = false;
            reticleHolder.SetActive(false);

            endScreen.SetActive(true);
        }
    }
}
