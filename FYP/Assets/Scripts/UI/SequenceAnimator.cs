using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceAnimator : MonoBehaviour
{
    //public float WaitBetween;
    //public float WaitEnd;

    //List<Animator> _animators;

    // Start is called before the first frame update
    //void Start()
    //{
    //    _animators = new List<Animator>(GetComponentsInChildren<Animator>());

    //    StartCoroutine(DoAnimation());
    //}

    //IEnumerator DoAnimation()
    //{
    //    while (true)
    //    {
    //        foreach (var animation in _animators)
    //        {
    //            animation.SetTrigger("DoAnimation");
    //            yield return new WaitForSeconds(WaitBetween);
    //        }

    //        yield return new WaitForSeconds(WaitEnd);
    //    }
    //}

    public GameObject UIControls;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        anim.SetTrigger("UIControl");
    }

    private void OnTriggerExit(Collider other)
    {
        UIControls.SetActive(false);
    }
}
