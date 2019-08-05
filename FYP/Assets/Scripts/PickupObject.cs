using UnityEngine;
using System.Collections;

public class PickupObject : MonoBehaviour {
	GameObject mainCamera;
	bool carrying;
	GameObject carriedObject;
	public float holdDistance;
	public float smooth;
    public float throwForce = 5.0f;
    public float rayDistance;
    float startYRotation;
    float deltaRotation;
    float yRotation;
    float previousUp;
    Quaternion offset;

    public Pickupable theStasisObject;

    // Use this for initialization
    void Start ()
    {
		mainCamera = GameObject.FindWithTag("MainCamera");
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(carrying)
        {
			Carry(carriedObject);
			CheckDrop();
            ThrowObject();
			//rotateObject();
		}

        else
        {
			pickup();
		}

        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            {
                theStasisObject = hit.collider.GetComponent<Pickupable>();
                theStasisObject.SetStasis(3f);
            }
        }
    }

	void rotateObject()
    {
		carriedObject.transform.Rotate(5,10,15);
	}

	void Carry(GameObject o)
    {
		o.transform.position = Vector3.Lerp (o.transform.position, mainCamera.transform.position + mainCamera.transform.forward * holdDistance, Time.deltaTime * smooth);
		o.transform.rotation = Quaternion.identity;

        deltaRotation = previousUp - mainCamera.transform.eulerAngles.y;
        yRotation = startYRotation - deltaRotation;

        Quaternion target = Quaternion.Euler(0, yRotation, 0);
        o.transform.rotation = Quaternion.Slerp(o.transform.rotation, target, Time.deltaTime * 3);
	}

	void pickup()
    {
		if(Input.GetButtonDown ("Fire1"))
        {
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x,y));
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)) {
				Pickupable p = hit.collider.GetComponent<Pickupable>();
                if (p)
                {
                    p.isStasis = false;
                    p.GetComponent<Rigidbody>().isKinematic = false;
                    if (p != null)
                    {
                        carrying = true;
                        carriedObject = p.gameObject;
                        //p.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        p.gameObject.GetComponent<Rigidbody>().useGravity = false;
                        Physics.IgnoreCollision(GetComponentInParent<Collider>(), carriedObject.GetComponent<Collider>(), true);

                        previousUp = mainCamera.transform.eulerAngles.y;
                        startYRotation = carriedObject.transform.eulerAngles.y;
                    }
                }
			}
		}
	}

	void CheckDrop()
    {
		if(Input.GetButtonDown("Fire1")) {
			DropObject();
		}
	}

    void ThrowObject()
    {
        if (Input.GetButtonDown("Throw"))
        {
            carriedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
            //carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            carriedObject.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce, ForceMode.Impulse);
            Physics.IgnoreCollision(GetComponentInParent<Collider>(), carriedObject.GetComponent<Collider>(), false);
            carriedObject = null;
            carrying = false;
        }
    }

	void DropObject()
    {
		carrying = false;
		//carriedObject.gameObject.rigidbody.isKinematic = false;
		carriedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
        //carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        Physics.IgnoreCollision(GetComponentInParent<Collider>(), carriedObject.GetComponent<Collider>(), false);
        carriedObject = null;
	}
}
