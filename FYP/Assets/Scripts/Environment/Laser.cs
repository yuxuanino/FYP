using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject reciever;
    public bool laserDetected;

    /*
    LineRenderer lR;
    public Color laserColor = Color.red;
    public float maxLength = 50.0f;
    Transform endEffectTransform;
    public ParticleSystem endEffect;
    Vector3 offset;
    int laserLength;
    Vector3[] laserPosition;
    Transform effectPosition;
    */

    bool audioPlayed = false;

    AudioSource laserSound;
    public AudioClip switchSound;

    // Start is called before the first frame update
    void Start()
    {
        //effectPosition = GetComponentInChildren<Transform>();
        laserDetected = true;
        laserSound = GetComponent<AudioSource>();
        //lR = GetComponentInChildren<LineRenderer>();
        //offset = new Vector3(0, 0, 0);
        //endEffect = GetComponentInChildren<ParticleSystem>();
        //if (endEffect) endEffectTransform = endEffect.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Sends out Raycast to check if it hits the reciever. True if hits.
        RaycastHit hit;  
        if (Physics.Raycast(transform.position, transform.up, out hit, Mathf.Infinity))
        {    
            if (hit.collider.gameObject == reciever)
            {
                laserDetected = true;

                if (audioPlayed == true) audioPlayed = false;
            }
            else
            {
                laserDetected = false;

                if (laserSound != null && switchSound != null)
                {
                    if (audioPlayed == false)
                    {
                        laserSound.PlayOneShot(switchSound);
                        audioPlayed = true;
                    }
                }

                else Debug.Log("Missing sound component(s)");
            }
        }

        //RenderLaser();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.up);
    }

    /*
    void RenderLaser()
    {

        //Shoot our laserbeam forwards!
        UpdateLength();

        //Move through the Array
        for (int i = 0; i < laserLength; i++)
        {
            //Set the position here to the current location and project it in the forward direction of the object it is attached to
            offset.x = effectPosition.position.x + i * effectPosition.forward.x + Random.Range(-noise, noise);
            offset.z = i * effectPosition.forward.z + Random.Range(-noise, noise) + effectPosition.position.z;
            laserPosition[i] = offset;
            laserPosition[0] = effectPosition.position;

            lR.SetPosition(i, laserPosition[i]);

        }


    }

    void UpdateLength()
    {
        //Raycast from the location of the cube forwards
        RaycastHit[] hit;
        hit = Physics.RaycastAll(effectPosition.position, effectPosition.up, maxLength);
        int i = 0;
        while (i < hit.Length)
        {
            //Check to make sure we aren't hitting triggers but colliders
            if (!hit[i].collider.isTrigger || hit[i].collider.tag != "Box" || hit[i].collider.tag != "Player")
            {
                laserLength = (int)Mathf.Round(hit[i].distance) + 2;
                laserPosition = new Vector3[laserLength];
                //Move our End Effect particle system to the hit point and start playing it
                if (endEffect)
                {
                    endEffectTransform.position = hit[i].point;
                    if (!endEffect.isPlaying)
                        endEffect.Play();
                }
                lR.positionCount = laserLength;
                return;
            }
            i++;
        }
        //If we're not hitting anything, don't play the particle effects
        if (endEffect)
        {
            if (endEffect.isPlaying)
                endEffect.Stop();
        }
        laserLength = (int)maxLength;
        laserPosition = new Vector3[laserLength];
        lR.positionCount = laserLength;
    }
    */
}
