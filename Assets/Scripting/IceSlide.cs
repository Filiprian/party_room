using UnityEngine;

public class IceSlide : MonoBehaviour
{
    public float slideFriction = 0.98f; // How long the slide effect lasts (closer to 1 = longer sliding)
    public float iceAcceleration = 1.5f; // How fast the player moves on ice
    private bool isOnIce = false;
    private Vector3 iceVelocity;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get Rigidbody component
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Icey"))
        {
            isOnIce = true; // Player stepped on ice
            iceVelocity = rb.linearVelocity; // Store current velocity for smooth transition
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Icey"))
        {
            isOnIce = false; // Player left the ice
        }
    }

    void FixedUpdate()
    {
        if (isOnIce)
        {
            // Get input movement
            float moveX = Input.GetAxis("Vertical");
            float moveZ = Input.GetAxis("Horizontal");

            // Calculate movement direction
            Vector3 moveDirection = new Vector3(-moveX, 0, moveZ).normalized;

            // If moving, accelerate in that direction
            if (moveDirection.magnitude > 0.1f)
            {
                iceVelocity += moveDirection * iceAcceleration * Time.fixedDeltaTime;
            }

            // Apply sliding effect (slowly reduce movement speed)
            iceVelocity *= slideFriction;

            // Apply velocity to Rigidbody
            rb.linearVelocity = new Vector3(iceVelocity.x, rb.linearVelocity.y, iceVelocity.z);
        }
    }
}
