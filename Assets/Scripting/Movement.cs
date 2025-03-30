using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleFirstPersonController : MonoBehaviour
{
    public float walkSpeed = 5f;   // Movement speed
    public float sprintSpeed = 12f;   // Movement speed
    public float lookSpeed = 2f;   // Mouse sensitivity
    public float jumpForce = 5f;   // Jump height
    public LayerMask groundLayer;  // Layer for ground detection

    private Rigidbody rb;
    private Camera playerCamera;
    private bool isGrounded;
    private bool isCrouching;
    private bool isSticky;
    private float moveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;  // Lock cursor to the screen
        moveSpeed = walkSpeed;
    }

    void Update()
    {
        // Movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);

        // Sprint
        if (!isSticky)
        {
            if (isGrounded && !isCrouching && Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = sprintSpeed;
            }
            else
            {
                moveSpeed = walkSpeed;
            }
        }
        else
        {
            moveSpeed = walkSpeed/3;
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

        transform.Rotate(Vector3.up * mouseX);  // Rotate body horizontally
        playerCamera.transform.Rotate(Vector3.left * mouseY);  // Rotate camera vertically

        CheckGrounded();  // Update ground check
    }

    // Check if the player is grounded
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
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sticky"))
        {
            isSticky = false;
        }
    }
}