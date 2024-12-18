using System.Collections;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackRange = 6f; // Distance to move towards enemy

    private Rigidbody2D rb;
    private Animator animator;
    private Transform targetEnemy;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        FindClosestEnemy(); // Automatically find the closest enemy

        if (targetEnemy != null)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, targetEnemy.position);

            if (distanceToEnemy > attackRange)
            {
                MoveTowardsEnemy(); // Move towards the enemy if not in range
            }
            else
            {
                StopMoving(); // Stop moving once in range
            }
        }
        else
        {
            StopMoving(); // Idle behavior if no enemies detected
        }

        UpdateAnimator();
    }

    private void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        targetEnemy = closestEnemy;
    }

    private void MoveTowardsEnemy()
    {
        if (targetEnemy == null) return;

        Vector2 direction = (targetEnemy.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        moveInput = Mathf.Abs(rb.velocity.x);
        FlipSprite(direction.x);
    }

    private void StopMoving()
    {
        rb.velocity = Vector2.zero;
        moveInput = 0;
    }

    private void FlipSprite(float direction)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = direction < 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SlimeMonster enemyScript = collision.gameObject.GetComponent<SlimeMonster>();
            if (enemyScript != null)
            {
                Destroy(enemyScript.gameObject); // Destroy the enemy
                if (enemyScript.getHealthBar() != null)
                {
                    Destroy(enemyScript.getHealthBar()); // Destroy the health bar
                }
            }
        }
    }

    private void UpdateAnimator()
    {
        // Update animator parameters based on movement speed
        if (moveInput == 0)
        {
            animator.SetFloat("CharacterSpeed", 1); // Idle
        }
        else if (moveInput > 0)
        {
            animator.SetFloat("CharacterSpeed", 4); // Walking
        }
    }
}
