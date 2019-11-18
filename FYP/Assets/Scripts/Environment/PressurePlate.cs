using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private Animation anim;
    public bool pressed = true;

    private float time = 0.7f;
    
    private Vector3 positionA;
    private Vector3 positionB;
    public float distance = 0.35f;

    AudioSource aS;
    public AudioClip pressurePlateSound;
    bool soundPlayed = false;

    //For Telekinesis object to check if it is over this pressurePlate;
    public float hoverAbove = 0;
    public Outline theOutline;

    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
        anim = GetComponent<Animation>();

        positionA = transform.position;
        theOutline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        positionB = new Vector3(positionA.x, positionA.y - distance, positionA.z);
        if (!pressed)
        {
            transform.position = Vector3.Lerp(transform.position, positionA, Time.deltaTime / time);
        }

        //For Telekinesis Carry object to check if it is hover above this PressurePlate. (Shows outline when hover above)
        hoverAbove -= Time.deltaTime;
        if(hoverAbove <= 0f)
        {
            theOutline.enabled = false;
        }else if(hoverAbove >= 0f)
        {
            theOutline.enabled = true;
        }
       
    }


    public void OnTriggerStay(Collider other)
    {
        if(other.tag == "Box")
        {
            theOutline.enabled = true;
            pressed = true;
            transform.position = Vector3.Lerp(transform.position, positionB, Time.deltaTime / time);
            if (aS != null && pressurePlateSound != null)
            {
                if (soundPlayed == false)
                {
                    aS.PlayOneShot(pressurePlateSound);
                    soundPlayed = true;
                }
            }

            else Debug.Log("Missing sound component(s)");
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.tag == "Box")
        {
            pressed = false;

            if (soundPlayed == true) soundPlayed = false;
        }
    }
}
