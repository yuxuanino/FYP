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
    public float airSpeedPenalty;
    private bool grounded = false;
    public bool canMove;

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
    public float cameraMaxY = 90f;
    public float cameraMinY = -25f;

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
    public float camSwitchSpeed = 4;    //Speed which camera changes from Throw to Walk mode.
    public Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;
    public float rotationSmoothTime = 0.1f;

  

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        canMove = true;
        Cursor.lockState = cursorMode = CursorLockMode.Locked;
        Cursor.visible = false;

        mainCamera = GameObject.FindWithTag("MainCamera");
        cameraT = Camera.main.transform;
        throwMode = false;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;
    }

    void Update()
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
        if (!throwMode)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, walkCamera.transform.position, camSwitchSpeed * Time.deltaTime);
        }
        else
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, throwCamera.transform.position, camSwitchSpeed * Time.deltaTime);
        }

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, cameraMinY, cameraMaxY);
        
        transform.eulerAngles = new Vector3(0, yaw, 0);                         //Player's Y Rotation
        mainCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        CameraRotator.RotateCameraRotator(mainCamera.transform.eulerAngles.x, 0f, 0f);
        
        

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
        if (canMove)
        {
            if (grounded)
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

            grounded = false;
        }
    }

    void OnCollisionStay()
    {
        jump = false;
        grounded = true;
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }
}

