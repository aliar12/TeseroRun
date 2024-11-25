using UnityEngine;
using UnityEngine.UI;

public class SlimeMonster : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public GameObject healthBarPrefab;
    private GameObject healthBar;
    private Image healthBarFill;

    public float speed = 2f;
    public float followDistance = 8f;
    public float stopDistance = 3f; // Distance at which the slime stops moving to avoid pushing the player
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar = Instantiate(healthBarPrefab);
        healthBarFill = healthBar.transform.Find("Background/Fill").GetComponent<Image>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (isDead) return;

        if (healthBar != null)
        {
            healthBar.transform.position = transform.position + new Vector3(0, 1.5f, 0);
        }

        if (player != null && Vector2.Distance(transform.position, player.position) <= followDistance)
        {
            FollowPlayer();
        }
        else
        {
            StopMoving();
        }
    }

    private void FollowPlayer()
    {
        if (isDead) return; // Prevent movement if dead

        // Check if "Hit" animation is playing
        if (animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("SlimeHit")) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Stop moving if within the stop distance
        if (distanceToPlayer <= stopDistance)
        {
            StopMoving();
            return;
        }

        // Enable running animation only if moving
        if (animator != null)
        {
            animator.SetBool("isRunning", true);
        }

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y); // Maintain gravity on Y-axis
    }

    private void StopMoving()
    {
        rb.velocity = Vector2.zero; // Stop all horizontal movement
        if (animator != null)
        {
            animator.SetBool("isRunning", false); // Stop running animation
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Slime took damage! Current Health: {currentHealth}");

        if (animator != null)
        {
            animator.SetTrigger("Hit");
            animator.Play("SlimeHit", 0, 0f); // Force "Hit" animation
        }

        // Stop movement during hit animation
        StopMoving();

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;

            float healthPercent = (float)currentHealth / maxHealth;
            healthBarFill.color = healthPercent > 0.5f ? Color.green : (healthPercent > 0.2f ? Color.yellow : Color.red);
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Slime died!");

        if (animator != null)
        {
            animator.SetTrigger("Die");
            animator.Play("SlimeDie", 0, 0f); // Force "Die" animation
        }

        StopMoving(); // Stop movement completely
        rb.velocity = Vector2.zero; // Ensure no movement on Y-axis

        Destroy(healthBar);
        Destroy(gameObject, 1.5f); // Destroy after death animation
    }
}
