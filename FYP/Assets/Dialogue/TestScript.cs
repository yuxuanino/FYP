using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public DialogueBase dialogue;

    public void TriggerDialogue()
    {
        DialogueManager.instance.EnqueueDialogue(dialogue);
    }

    private void Update()
    {
        //Change this to trigger instead of GetKeyDown
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TriggerDialogue();
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            TriggerDialogue();
        }
    }*/
}
