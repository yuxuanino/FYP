using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    protected GameObject mainCamera;
    protected bool carrying;
    protected GameObject carriedObject;
    protected RaycastHit stasisHit;
    public float stasisDuration = 60f;
    public float holdDistance = 7.5f;
    public float currentHoldDistance;
    public float throwHoldDistance = 4.0f;
    public float smooth = 5.0f;
    public float throwForce = 10.0f;
    float startYRotation;
    float deltaRotation;
    float yRotation;
    float previousUp;
    Quaternion offset;
    Player360Movement thePlayer;
    PlayerTPS thePlayerTPS;
    Outline outline;

    public bool pickupCollided;

    protected Rigidbody rb;

    void Update()
    {
        pickupCollided = carriedObject.GetComponent<Pickupable>().collided;
    }
    public void Init()
    {
        thePlayerTPS = gameObject.GetComponent<PlayerTPS>();
    }

    protected void CastStasis()
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;
        
        //Sends out a raycast from the main camera.
        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
        int layer_mask = 1 << 8;
        if (Physics.Raycast(ray, out stasisHit, Mathf.Infinity, layer_mask))
        {
            //Object must have this component to be Stasis.
            if (stasisHit.collider.GetComponent<Pickupable>())
            {
                stasisHit.collider.GetComponent<Pickupable>().SetStasis(stasisDuration);
                if (carrying)
                {
                    DropObject();
                }
            }

            if (stasisHit.collider.GetComponent<MovingPlatform>())
            {
                stasisHit.collider.GetComponent<MovingPlatform>().SetStasis(stasisDuration);
            }
            if (stasisHit.collider.GetComponent<RotatingInteractable>())
            {
                stasisHit.collider.GetComponent<RotatingInteractable>().SetStasis(stasisDuration);
            }
            if (stasisHit.collider.GetComponent<DrawBridge>())
            {
                print("wat");
                Debug.Log("Hit = " + stasisHit.transform.name);
                stasisHit.collider.GetComponent<DrawBridge>().SetStasis(stasisDuration);
            }
        }
    }

    //Keeps the carried object in a fixed rotation when the player turns around.
    protected void rotateObject()
    {
        carriedObject.transform.Rotate(5, 10, 15);
    }

    protected void Carry(GameObject o)
    {   
        if (!thePlayerTPS.throwMode)
        {
            //o.transform.position = Vector3.Lerp(o.transform.position, mainCamera.transform.position + mainCamera.transform.forward * currentHoldDistance, Time.deltaTime * smooth);
            o.GetComponent<Rigidbody>().velocity = ((mainCamera.transform.position + mainCamera.transform.forward * currentHoldDistance) - o.transform.position) * smooth;

            //Raycast down to show if it is over PressurePlate. Helpful when far away.
            RaycastHit hit;
            Debug.DrawRay(o.transform.position,-o.transform.up, Color.red);
            if (Physics.Raycast(o.transform.position, -o.transform.up, out hit))
            {  
                PressurePlate p = hit.collider.GetComponent<PressurePlate>();
                Debug.Log(p);
                if (p != null)
                {
                    p.hoverAbove = 0.2f;
                }
            }
        }
        else
        {
            o.transform.position = Vector3.Lerp(o.transform.position, mainCamera.transform.position + mainCamera.transform.forward * throwHoldDistance, Time.deltaTime * smooth);
        }

        if (!carriedObject.GetComponent<Pickupable>().carryable)
        {
            DropObject();
        }

        o.transform.rotation = Quaternion.identity;
           
        deltaRotation = previousUp - mainCamera.transform.eulerAngles.y;
        yRotation = startYRotation - deltaRotation;

        Quaternion target = Quaternion.Euler(0, yRotation, 0);
        o.transform.rotation = Quaternion.Slerp(o.transform.rotation, target, Time.deltaTime * 3);
    }

    //Sends out a raycast check and picks up an object.
    protected void Pickup()
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;
        
        //Sends out a raycast from the main camera.
        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit;

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);

        int layer_mask = 1 << 8;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_mask))
        {
            //Object must have this component to be picked up.
            Pickupable p = hit.collider.GetComponent<Pickupable>();
            p.PickUpEffects(true);
            //p.isStasis = false;
            //p.myRB.isKinematic = false;
            if (p != null && p.carryable)
            {
                //Cancel stasis when the stasis coroutine is cancelled.
                if (p.stasisCoroutine != null)
                {
                    p.CancelStasis();
                }
                //Highlights object when it is picked up.
                carrying = true;
                carriedObject = p.gameObject;
                outline = carriedObject.GetComponent<Outline>();
                outline.enabled = true;
                carriedObject.GetComponent<Pickupable>().isCarried = true;
                //Turns off gravity for carried object.
                p.isObjectGravity = false;
                p.gameObject.GetComponent<Rigidbody>().useGravity = false;
                Physics.IgnoreCollision(GetComponentInParent<Collider>(), carriedObject.GetComponent<Collider>(), true);

                previousUp = mainCamera.transform.eulerAngles.y;
                startYRotation = carriedObject.transform.eulerAngles.y;
            }
        }
    }

    //Applies a force to the carriedObject and throws it.
    protected void ThrowObject()
    {
        carriedObject.GetComponent<Pickupable>().isCarried = false;
        //Turn gravity on again.
        carriedObject.gameObject.GetComponent<Pickupable>().isObjectGravity = true;
        carriedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
        //carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        carriedObject.GetComponent<Rigidbody>().AddForce(mainCamera.transform.forward * (thePlayerTPS.currentChargeTime * throwForce), ForceMode.Impulse);
        Physics.IgnoreCollision(GetComponentInParent<Collider>(), carriedObject.GetComponent<Collider>(), false);
        outline.enabled = false;
        outline = null;
        carriedObject = null;
        carrying = false;
        thePlayerTPS.throwMode = false;
        thePlayerTPS.currentChargeTime = 0;
    }

    //Drops the carriedObject.
    protected void DropObject()
    {
        carriedObject.GetComponent<Pickupable>().isCarried = false;
        carriedObject.GetComponent<Pickupable>().PickUpEffects(false);
        carriedObject.gameObject.GetComponent<Pickupable>().isObjectGravity = true;
        carriedObject.GetComponent<Rigidbody>().useGravity = true;
        carriedObject.GetComponent<Rigidbody>().velocity = new Vector3 (0, 0, 0);
        carriedObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        Physics.IgnoreCollision(GetComponentInParent<Collider>(), carriedObject.GetComponent<Collider>(), false);
        outline.enabled = false;
        outline = null;
        carriedObject = null;
        carrying = false;
        thePlayerTPS.currentChargeTime = 0f;
    }
}
