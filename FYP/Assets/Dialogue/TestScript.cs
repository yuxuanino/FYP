using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public DialogueBase dialogue;
    public GameObject triggerStart;
    public bool dialogueIsActive;

    //public PlayerTPS playerTPS;
    //public DialogueManager dM;

    private void Start()
    {
        dialogueIsActive = false;
        var boxCollider = gameObject.AddComponent<BoxCollider>();
        //playerTPS = GetComponent<PlayerTPS>();
        //dM = GetComponent<DialogueManager>();
        dialogueIsActive = false;
        Debug.Log("Dialogue is shit");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DialogueManager.instance.EnqueueDialogue(dialogue);
            triggerStart.SetActive(false);
            dialogueIsActive = true;
            Debug.Log("Triggered");
        }
    }
}
