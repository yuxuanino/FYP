using UnityEngine;
using System.Collections;

public class Pickupable : MonoBehaviour {

    public float speed;
    Transform position;
    public Rigidbody myRB;

    public float thrust;
    Vector3 tVelocity;

    public bool isStasis;

    public GameObject stasisEffect;
    public Vector3 currentPosition;

    public Coroutine stasisCoroutine;

    // Use this for initialization
    void Start () {
        myRB = GetComponent<Rigidbody>();
    }
	

	// Update is called once per frame
	void Update () {

        if (isStasis == false)
        {
            if (tVelocity != Vector3.zero)
            {
                myRB.velocity = tVelocity;
                tVelocity = Vector3.zero;
                myRB.isKinematic = false;
            }
        } else { 
            if (tVelocity == Vector3.zero)
            {
                tVelocity = myRB.velocity;
            }
            myRB.velocity = Vector3.zero;
            myRB.isKinematic = true;
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

        stasisCoroutine = StartCoroutine(StasisEnum(duration));
    }

    public void CancelStasis()
    {
        isStasis = false;
        myRB.isKinematic = false;
        StopCoroutine("StasisEnum");
        stasisCoroutine = null;
    }

    IEnumerator StasisEnum(float duration)
    {
        isStasis = true;
        yield return new WaitForSeconds(duration);
        isStasis = false;
    }
}
