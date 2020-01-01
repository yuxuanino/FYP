using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public GameObject Player;

    private void Awake()
    {
        //DialogueTS = FindObjectOfType<TestScript>();

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
    public GameObject diamond;
    public GameObject nextLineButton;
    private float delay = 0.001f;

    public bool inDialogue;

    private bool isCurrentlyTyping;
    private string completeText;
    CursorLockMode cursorMode;

    public Queue<DialogueBase.Info> dialogueInfo = new Queue<DialogueBase.Info>();  //Fifo Collection

    //Add information
    public void EnqueueDialogue(DialogueBase db)
    {
        dialogueBox.SetActive(true);
        dialogueInfo.Clear();

        foreach(DialogueBase.Info info in db.dialogueInfo)
        {
            dialogueInfo.Enqueue(info);
        }

        DequeueDialogue();
    }

    //Take out information
    public void DequeueDialogue()
    {
        Debug.Log("Sentence number" + dialogueInfo.Count);

        if (isCurrentlyTyping == true)
        {
            CompleteText();
            StopAllCoroutines();  //Theres only 1 coroutine lol
            isCurrentlyTyping = false;
            diamond.SetActive(true);
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
        diamond.SetActive(false);

        //Text is set to empty
        //dialogueText.text = "";
        //Every letter adds in one by one
        foreach (char c in info.myText.ToCharArray())
        {
            yield return new WaitForSeconds(delay);
            dialogueText.text += c;
        }
        isCurrentlyTyping = false;
        diamond.SetActive(true);
    }

    private void CompleteText()
    {
        dialogueText.text = completeText;
    }

    //When dialogue ends, dialogue will be disabled
    public void EndofDialogue()
    {
        dialogueBox.SetActive(false);
        nextLineButton.SetActive(false);

        Player.SetActive(true);
        Cursor.lockState = cursorMode = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("EndOfDialogue");
        
    }

}
