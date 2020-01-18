using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public AudioSource mainMenuMusic;
    public float fadeOutTime = 0.3f;
    bool fadeOut;
    //public Text progressText;

    //public Animator anim;

    void Awake()
    {
        fadeOut = false;
    }

    void Update()
    {
        if (mainMenuMusic.volume != 0 && fadeOut)
        {
            mainMenuMusic.volume -= Time.deltaTime / fadeOutTime;
        }
    }

    public void LoadLevel (int sceneIndex)
    {
        //anim.SetTrigger("Fade");
        fadeOut = true;
        loadingScreen.SetActive(true);
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        yield return new WaitForSeconds(1);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        /*while (operation.progress == .9f)
        {
            //anim.SetTrigger("Fade");
            yield return new WaitForSeconds(1);
        }*/

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            //progressText.text = progress * 100f + "%";
            yield return null;
        }
    }
}
