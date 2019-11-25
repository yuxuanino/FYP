using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTPS : PlayerAbilities
{
    public float speed = 6.0f;
    public float gravity = 10.0f;
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 1.3f;
    public float groundCheckDistance = 0.1f;

    [SerializeField]
    bool isGrounded;
    public bool canMove;
    private CapsuleCollider pCollider;
    Vector3 colliderPosition;
    float colliderRadius;
    Vector3 safeSpot;
    public float sUpdateInterval = 3f;
    public float resetHeight = 15600f;

    LayerMask playerLayer;

    //Player direction
    public float turnSmoothTime = 0.2f;
    private float turnSmoothVelocity;
    private Transform cameraT;
    float targetRotation;
    CursorLockMode cursorMode;
    Rigidbody rb;

    //Player ThrowMode camera view.
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    public float cameraMaxY = 90f;      //Camera highest angle.
    public float cameraMinY = -25f;     //Camera lowest angle.

    //Switch between default or ThrowMode. Default when walking around. Throw mode for when tossing object.
    public bool throwMode;
    public float chargeDuration;
    public float currentChargeTime;

    //Animations
    public Animator anim;
    private float inputH;
    private float inputV;
    private bool jump;

    //Camera
    public GameObject throwCamera;      //Camera Position when throwing.
    public GameObject walkCamera;       //Camera Position when walking !throwing.
    public GameObject zoomInCamera;     //Camera Position when it hits a wall. (Prevent seeing through walls/floor)
    public GameObject camReverseCheck;  //Check from this position to walkCamera Position if there is any object in between.(Prevent seeing through walls/floor)
    public float camSwitchSpeed = 4f;    //Speed which camera changes from Throw to Walk mode.
    public float camFollowSpeed = 20f;
    public Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;
    public float rotationSmoothTime = 0.1f;
    private bool cameraBlocked;

  

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        canMove = true;
        Cursor.lockState = cursorMode = CursorLockMode.Locked;
        Cursor.visible = false;

        playerLayer = 1 << 9;
        playerLayer = ~playerLayer;

        mainCamera = GameObject.FindWithTag("MainCamera");
        cameraT = Camera.main.transform;
        throwMode = false;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;
        pCollider = GetComponent<CapsuleCollider>();
        float radius = GetComponent<CapsuleCollider>().radius * 0.9f;
        colliderPosition = transform.position + Vector3.up * (radius * 0.9f);
        safeSpot = transform.position;
        InvokeRepeating("UpdateSafeSpot", 0, sUpdateInterval);
    }

    void Update()
    {
        //If box collide with other object, you wont be able to scroll and increase/decrease hold distance
        if (carriedObject != null)
        {
            if (!carriedObject.GetComponent<Pickupable>().collided)
            {
                //Increase or decrease of Telekinesis Hold distance
                if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
                {
                    currentHoldDistance += 1.0f;
                }
                else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
                {
                    if (currentHoldDistance > holdDistance)
                    {
                        currentHoldDistance -= 1.0f;
                    }
                }
            }else{
                if (currentHoldDistance >= holdDistance)
                {
                    currentHoldDistance -= 0.2f;
                }
            }     
        }else{}
 
        if (!throwMode)
        {
            currentChargeTime = 0f;
        }

        if (canMove)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                throwMode = false;
                currentHoldDistance = holdDistance;
                if (carrying) DropObject();

                else Pickup();
            }

            if (carrying)
            {
                Carry(carriedObject);
                if (Input.GetButton("Throw"))
                {
                    if (!throwMode)
                    {
                        carriedObject.GetComponent<Pickupable>().Transparency(true);
                        throwMode = true;

                    }
                    print("Throw Mode");
                    if (currentChargeTime < chargeDuration)
                    {
                        currentChargeTime += Time.deltaTime;
                        Debug.Log("Current Charge Time = " + currentChargeTime);
                    }
                }
                else if (Input.GetButtonUp("Throw"))
                {
                    carriedObject.GetComponent<Pickupable>().Transparency(false);
                    throwMode = false;
                    if (currentChargeTime >= 1f)
                    {
                        ThrowObject();

                        // Reset main camera.
                        mainCamera.GetComponent<Cam>().yaw = yaw;
                        mainCamera.GetComponent<Cam>().pitch = pitch;

                        mainCamera.GetComponent<Cam>().currentRotation = new Vector3(pitch, yaw);
                    }
                }
                               
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                CastStasis();
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                throwMode = false;
            }

            //Player throw mode Camera.
            if (throwMode)
            {
                yaw += speedH * Input.GetAxis("Mouse X");
                pitch -= speedV * Input.GetAxis("Mouse Y");

                pitch = Mathf.Clamp(pitch, cameraMinY, cameraMaxY);

                transform.eulerAngles = new Vector3(0, yaw, 0);
                mainCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
            }
        }

        //Camera=====================================
        if (!cameraBlocked)
        {
            if (!throwMode)
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, walkCamera.transform.position, camFollowSpeed * Time.deltaTime);
            }
            else
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, throwCamera.transform.position, camFollowSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (!throwMode)
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, zoomInCamera.transform.position, camFollowSpeed * Time.deltaTime);
            }
            else
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, throwCamera.transform.position, camFollowSpeed * Time.deltaTime);
            }
        }

        if (transform.position.y < resetHeight)
        {
            transform.position = safeSpot;
        }

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, cameraMinY, cameraMaxY);
        
        transform.eulerAngles = new Vector3(0, yaw, 0);                         //Player's Y Rotation
        mainCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        CameraRotator.RotateCameraRotator(mainCamera.transform.eulerAngles.x, 0f, 0f);

        Vector3 targetDir = walkCamera.transform.position - camReverseCheck.transform.position;
        Vector3 newDir = Vector3.RotateTowards(-transform.forward, targetDir, 1f, 0.0f);
        camReverseCheck.transform.rotation = Quaternion.LookRotation(newDir);

        //Camera Raycast.
        RaycastHit hit;     //Check forward to see if there is object blocking camera view.
        if (Physics.Raycast(walkCamera.transform.position, walkCamera.transform.forward, out hit))
        {
            
            if (hit.transform.tag != "Player")
            {
                cameraBlocked = true;
            }
            else
            {
                cameraBlocked = false;
            }
        }
        RaycastHit hit2;    //Reverse Check to see if there is object blocking camera view. (Due to camera does not render object if it is in it.)
        if (Physics.Raycast(camReverseCheck.transform.position, camReverseCheck.transform.forward, out hit2))
        {
            if(hit2.transform.tag != "Player")
            {
                cameraBlocked = true;
            }
            else
            {
                cameraBlocked = false;
            }
        }

            //Animations===================================
            inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");
        anim.SetFloat("inputV", inputV);
        anim.SetFloat("inputH", inputH);
        anim.SetBool("jump", jump);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GroundCheck();

        if (canMove)
        {
            if (isGrounded)
            {
                jump = false;
                Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                targetVelocity = transform.TransformDirection(targetVelocity);
                targetVelocity *= speed;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;
                rb.AddForce(velocityChange, ForceMode.VelocityChange);

                // Jump
                if (canJump && Input.GetButton("Jump"))
                {
                    jump = true;
                    rb.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
                }
            }

            else
            {
                Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                targetVelocity = transform.TransformDirection(targetVelocity);
                targetVelocity *= speed;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;
                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }

            // We apply gravity manually for more tuning control
            rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));
        }
    }

    void GroundCheck()
    {
        RaycastHit hit;
        Vector3 dir = new Vector3(0, -1);

        isGrounded = Physics.SphereCast(pCollider.transform.position + pCollider.center + (Vector3.up * 0.1f), pCollider.height / 2, Vector3.down, out hit, groundCheckDistance);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hazard") transform.position = safeSpot;
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    void UpdateSafeSpot()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 1f);
        if (hit.collider.tag == "Ground")
        {
            safeSpot = hit.point + new Vector3(0, 1f, 0);
        }

        else if (hit.collider.tag != "Ground") return;    
    }
}

