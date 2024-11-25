using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Include for scene management

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 10f;
    public float attackDuration = 0.5f;
    public GameObject swordSlashPrefab; // Slash prefab reference
    public GameObject slash;
    private bool isJumping = false;
    private bool isAttacking = false;
    private Rigidbody2D rb;
    private Animator animator;
    private float moveInput;
    private float lastDirection = 1.0f; // Direction player is facing
    public Vector3 slashOffset; // Offset for spawning slash
    private int jumpCount = 0; // Counter for double jump
    public GameManager gameManager;
    public float fallThreshold = -10f; // Y-coordinate threshold for falling off the map

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAttack();
        UpdateAnimator();
        CheckFallOffMap(); // Check if the player has fallen off the map
    }

    private void HandleMovement()
    {
        moveInput = Input.GetAxisRaw("Horizontal"); // -1 for left, 1 for right, 0 for idle

        if (!isAttacking)
        {
            // Set horizontal velocity
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
            rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);

            if (moveInput != 0)
            {
                lastDirection = moveInput;
                FlipSprite(lastDirection, GetComponent<SpriteRenderer>());
            }
            else
            {
                // Stop horizontal movement when no key is pressed
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

    private void FlipSprite(float direction, SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null)
            return;

        // Flip the character sprite by modifying the flipX property of the SpriteRenderer
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

        // Reset jump count when grounded
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
        Debug.Log("Spawning slash!");

        Vector3 spawnPosition = transform.position + new Vector3(lastDirection * slashOffset.x, slashOffset.y, 0.0f);
        slash = Instantiate(swordSlashPrefab, spawnPosition, Quaternion.identity);

        SpriteRenderer slashRenderer = slash.GetComponent<SpriteRenderer>();
        if (slashRenderer != null)
        {
            slashRenderer.flipX = lastDirection < 0;
        }

        Debug.Log("Slash spawned at position: " + spawnPosition);
    }


    private void UpdateAnimator()
    {
        // Update animator parameters
        if (moveInput == 0)
        {
            animator.SetFloat("CharacterSpeed", 1); // Idle
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetFloat("CharacterSpeed", 6); // Running
        }
        else
        {
            animator.SetFloat("CharacterSpeed", 4); // Walking
        }

        animator.SetBool("isJumping", isJumping); // Set jumping state
    }

    private void CheckFallOffMap()
    {
        if (transform.position.y < fallThreshold)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
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

    public void UnspawnSlash()
    {
        if (slash != null)
        {
            Destroy(slash);
            slash = null; // Reset the reference
        }
    }
}
