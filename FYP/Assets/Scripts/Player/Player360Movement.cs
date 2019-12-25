using UnityEngine;
using System.Collections;

public class Player360Movement : PlayerAbilities
{

    public float speed = 10.0f;
    public float gravity = 10.0f;
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
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
    public float cameraMinY = -90f;

    //Switch between default or ThrowMode. Default when walking around. Throw mode for when tossing object.
    public bool throwMode;
    public float chargeDuration;
    public float currentChargeTime;

    //Animations
    public Animator anim;
    private float inputH;
    private float inputV;
    private bool jump;

    //Dialogue
    public TestScript dialogueTS;

    void Start()
    {
        base.Init();
        canMove = true;
        Cursor.lockState = cursorMode = CursorLockMode.Locked;
        Cursor.visible = false;

        mainCamera = GameObject.FindWithTag("MainCamera");
        cameraT = Camera.main.transform;
        throwMode = false;

        dialogueTS = GetComponent<TestScript>();
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
                        //Calls the transparency function and makes the carriedObject seethrough.
                        carriedObject.GetComponent<Pickupable>().PickUpEffects(true);
                        throwMode = true;
                        /*
                        transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);

                        yaw = transform.eulerAngles.y;
                        pitch = 0f;
                        */

                        yaw = mainCamera.GetComponent<Cam>().yaw;
                        pitch = mainCamera.GetComponent<Cam>().pitch;
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
                    //Calls the Transparency function in Pickupable and makes the carriedObject solid again.
                    carriedObject.GetComponent<Pickupable>().PickUpEffects(false);
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
        //Animations
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");
        anim.SetFloat("inputV", inputV);
        anim.SetFloat("inputH", inputH);
        anim.SetBool("jump", jump);
    }

    void FixedUpdate()
    {
        //==================================================== Walking MODE ====================================================
        if (!throwMode)
        {   
            if (canMove)
            {
                if (grounded)
                {
                    // Calculate how fast we should be moving
                    Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                    //targetVelocity = transform.TransformDirection(targetVelocity);
                    targetVelocity *= speed;
                    targetVelocity = cameraT.TransformDirection(targetVelocity);
                    
                    //Player direction.
                    Vector2 inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                    if (inputDir != Vector2.zero)
                    {
                        float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
                        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
                    }

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
                    Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal") / airSpeedPenalty, 0, Input.GetAxis("Vertical") / airSpeedPenalty);
                    //targetVelocity = transform.TransformDirection(targetVelocity);
                    targetVelocity *= speed;
                    targetVelocity = cameraT.TransformDirection(targetVelocity);

                    //Player direction.
                    Vector2 inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                    if (inputDir != Vector2.zero)
                    {
                        float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
                        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
                    }

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
        //==================================================== THROW MODE ====================================================
        else
        {
            if (canMove)
            {
                if (grounded)
                {
                    // Calculate how fast we should be moving
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
                    Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal") / airSpeedPenalty, 0, Input.GetAxis("Vertical") / airSpeedPenalty);
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