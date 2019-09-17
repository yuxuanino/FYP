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

    void Start()
    {
        StartCoroutine(Type());
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
    }

    IEnumerator Type()
    {
        foreach(char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
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
