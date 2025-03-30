using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleFirstPersonController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 12f;
    public float lookSpeed = 2f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    
    private Rigidbody rb;
    private Camera playerCamera;
    private bool isGrounded;
    private bool isCrouching;
    private bool isSticky;
    private bool isOnIce;
    private float moveSpeed;
    private Vector3 lastVelocity;

    public float iceAcceleration = 2f; // How fast the player gains control on ice
    public float iceFriction = 0.98f;  // How much sliding slows down over time

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        moveSpeed = walkSpeed;
    }

    void Update()
    {
        CheckGrounded();

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;

        if (!isOnIce) 
        {
            // Normal movement on ground
            rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
        }
        else
        {
            // Add force for gradual movement on ice
            rb.AddForce(moveDirection * iceAcceleration, ForceMode.Acceleration);
        }

        // Sprinting
        if (!isSticky && !isOnIce)
        {
            moveSpeed = (isGrounded && !isCrouching && Input.GetKey(KeyCode.LeftShift)) ? sprintSpeed : walkSpeed;
        }
        else if (isSticky)
        {
            moveSpeed = walkSpeed / 3;
        }

        // Jumping
        if (isGrounded && !isCrouching && !isSticky && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Crouching
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(1f, 0.5f, 1f);
            isCrouching = true;
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            isCrouching = false;
        }

        // Looking around
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        transform.Rotate(Vector3.up * mouseX);
        playerCamera.transform.Rotate(Vector3.left * mouseY);
    }

    void FixedUpdate()
    {
        if (isOnIce)
        {
            // Gradually reduce speed for sliding effect
            rb.linearVelocity = new Vector3(rb.linearVelocity.x * iceFriction, rb.linearVelocity.y, rb.linearVelocity.z * iceFriction);
        }
    }

    void CheckGrounded()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, groundLayer);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sticky"))
        {
            isSticky = true;
        }

        if (collision.gameObject.CompareTag("Icey"))
        {
            isOnIce = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sticky"))
        {
            isSticky = false;
        }

        if (collision.gameObject.CompareTag("Icey"))
        {
            isOnIce = false;
        }
    }
}
