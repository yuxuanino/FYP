using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public DialogueBase dialogue;

    public GameObject triggerStart;

    private void Start()
    {
        var boxCollider = gameObject.AddComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DialogueManager.instance.EnqueueDialogue(dialogue);
            triggerStart.SetActive(false);
        }
    }

    /*public void TriggerDialogue()
    {
        triggerStart.SetActive(false);
        //Destroy(triggerStart);
    }*/

    /*private void Update()
    {
        //Change this to trigger instead of GetKeyDown
        if (Input.GetMouseButtonDown(0))
        {
            TriggerDialogue();
        }
    }*/


    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            TriggerDialogue();
        }
    }*/
}
