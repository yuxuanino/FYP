using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCrystal : MonoBehaviour
{
    public int level;
    public bool lvlCompleted;
    private bool playerInRadius;

    public Image bossDoorRune;

    private GameObject playerGO;
    public Transform hubTP;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lvlCompleted)
        {
            bossDoorRune.color = new Color32(255, 255, 255, 255);
        }

        if(playerInRadius && Input.GetAxisRaw("Interact") == 1)
        {
            print("wat");
            lvlCompleted = true;
            //Animation
            playerGO.transform.position = hubTP.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInRadius = true;
            playerGO = other.transform.gameObject;
        }  
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRadius = false;
        }
    }
}
