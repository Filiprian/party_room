using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyCharacterController : MonoBehaviour
{
    public float speed = 6f;
    public float sprintSpeed = 12f;
    public float jumpForce = 6f;
    public LayerMask groundLayer;
    public float sensitivity = 2f;

    private Rigidbody rb;
    private Transform cameraTransform;
    private bool isGrounded;
    private float rotationX = 0f;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true;
        rb.useGravity = true;  // Ensures gravity is enabled
    }

    void Update()
    {
        MovePlayer();
        LookAround();
        CheckGrounded();

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
            jumpForce = 40f;
        }
        else
        {
            currentSpeed = speed;
            jumpForce = 20f;
        }

        // Crouching
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(1f, 0.5f, 1f);
            currentSpeed = speed;
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        if (!isGrounded)
        {
            transform.position += Vector3.down * 4f * Time.deltaTime; // Adjust speed as needed
        }
        else 
        {
            rb.linearDamping = 8f;
        }
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        
        rb.AddForce(moveDirection.normalized * currentSpeed, ForceMode.Acceleration);
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void CheckGrounded()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f, groundLayer);
    }
}
