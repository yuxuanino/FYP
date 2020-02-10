using UnityEngine;
using System.Collections;

public class Pickupable : MonoBehaviour {

    public float speed;
    Transform position;
    Rigidbody myRB;

    public float thrust;
    Vector3 tVelocity;

    public bool carryable = true;
    public bool isStasis;
    bool hideCube;

    public GameObject stasisEffect;
    public GameObject telekinesisEffect;
    public Vector3 currentPosition;
    Vector3 startPosition;

    AudioSource aS;
    AudioClip sFX;

    public float resetHeight = -200f;
    public float resetDelay = 1.5f;

    public Material baseMaterial;
    public Material fadeMaterial;

    public Coroutine stasisCoroutine;

    public bool isCarried;
    public bool collided;

    public GameObject indicator; //New

    //Object gravity - prevent it being floaty.
    public float gravity = 20f;
    public bool isObjectGravity = false;

    // Use this for initialization
    void Start() {
        startPosition = transform.position;
        myRB = GetComponent<Rigidbody>();
        baseMaterial = GetComponent<Renderer>().material;
        aS = GetComponent<AudioSource>();
        sFX = aS.clip;
        indicator.SetActive(true); //New
    }


    // Update is called once per frame
    void Update() {
        if (transform.localPosition.y <= resetHeight)
        {
            transform.position = startPosition;
        }

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

        if (transform.position.y <= -200)
        {
            transform.position = startPosition;
        }
    }

    public void SetStasis(float duration)
    {
        if (stasisEffect != null)
        {
            stasisEffect.SetActive(true);
            indicator.SetActive(false); //New
            aS.PlayOneShot(sFX);
        }

        else
        {
            Debug.Log("Missing stasisParticle");
        }

        if(stasisCoroutine != null)
        {
            StopCoroutine(stasisCoroutine);
            stasisCoroutine = null;
        }

        stasisCoroutine = StartCoroutine(StasisEnum(duration));
    }

    public void CancelStasis()
    {
        stasisEffect.SetActive(false); //New
        isStasis = false;
        myRB.isKinematic = false;
        StopCoroutine(stasisCoroutine);
        stasisCoroutine = null;
        indicator.SetActive(true); //New
    }

    public void PickUpEffects(bool pickedUp){
        Renderer cR = GetComponent<Renderer>();

        if (pickedUp)
        {
            cR.material = fadeMaterial;
            telekinesisEffect.SetActive(true);
            indicator.SetActive(false); //New
        }
        else
        {
            cR.material = baseMaterial;
            telekinesisEffect.SetActive(false);
            indicator.SetActive(true); //New
        }
    }

    IEnumerator StasisEnum(float duration)
    {
        isStasis = true;
        indicator.SetActive(false); //New
        yield return new WaitForSeconds(duration);
        isStasis = false;
        stasisEffect.SetActive(false); //New
        myRB.isKinematic = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Reset")
        {
            StartCoroutine("Reset");
        }

        if(other.tag == "AntiBox")
        {

            StartCoroutine("Reset");
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.tag != "Player" && other.tag != "Spikes" && other.tag != "PressurePlate")
        {
            collided = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        collided = false;
    }

    IEnumerator Reset()
    {
        carryable = false;
        CancelStasis();
        myRB.useGravity = false;
        myRB.velocity = Vector3.zero;
        GetComponent<Renderer>().material = fadeMaterial;

        yield return new WaitForSeconds(resetDelay);// Whats the reason for this line? Lel I think he can't make it run thats why

        carryable = true;
        transform.position = startPosition;
        myRB.useGravity = true;
        GetComponent<Renderer>().material = baseMaterial;
        indicator.SetActive(true); //New
    }

}
