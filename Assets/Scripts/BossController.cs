using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator bossAnimator;
    private SpriteRenderer spriteRenderer; // For flipping the sprite
    private Transform player; // Reference to the player
    private bool isImmune = false;
    private bool hasLanded = false;
    private int health = 300;

    public Slider healthBar; // Reference to the health bar UI
    public float idleDuration = 2f;
    public float attackInterval = 4f; // Adjusted attack interval
    public float moveSpeed = 2f; // Speed of the boss

    // Reference to the treasure chest prefab
    public GameObject treasureChestPrefab;

    public Collider2D meleeRange;
    public GameObject laserGameObject; // Assign this in the Inspector
    public Transform laserSpawnPoint; // Assign the spawn point

    public GameObject armProjectilePrefab; // Reference the arm projectile prefab
    public Transform armSpawnPoint; // Reference the spawn point on the boss
    public float shootDistance = 10f; // Minimum distance to trigger the arm attack

    public GameObject abilityPanel;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bossAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Disable the laser initially
        if (laserGameObject != null)
        {
            laserGameObject.SetActive(false);
        }

        // Initialize the health bar
        if (healthBar != null)
        {
            healthBar.maxValue = health;
            healthBar.value = health;
        }

        // Ignore collisions between the boss and the player
        if (player != null)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            Collider2D bossCollider = GetComponent<Collider2D>();
            if (playerCollider != null && bossCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, bossCollider);
                Debug.Log("Collision between player and boss ignored.");
            }
        }

        Debug.Log("BossController initialized.");
    }

    private void Update()
    {
        if (!hasLanded && rb.velocity.magnitude <= 0.1f)
        {
            hasLanded = true;
            StartCoroutine(StartBossLogic());
        }

        if (player != null)
        {
            FollowPlayer(); // Make the boss follow the player
        }
    }

    public void StartBossBehavior()
    {
        Debug.Log("Starting Boss Logic...");
        abilityPanel.SetActive(true);
        StartCoroutine(StartBossLogic());
    }

    private IEnumerator StartBossLogic()
    {
        Debug.Log("Boss has landed! Starting Boss Logic...");
        yield return new WaitForSeconds(1f); // Pause before starting attacks
        StartCoroutine(BossBehaviorLoop());
    }

    private IEnumerator BossBehaviorLoop()
    {
        while (health > 0)
        {
            PerformRandomAttack(); // Perform a random attack
            yield return new WaitForSeconds(attackInterval); // Wait between attacks
        }
    }

    private void FollowPlayer()
    {
        // Move toward the player
        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        // Flip the sprite based on movement direction
        if (direction.x > 0) // Player is to the right
        {
            spriteRenderer.flipX = true; // Flip to face right
        }
        else if (direction.x < 0) // Player is to the left
        {
            spriteRenderer.flipX = false; // Flip to face left
        }
    }


    private void PerformRandomAttack()
    {
        int randomAttack = Random.Range(0, 3);
        switch (randomAttack)
        {
            case 0:
                PerformLaserCast();
                break;
            case 1:
                PerformShoot();
                break;
            case 2:
                PerformMelee();
                break;
        }
    }

    private void PerformShoot()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(player.position, transform.position);
            if (distanceToPlayer > shootDistance)
            {
                bossAnimator.SetTrigger("Shoot");
                StartCoroutine(LaunchArmProjectile(player.position));
            }
            else
            {
                PerformMelee();
            }
        }
    }

    private IEnumerator LaunchArmProjectile(Vector3 targetPosition)
    {
        yield return new WaitForSeconds(0.5f);
        if (armProjectilePrefab != null && armSpawnPoint != null)
        {
            GameObject armProjectile = Instantiate(armProjectilePrefab, armSpawnPoint.position, Quaternion.identity);
            ArmProjectileBehavior armBehavior = armProjectile.GetComponent<ArmProjectileBehavior>();
            if (armBehavior != null)
            {
                armBehavior.Launch(targetPosition);
            }
        }
    }

    private void PerformMelee()
    {
        bossAnimator.SetTrigger("Melee");
        StartCoroutine(DelayedMeleeAttack(0.5f));
    }

    private IEnumerator DelayedMeleeAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (meleeRange != null)
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.NoFilter();
            Collider2D[] results = new Collider2D[10];
            int hitCount = meleeRange.OverlapCollider(filter, results);

            for (int i = 0; i < hitCount; i++)
            {
                if (results[i].CompareTag("Player"))
                {
                    PlayerMovement playerMovement = results[i].GetComponent<PlayerMovement>();
                    if (playerMovement != null)
                    {
                        playerMovement.TakeDamage(40);
                    }
                }
            }
        }
    }

    private void PerformLaserCast()
    {
        bossAnimator.SetTrigger("LaserCast");
        StartCoroutine(DelayedLaserAttack(0.5f));
    }

    private IEnumerator DelayedLaserAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (laserGameObject != null && laserSpawnPoint != null)
        {
            laserGameObject.transform.position = laserSpawnPoint.position;
            Vector3 direction = (player.position - laserSpawnPoint.position).normalized;
            LaserBehavior laserBehavior = laserGameObject.GetComponent<LaserBehavior>();
            if (laserBehavior != null)
            {
                laserBehavior.ActivateLaser(direction);
            }
            laserGameObject.SetActive(true);
            StartCoroutine(DisableLaserAfterTime(2f));
        }
    }

    private IEnumerator DisableLaserAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (laserGameObject != null)
        {
            laserGameObject.SetActive(false);
        }
    }

    public int GetMaxHealth()
    {
        return 600;
    }

    public int GetCurrentHealth()
    {
        return health;
    }


    public void TakeDamage(int damage)
    {
        if (isImmune) return;
        health -= damage;
        health = Mathf.Clamp(health, 0, 1000);
        if (healthBar != null)
        {
            healthBar.value = health;
        }
        if (health <= 0)
        {
            StartCoroutine(Die());
            healthBar.fillRect.gameObject.SetActive(false); // Hide the fill when empty
        }
    }

    private IEnumerator Die()
    {
        bossAnimator.SetTrigger("Death");
        yield return new WaitForSeconds(3f);
        abilityPanel.SetActive(false);
        SpawnTreasureChest();
        Destroy(gameObject);
    }

    private void SpawnTreasureChest()
    {
        if (treasureChestPrefab != null)
        {
            // Spawn the chest slightly above the ground to simulate a fall
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
            GameObject chest = Instantiate(treasureChestPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Treasure chest spawned at: " + spawnPosition);
        }
    }

}
