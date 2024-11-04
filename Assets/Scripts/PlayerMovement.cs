using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;        // Movement speed
    public float jumpForce = 10f;       // Jump force
    public Transform groundCheck;       // Transform to check if the player is grounded
    public LayerMask groundLayer;       // Layer for the ground

    private Rigidbody2D rb;             // Rigidbody2D component for physics
    private bool isGrounded;            // Check if the player is on the ground

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Debug output
        Debug.Log("Is Grounded: " + isGrounded);

        // Get horizontal movement input (A/D or Arrow keys)
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Move the player left or right
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Jump input (W key)
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

}
