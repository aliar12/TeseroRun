using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    private Animator bossAnimator;
    private bool isImmune = false;
    private int health = 200;

    public Slider healthBar; // Reference to the health bar UI
    public float idleDuration = 2f;
    public float attackInterval = 3f;

    // Reference to the treasure chest prefab
    public GameObject treasureChestPrefab;

    // Offset to spawn the treasure chest relative to the boss
    //public Vector3 treasureChestSpawnOffset;

    private void Start()
    {
        bossAnimator = GetComponent<Animator>();
        bossAnimator.speed = 0.5f;

        // Initialize the health bar
        if (healthBar != null)
        {
            healthBar.maxValue = health;
            healthBar.value = health;
        }

        Debug.Log("BossController initialized.");
    }

    public void StartBossBehavior()
    {
        Debug.Log("Starting Boss Logic...");
        StartCoroutine(WaitForEntranceAndStartLogic());
    }

    private IEnumerator WaitForEntranceAndStartLogic()
    {
        Debug.Log("Waiting for BossEntrance animation to finish...");

        while (bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("BossEntrance") &&
               bossAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null; // Wait until the animation finishes
        }

        Debug.Log("BossEntrance animation completed. Transitioning to BossIdle...");
        bossAnimator.SetBool("EntranceDone", true);
        transform.position = bossAnimator.rootPosition;

        StartCoroutine(BossBehaviorLoop());
    }

    private IEnumerator BossBehaviorLoop()
    {
        while (health > 0)
        {
            bossAnimator.SetBool("isIdle", true);
            yield return new WaitForSeconds(idleDuration);
            bossAnimator.SetBool("isIdle", false);

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

            yield return new WaitForSeconds(attackInterval);
        }
    }

    private void PerformGlow()
    {
        bossAnimator.SetTrigger("Glow");
        Debug.Log("Boss is performing Glow attack!");
    }

    private void PerformShoot()
    {
        bossAnimator.SetTrigger("Shoot");
        Debug.Log("Boss is performing Shoot attack!");
    }

    private void PerformMelee()
    {
        bossAnimator.SetTrigger("Melee");
        Debug.Log("Boss is performing Melee attack!");
    }

    private void PerformLaserCast()
    {
        bossAnimator.SetTrigger("LaserCast");
        Debug.Log("Boss is performing Laser Cast attack!");
    }

    public int GetMaxHealth()
    {
        return 200;
    }

    public int GetCurrentHealth()
    {
        return health;
    }

    public void TakeDamage(int damage)
    {
        if (isImmune) return;

        health -= damage;
        health = Mathf.Clamp(health, 0, GetMaxHealth()); // Clamp health to prevent negatives

        Debug.Log($"Boss took {damage} damage. Current health: {health}");

        // Update the health bar
        if (healthBar != null)
        {
            healthBar.value = health;

            if (health <= 0)
            {
                healthBar.fillRect.gameObject.SetActive(false); // Hide the fill when empty
            }
        }

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        bossAnimator.SetTrigger("Death");
        Debug.Log("Boss is dying...");
        yield return new WaitForSeconds(3f);

        // Spawn the treasure chest
        SpawnTreasureChest();

        Destroy(gameObject);
    }

    private void SpawnTreasureChest()
    {
        if (treasureChestPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(122.5f, -6.7f, 0f);
            Debug.Log($"Hardcoded spawn position: {spawnPosition}");

            Instantiate(treasureChestPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Treasure chest spawned!");
        }
        else
        {
            Debug.LogWarning("Treasure chest prefab not assigned in BossController!");
        }
    }

    public void SetImmune(bool value)
    {
        isImmune = value;
        bossAnimator.SetBool("isImmune", value);
    }
}
