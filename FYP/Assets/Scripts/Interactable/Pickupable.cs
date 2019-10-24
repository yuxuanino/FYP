using UnityEngine;
using System.Collections;

public class Pickupable : MonoBehaviour {

    public float speed;
    Transform position;
    Rigidbody myRB;

    public float thrust;
    Vector3 tVelocity;

    public bool isStasis;
    bool hideCube;

    public GameObject stasisEffect;
    public Vector3 currentPosition;

    public Material baseMaterial;
    public Material fadeMaterial;

    public Coroutine stasisCoroutine;


    //Object gravity - prevent it being floaty.
    public float gravity = 20f;
    public bool isObjectGravity = false;

    // Use this for initialization
    void Start() {
        myRB = GetComponent<Rigidbody>();
        baseMaterial = GetComponent<Renderer>().material;
    }


    // Update is called once per frame
    void Update() {

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

        if (isObjectGravity)
        {
           myRB.AddForce(new Vector3(0, -gravity * myRB.mass, 0));
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

    public void Transparency(bool pickedUp){
        Renderer cR = GetComponent<Renderer>();

        if (pickedUp == true)
        {
            cR.material = fadeMaterial;
        }

        else if (pickedUp == false) cR.material = baseMaterial;
        }

    IEnumerator StasisEnum(float duration)
    {
        isStasis = true;
        yield return new WaitForSeconds(duration);
        isStasis = false;
    }
}
