using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    //public Text progressText;

    public Animator anim;

    public void LoadLevel (int sceneIndex)
    {
        anim.SetTrigger("Fade");
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        yield return new WaitForSeconds(1);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (operation.progress == .9f)
        {
            anim.SetTrigger("Fade");
            yield return new WaitForSeconds(1);
        }

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            //progressText.text = progress * 100f + "%";
        }
    }
}
