using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeDoor : MonoBehaviour
{
    public GameObject pPlate1;
    public GameObject pPlate2;
    public GameObject pPlate3;

    public GameObject exitDoorPoint;

    bool audioPlayed = false;

    AudioSource doorSound;
    public AudioClip openSound;

    private float smooth = 0.5f;

    PressurePlate pPlate1cs;
    PressurePlate pPlate2cs;
    PressurePlate pPlate3cs;

    // Start is called before the first frame update
    void Start()
    {
        pPlate1cs = pPlate1.GetComponent<PressurePlate>();
        pPlate2cs = pPlate2.GetComponent<PressurePlate>();
        pPlate3cs = pPlate3.GetComponent<PressurePlate>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pPlate1cs.pressed && pPlate2cs.pressed && pPlate3cs.pressed)
        {
            transform.position = Vector3.Lerp(transform.position, exitDoorPoint.transform.position, Time.deltaTime / smooth);

            if (doorSound != null && openSound != null)
            {
                if (audioPlayed == false)
                {
                    doorSound.PlayOneShot(openSound);
                    audioPlayed = true;
                }
            }
        }

    }
}
