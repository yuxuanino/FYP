using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject point1;
    public GameObject point2;

    private float time = 3f;
    public bool atPoint1 = true;
    public bool isStasis = false;
    public GameObject stasisEffect;
    
    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStasis)
        {
            if (atPoint1)
            {
                transform.position = Vector3.MoveTowards(transform.position, point2.transform.position, Time.deltaTime * time);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, point1.transform.position, Time.deltaTime * time);
            }
        }
    }

    public void OnTriggerEnter (Collider other)
    {
        if (other.tag == "PlatformPoint")
        {
            
            if (atPoint1)
            {
                atPoint1 = false;
            }else
            {
                atPoint1 = true;
            }
        }

        if(other.tag == "Player")
        {
            other.transform.parent = gameObject.transform;
        }

        if(other.tag == "Box")
        {
            other.transform.parent = gameObject.transform;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
        }

        if (other.tag == "Box")
        {
            other.transform.parent = null;
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
