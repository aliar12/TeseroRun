using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 10f;
    public float attackDuration = 0.5f;
    public GameObject swordSlashPrefab;
    public GameObject slash;
    public Vector3 slashOffset;
    public GameManager gameManager;
    public float fallThreshold = -10f;

    private bool isJumping = false;
    private bool isAttacking = false;
    private Rigidbody2D rb;
    private Animator animator;
    private float moveInput;
    private float lastDirection = 1.0f;
    private int jumpCount = 0;

    // Health variables
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth); // Initialize health bar
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAttack();
        UpdateAnimator();
        CheckFallOffMap();
    }

    private void HandleMovement()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (!isAttacking)
        {
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
            rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);

            if (moveInput != 0)
            {
                lastDirection = moveInput;
                FlipSprite(lastDirection, GetComponent<SpriteRenderer>());
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

    private void FlipSprite(float direction, SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null) return;
        spriteRenderer.flipX = direction < 0;
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && jumpCount < 2)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
            isJumping = true;
        }

        if (rb.velocity.y == 0)
        {
            isJumping = false;
            jumpCount = 0;
        }
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    private System.Collections.IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        animator.SetTrigger("doAttack"); // Trigger the attack animation
        SpawnSlash(); // Spawn the slash attack
        yield return new WaitForSeconds(attackDuration); // Wait for the attack duration
        isAttacking = false;
    }

    private void SpawnSlash()
    {
        Vector3 spawnPosition = transform.position + new Vector3(lastDirection * slashOffset.x, slashOffset.y, 0.0f);
        slash = Instantiate(swordSlashPrefab, spawnPosition, Quaternion.identity);

        SpriteRenderer slashRenderer = slash.GetComponent<SpriteRenderer>();
        if (slashRenderer != null)
        {
            slashRenderer.flipX = lastDirection < 0;
        }
    }
    public void UnspawnSlash()
    {
        if (slash != null)
        {
            Destroy(slash);
            slash = null; // Reset the reference
        }
    }
    private void UpdateAnimator()
    {
        if (moveInput == 0)
        {
            animator.SetFloat("CharacterSpeed", 1);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetFloat("CharacterSpeed", 6);
        }
        else
        {
            animator.SetFloat("CharacterSpeed", 4);
        }

        animator.SetBool("isJumping", isJumping);
    }

    private void CheckFallOffMap()
    {
        if (transform.position.y < fallThreshold)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            gameManager.AddScore(5); // Add score when colliding with Coin
            Destroy(other.gameObject); // Destroy the coin
        }
        else if (other.gameObject.CompareTag("Trap"))
        {
            gameManager.removePoints(50); // Remove points on trap collision
        }
        else if (other.gameObject.CompareTag("Key"))
        {
            gameManager.AddKey(1); // Add key when colliding with Key
            Destroy(other.gameObject); // Destroy the key
        }
    }

    private bool IsDeflecting()
    {
        // Replace with your deflect/blocking logic, e.g., check if the player is attacking
        return Input.GetKey(KeyCode.Space); // Example: Deflect when holding Space
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage! Current health: {currentHealth}");
        healthBar.SetHealth(currentHealth); // Update health bar
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth); // Update health bar
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
