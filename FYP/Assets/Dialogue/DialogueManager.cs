using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Fix this shiiii" + gameObject.name);
        }

        else
        {
            instance = this;
        }
    }

    public GameObject dialogueBox;

    public Text dialogueName;
    public Text dialogueText;
    public Image dialoguePortrait;
    private float delay = 0.001f;

    //Options stuff
    //private bool isDialogueOption;
    //public GameObject dialogueOptionUI;
    public bool inDialogue;
    //public GameObject[] optionButtons;
    //private int optionsAmount;
    //public Text questionText;

    private bool isCurrentlyTyping;
    private string completeText;

    public Queue<DialogueBase.Info> dialogueInfo = new Queue<DialogueBase.Info>();  //Fifo Collection

    //Add information
    public void EnqueueDialogue(DialogueBase db)
    {

        //if (inDialogue) return;
        dialogueBox.SetActive(true);
        dialogueInfo.Clear();

        //OptionsParser(db);

        foreach(DialogueBase.Info info in db.dialogueInfo)
        {
            dialogueInfo.Enqueue(info);
        }

        DequeueDialogue();
    }

    //Take out information
    public void DequeueDialogue()
    {
        if (isCurrentlyTyping == true)
        {
            CompleteText();
            StopAllCoroutines();  //Theres only 1 coroutine lol
            isCurrentlyTyping = false;
            return; //Don't want to deque the dialogue
        }

        //Detects if we have no more dialogue and return
        if (dialogueInfo.Count == 0)
        {
            EndofDialogue();
            return;
        }

        DialogueBase.Info info = dialogueInfo.Dequeue();
        completeText = info.myText;

        dialogueName.text = info.myName;
        dialogueText.text = info.myText;
        dialoguePortrait.sprite = info.portrait;

        dialogueText.text = "";  //Clear text

        StartCoroutine(TypeText(info));
    }

    IEnumerator TypeText(DialogueBase.Info info)
    {
        isCurrentlyTyping = true;

        //Text is set to empty
        //dialogueText.text = "";
        //Every letter adds in one by one
        foreach (char c in info.myText.ToCharArray())
        {
            yield return new WaitForSeconds(delay);
            dialogueText.text += c;
            //yield return null;
        }
        isCurrentlyTyping = false;
    }

    private void CompleteText()
    {
        dialogueText.text = completeText;
    }

    //When dialogue ends, dialogue will be disabled
    public void EndofDialogue()
    {
        dialogueBox.SetActive(false);
        OptionsLogic();
    }

    private void OptionsLogic()
    {
        //if (isDialogueOption == true)
        //{
        //    dialogueOptionUI.SetActive(true);
        //}
        //else
        //{
            inDialogue = false;  //Can't open dialogue while in options mode
        //}
    }

    /*private void OptionsParser(DialogueBase db)
    {

    }*/
}
