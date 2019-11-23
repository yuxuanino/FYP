﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject controlsStuff;
    public GameObject soundStuff;
    public GameObject graphicsStuff;
    public Animator anim;

    public void StartButton()
    {
        StartCoroutine(GoToLoadingScene());
        anim = gameObject.GetComponent<Animator>();
    }

    public IEnumerator GoToLoadingScene()
    {
        anim.SetTrigger("Fade");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("LoadingScene");
    }

    public void SettingsButton()
    {
        settingsMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    //Set every screen false
    public void ReturnMainMenu()
    {
        settingsMenu.SetActive(false);
    }

    //Settings
    public void Controls()
    {
        controlsStuff.SetActive(true);
        soundStuff.SetActive(false);
        graphicsStuff.SetActive(false);
    }

    public void Sound()
    {
        controlsStuff.SetActive(false);
        soundStuff.SetActive(true);
        graphicsStuff.SetActive(false);
    }

    public void Graphics()
    {
        controlsStuff.SetActive(false);
        soundStuff.SetActive(false);
        graphicsStuff.SetActive(true);
    }
}
