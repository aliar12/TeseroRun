using UnityEngine;
using System.Collections;

public class TreasureChest : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private bool hasLanded = false; // Tracks if the chest has landed
    private bool canOpen = false; // Tracks if the player is in range to open the chest
    private Collider2D playerCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Collider2D chestCollider = GetComponent<Collider2D>();

        if (animator == null)
        {
            Debug.LogError("Animator not found on TreasureChest.");
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on TreasureChest.");
        }

        if (chestCollider == null)
        {
            Debug.LogError("Collider not found on TreasureChest.");
        }

        rb.bodyType = RigidbodyType2D.Dynamic; // Enable gravity for falling
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent the chest from rotating

        // Find the player's collider and ignore collision
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerCollider = player.GetComponent<Collider2D>();
            if (playerCollider != null && chestCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, chestCollider, false); // Enable collision for interaction
            }
        }
    }

    private void Update()
    {
        // Stop the chest when it lands
        if (!hasLanded && Mathf.Abs(rb.velocity.y) < 0.1f)
        {
            LandOnGround();
        }

        // Check if the player presses 'E' to open the chest
        if (canOpen && Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasLanded && collision.collider.CompareTag("Ground"))
        {
            LandOnGround();
        }
        else if (collision.collider.CompareTag("Player"))
        {
            canOpen = true;
            Debug.Log("Player in range. Press 'E' to open the chest.");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            canOpen = false;
            Debug.Log("Player out of range.");
        }
    }

    private void OpenChest()
    {
        if (animator == null)
        {
            Debug.LogError("Animator not assigned!");
            return;
        }

        Debug.Log("Opening the treasure chest...");
        animator.SetTrigger("OpenChest"); // Trigger open animation
        StartCoroutine(LoadEndingScene());
    }

    private IEnumerator LoadEndingScene()
    {
        yield return new WaitForSeconds(2f); // Wait for the open animation to complete
        Debug.Log("Loading ending scene...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Ending"); // Replace with your scene name
    }

    private void LandOnGround()
    {
        hasLanded = true;
        rb.velocity = Vector2.zero; // Stop all motion
        rb.constraints = RigidbodyConstraints2D.FreezePositionX; //| RigidbodyConstraints2D.FreezePositionY; // Freeze position but allow interactions
        Debug.Log("Treasure chest has landed.");
    }
}
