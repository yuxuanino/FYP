using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu;

    // Start is called before the first frame update
    void Start()
    {
        settingsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1);
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
}
