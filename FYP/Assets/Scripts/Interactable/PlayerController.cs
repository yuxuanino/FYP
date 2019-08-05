using UnityEngine;
using System.Collections;

public class PlayerController : PlayerAbilities
{

    public float speed = 10.0f;
    public float gravity = 10.0f;
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
    public float airSpeedPenalty;
    private bool grounded = false;
    public bool canMove;

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public float cameraMaxY = 90f;
    public float cameraMinY = -90f;

    CursorLockMode cursorMode;
    
    Rigidbody rb;

    void Start()
    {
        canMove = true;
        Cursor.lockState = cursorMode = CursorLockMode.Locked;
        Cursor.visible = false;
        mainCamera = GameObject.FindWithTag("MainCamera");
    }


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;
    }


    void Update()
    {
            if (canMove)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (carrying) DropObject();

                else Pickup();
            }

            if (carrying)
            {
                Carry(carriedObject);
                if (Input.GetButtonDown("Throw")) ThrowObject();
            }

            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            pitch = Mathf.Clamp(pitch, cameraMinY, cameraMaxY);

            transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
            mainCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

            if (Input.GetKeyDown(KeyCode.F))
            {
                CastStasis();
            }
        }
    }
        void FixedUpdate()
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

        void OnCollisionStay()
        {
            grounded = true;
        }

        float CalculateJumpVerticalSpeed()
        {
            // From the jump height and gravity we deduce the upwards speed 
            // for the character to reach at the apex.
            return Mathf.Sqrt(2 * jumpHeight * gravity);
        }

    //void ChangeCursorLockState()
    //{
    //    if (cursorMode == CursorLockMode.Locked)
    //    {
    //        Cursor.lockState = cursorMode = CursorLockMode.None;
    //        Cursor.visible = true;
    //    }

    //    else Cursor.lockState = cursorMode = CursorLockMode.Locked;
    //    Cursor.visible = false;
    //}
}