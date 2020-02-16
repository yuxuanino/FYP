using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public DialogueBase dialogue;
    private DialogueManager dialogueM;
    private LevelCrystal lvlCrystal;
    public bool dialogueIsActive;

    public GameObject UIinst;
    CursorLockMode cursorMode;

    private void Start()
    {
        dialogueIsActive = false;
        var boxCollider = gameObject.AddComponent<BoxCollider>();
        dialogueM = FindObjectOfType<DialogueManager>();
        lvlCrystal = FindObjectOfType<LevelCrystal>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Time.timeScale = 0.0f;

            //Stop player's movements
            dialogueM.Player.SetActive(false);
            //dialogueM.Box1.SetActive(false);
            //dialogueM.Box2.SetActive(false);

            //Start dialogue
            DialogueManager.instance.EnqueueDialogue(dialogue);
            dialogueM.nextLineButton.SetActive(true);
            Cursor.lockState = cursorMode = CursorLockMode.None;
            Cursor.visible = true;

            UIinst.SetActive(false);

            dialogueIsActive = true;
        }
    }
}
