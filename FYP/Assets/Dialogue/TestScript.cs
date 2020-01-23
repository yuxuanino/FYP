using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public DialogueBase dialogue;
    private DialogueManager dialogueM;
    public bool dialogueIsActive;

    CursorLockMode cursorMode;

    private void Start()
    {
        dialogueIsActive = false;
        var boxCollider = gameObject.AddComponent<BoxCollider>();
        dialogueM = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" || other.tag == "Box")
        {
            //Stop player's movements
            dialogueM.Player.SetActive(false);
            dialogueM.Box1.SetActive(false);
            dialogueM.Box2.SetActive(false);

            //Start dialogue
            DialogueManager.instance.EnqueueDialogue(dialogue);
            dialogueM.nextLineButton.SetActive(true);
            Cursor.lockState = cursorMode = CursorLockMode.None;
            Cursor.visible = true;

            dialogueIsActive = true;

            //Debug.Log("Triggered");
        }

        Destroy(GetComponent<BoxCollider>());
        dialogueIsActive = true;
    }
}
