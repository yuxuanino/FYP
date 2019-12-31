using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public DialogueBase dialogue;
    public DialogueManager dialogueM;
    public bool dialogueIsActive;

    //private Player360Movement p360;
    //private PlayerTPS Playertps;
    public GameObject Player;
    CursorLockMode cursorMode;

    //GameObject varGameObject = GameObject.FindWithTag("Player");

    private void Start()
    {
        dialogueIsActive = false;
        var boxCollider = gameObject.AddComponent<BoxCollider>();
        //Playertps = FindObjectOfType<PlayerTPS>();
        dialogueM = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            //Stop player's movements
            Player.SetActive(false);

            //Start dialogue
            DialogueManager.instance.EnqueueDialogue(dialogue);
            dialogueM.nextLineButton.SetActive(true);
            Cursor.lockState = cursorMode = CursorLockMode.None;
            Cursor.visible = true;

            dialogueIsActive = true;

            Debug.Log("Triggered");
        }

        Destroy(GetComponent<BoxCollider>());
    }

}
