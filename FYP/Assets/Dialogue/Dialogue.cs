using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public Text textDisplay;
    public string[] sentences; //Hold sentences
    private int index;
    public float typingSpeed;

    public GameObject continueButton;
    //public GameObject skipDialogueButton;
    private bool continueDialogue;  //For skipping dialogue
    //public Animator anim;

    //public Sprite diamond;

    void Start()
    {
        StartCoroutine(Type());
        //diamond = GetComponent<Sprite>();
    }

    void Update()
    {
        /*if(textDisplay.text != sentences[index] && Input.GetMouseButtonDown(0))  //If sentence is not completed and is clicked, complete the sentence
        {
            textDisplay.text = ;
        }*/

        if(textDisplay.text == sentences[index])  //If sentence is completed, continue button will appear
        {
            continueButton.SetActive(true);
        }

        if(continueDialogue == true)
        {
            StopCoroutine(Type());
        }
    }

     IEnumerator Type()
    {
        bool continueDialogue = false;

        while (!continueDialogue)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                continueDialogue = true;
            }
        }

        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        /*if (Input.GetMouseButtonDown(0))
        {
            textDisplay.text = sentences[index];
        }
        Debug.Log(Input.GetMouseButtonDown(0));*/
    }

    public void NextSentence()  //Continue to next sentence of dialogue
    {
        continueButton.SetActive(false);

        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }

        else
        {
            textDisplay.text = "";
            continueButton.SetActive(false);
        }
    }
}
