using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsTutorialHUD : MonoBehaviour
{
    public GameObject[] controlElements;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach(GameObject e in controlElements)
            {
                e.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (GameObject e in controlElements)
            {
                e.SetActive(false);
            }
        }
    }
}
