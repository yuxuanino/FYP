﻿using System.Collections;
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
            //if (Input.GetKeyDown("space"))
            //{
                continueDialogue = true;
                Debug.Log("Next sentence alr");
            //}
        }

        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()  //Continue to next sentence of dialogue
    {

        //if (Input.GetKeyDown("space"))
        //{
            if (index < sentences.Length - 1)
            {
                index++;
                textDisplay.text = "";
                StartCoroutine(Type());

                Debug.Log("Next sentence ya");
            }

            else
            {
                textDisplay.text = "";
                continueButton.SetActive(false);
            }
        //}
    }
}
