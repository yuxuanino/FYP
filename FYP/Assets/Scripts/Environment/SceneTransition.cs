using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("Create a collider as a trigger and put in scene index number you want to transition to")]
    [Space]
    [Header("Check build settings for scene index numbers")]
    public int sceneToTransitionTo;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToTransitionTo);
        }
    }
}
