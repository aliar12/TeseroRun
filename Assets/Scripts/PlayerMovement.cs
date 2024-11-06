using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;        
    public float jumpForce = 10f;       
    public Transform groundCheck;      
    public LayerMask groundLayer;
    public GameManager scoreManager;


    private Rigidbody2D rb;          
    private bool isGrounded;            

    void Start()
    {
       
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
       
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        

       
        float moveInput = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            scoreManager.AddScore(200);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Trap")
        {
            scoreManager.removePoints(50);
            //Destroy(other.gameObject);

        }
    }
}
