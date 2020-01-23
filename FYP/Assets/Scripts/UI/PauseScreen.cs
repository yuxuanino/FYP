using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public GameObject pauseScreen;
    public GameObject settingScreen;
    private PlayerTPS pC;

    //Settings
    public GameObject controlSetting;
    public GameObject soundSetting;
    public GameObject graphicSetting;

    public GameObject applyBtn;

    public GameObject resetToDefaultBtn;

    public TestScript TS;

    // Start is called before the first frame update
    void Start()
    {
        pC = FindObjectOfType<PlayerTPS>();
        TS = FindObjectOfType<TestScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if(!pauseScreen.activeInHierarchy)
            {
                PauseGame();
            }

            else
            {
                ContinueGame();
            }
        }
    }

    public void Title()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //Disable scripts that still work while timescale is set to 0
        pC.canMove = false;
    }
    public void ContinueGame()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
        settingScreen.SetActive(false);
        pC.canMove = true;

        if (TS.dialogueIsActive == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //enable the scripts again
    }

    public void Setting()
    {
        settingScreen.SetActive(true);
    }


    //Buttons to get respective settings. ( Control, Sound, Graphic )
    public void ControlBtn()
    {
        controlSetting.SetActive(true);
        soundSetting.SetActive(false);
        graphicSetting.SetActive(false);
        applyBtn.SetActive(true);
        resetToDefaultBtn.SetActive(true);
    }
    public void SoundBtn()
    {
        controlSetting.SetActive(false);
        soundSetting.SetActive(true);
        graphicSetting.SetActive(false);
        applyBtn.SetActive(true);
        resetToDefaultBtn.SetActive(true);
    }
    public void GraphicBtn()
    {
        controlSetting.SetActive(false);
        soundSetting.SetActive(false);
        graphicSetting.SetActive(true);
        applyBtn.SetActive(true);
        resetToDefaultBtn.SetActive(true);
    }

    public void Back()
    {
        settingScreen.SetActive(false);
    }
}
