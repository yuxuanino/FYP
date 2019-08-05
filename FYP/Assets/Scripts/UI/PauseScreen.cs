using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public GameObject pauseScreen;
    private PlayerController pC;

    // Start is called before the first frame update
    void Start()
    {
        pC = FindObjectOfType<PlayerController>();
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
    private void ContinueGame()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
        pC.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //enable the scripts again
    }

}
