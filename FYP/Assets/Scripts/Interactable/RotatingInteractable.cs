using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingInteractable : MonoBehaviour
{
    public float RotationSpeed;
    private Rigidbody rb;

    //private float time = 3f;
    public bool atPoint1 = true;
    public bool isStasis = false;
    public GameObject stasisEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //If not Stasis it will rotate.
        if (!isStasis)
        {
            transform.Rotate(Vector3.right, RotationSpeed);
        }
    }

    public void SetStasis(float duration)
    {
        if (stasisEffect != null)
        {
            var go = Instantiate(stasisEffect, transform.position, Quaternion.identity);
            Destroy(go, 1f);
        }
        else
        {
            Debug.Log("Missing stasisParticle");
        }

        StartCoroutine(StasisEnum(duration));
    }

    IEnumerator StasisEnum(float duration)
    {
        isStasis = true;
        yield return new WaitForSeconds(duration);
        isStasis = false;
    }
}
