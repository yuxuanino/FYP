using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    protected GameObject mainCamera;
    protected bool carrying;
    protected GameObject carriedObject;
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

    // Start is called before the first frame update
    void Awake()
    {
        
    }
    public void Init()
    {
        thePlayer = gameObject.GetComponent<Player360Movement>();
        thePlayerTPS = gameObject.GetComponent<PlayerTPS>();
    }

    protected void CastStasis()
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;
        
        //Sends out a raycast from the main camera.
        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit;
        int layer_mask = 1 << 8;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_mask))
        {
            //Object must have this component to be Stasis.
                if (hit.collider.GetComponent<Pickupable>() != null)
            {
                
                hit.collider.GetComponent<Pickupable>().SetStasis(5f);
                if (carrying)
                    {
                        DropObject();
                    } 
            }

            if(hit.collider.GetComponent<MovingPlatform>() != null)
            {
                hit.collider.GetComponent<MovingPlatform>().SetStasis(5f);
            }
            if(hit.collider.GetComponent<RotatingInteractable>() != null)
            {
                hit.collider.GetComponent<RotatingInteractable>().SetStasis(5f);
            }
            if(hit.collider.GetComponent<DrawBridge>() != null)
            {
                print("wat");
                Debug.Log("Hit = " + hit.transform.name);
                hit.collider.GetComponent<DrawBridge>().SetStasis(5f);
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
            o.transform.position = Vector3.Lerp(o.transform.position, mainCamera.transform.position + mainCamera.transform.forward * currentHoldDistance, Time.deltaTime * smooth);

            //Raycast down to show if it is over PressurePlate. Helpful when far away.
            RaycastHit hit;
            Debug.DrawRay(o.transform.position,-o.transform.up, Color.red);
            if (Physics.Raycast(o.transform.position, -o.transform.up, out hit))
            {
                PressurePlate p = hit.collider.GetComponent<PressurePlate>(); 
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
            //p.isStasis = false;
            //p.myRB.isKinematic = false;
            if (p != null)
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

                //Turns off gravity for carried object.
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
        //Turn gravity on again.
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
        carriedObject.GetComponent<Pickupable>().Transparency(false);
        carriedObject.GetComponent<Rigidbody>().useGravity = true;
        carriedObject.GetComponent<Rigidbody>().velocity = new Vector3 (0, 0, 0);
        //carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        Physics.IgnoreCollision(GetComponentInParent<Collider>(), carriedObject.GetComponent<Collider>(), false);
        outline.enabled = false;
        outline = null;
        carriedObject = null;
        carrying = false;
        thePlayerTPS.currentChargeTime = 0f;
    }
}
