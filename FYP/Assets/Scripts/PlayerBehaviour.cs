using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    Rigidbody rb;

    public bool isGrounded = true; // Tracks whether this is touching the ground.
    public bool IsGrounded() { return isGrounded; }
    public float jumpSpeed = 8.0f;
    public float speed = 10.0f;
    public float gravity = 10.0f;
    public float maxVelocityChange = 10.0f;

    protected Vector3 groundNormal; // If you are not in the air, this records the normal of the ground you are on.
    protected BoxCollider platform; // Platform that one is standing on when grounded.
    public BoxCollider movementCollider;
    public LayerMask whatIsGround;
    int layerMask;
    public float groundLineOffset = 0.02f; // How far down does the line to find the ground extend?

    private Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        layerMask = 1 << whatIsGround.value;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        UpdateIsGrounded();

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


        // We apply gravity manually for more tuning control
        rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));
    }

    protected virtual void UpdateIsGrounded()
    {
        RaycastHit h;

        Debug.DrawRay(transform.position, Vector3.down, Color.red);

        if (Physics.Raycast(transform.position, Vector3.down, out h, groundLineOffset, layerMask))
        {
            isGrounded = true;
        }

        else
        {
            isGrounded = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump"))
        {
            moveDirection.y = jumpSpeed;
        }
    }
}
