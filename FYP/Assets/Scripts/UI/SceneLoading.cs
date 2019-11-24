using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    public Animator anim;
    [SerializeField]
    private Image progressBar;

    // Start is called before the first frame update
    void Start()
    {
        //Start async operation
        anim.SetTrigger("FadeToDia");
        StartCoroutine(LoadAsyncOperation());
        anim = gameObject.GetComponent<Animator>();
    }

    IEnumerator LoadAsyncOperation()
    {
        //Create an async operation
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(3);

        while (gameLevel.progress < 1)
        {
            //Take the progress bar fill = async operation progress.
            progressBar.fillAmount = gameLevel.progress;
            yield return new WaitForSeconds(10);
        }
    }
}
