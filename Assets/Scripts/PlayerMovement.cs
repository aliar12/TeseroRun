using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float runSpeed = 6f;  // Speed while running
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public GameManager gameManager;
    private Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int jumpCount; 
    private float currentMoveSpeed;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        jumpCount = 0;  
        currentMoveSpeed = moveSpeed;  // Initialize with normal move speed
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // If grounded, reset jump count and trigger the "isJumping" animation to false
        if (isGrounded)
        {
            jumpCount = 0;  // Reset jump count when grounded
            animator.SetBool("isJumping", false);  // Set "isJumping" to false when grounded
        }

        // Get horizontal input (A/D or arrow keys)
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Set the player's velocity
        rb.velocity = new Vector2(moveInput * currentMoveSpeed, rb.velocity.y);
        if (moveInput == 0.0f)
        {
            animator.SetFloat("CharacterSpeed", 0.0f);
        }

        // Handle running animation and speed change when Shift is pressed
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentMoveSpeed = runSpeed;  // Set speed to run speed
            animator.SetFloat("CharacterSpeed", 6);
        }
        else
        {
            currentMoveSpeed = moveSpeed;  // Set speed back to normal
            animator.SetFloat("CharacterSpeed", 4);
        }

        // Handle character flip based on movement direction
        if (moveInput > 0) // Moving right
        {
            transform.localScale = new Vector3(1, 1, 1);  // Facing right
        }
        else if (moveInput < 0) // Moving left
        {
            transform.localScale = new Vector3(-1, 1, 1);  // Facing left
        }

        // Handle jumping (W key)
        if (Input.GetKeyDown(KeyCode.W) && jumpCount < 1) // Single jump, if not already in the air
        {
            animator.SetBool("isJumping", true);  // Trigger jump animation
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);  // Apply jump force
            jumpCount++;  // Increment jump count
        }
        // You can optionally check if the player has fallen below the ground
        /*else if (rb.velocity.y < 0)
        {
            animator.SetBool("isJumping", false);  // Set "isJumping" to false when falling
        }*/
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            gameManager.AddScore(5);  // Add score when colliding with Coin
            Destroy(other.gameObject);  // Destroy coin
        }
        else if (other.gameObject.CompareTag("Trap"))
        {
            gameManager.removePoints(50);  // Remove points on trap collision
        }
        else if (other.gameObject.CompareTag("Key"))
        {
            gameManager.AddKey(1);  // Add key when colliding with Key
            Destroy(other.gameObject);  // Destroy key
        }
    }
}
