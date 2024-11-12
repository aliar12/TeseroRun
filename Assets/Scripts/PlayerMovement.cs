using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public GameManager gameManager;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int jumpCount; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = 0;  
    }

    void Update()
    {
       
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        

       
        if (isGrounded)
        {
            jumpCount = 0; 
        }

        
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        
        if (Input.GetKeyDown(KeyCode.W) && jumpCount < 1)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++; 
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            gameManager.AddScore(200);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Trap"))
        {
            gameManager.removePoints(50);
           
        }
        else if (other.gameObject.CompareTag("key"))
        {
            gameManager.AddKey(1);
            Destroy(other.gameObject);
        }
    }
}
