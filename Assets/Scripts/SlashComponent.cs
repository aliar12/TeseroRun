using UnityEngine;

public class SlashComponent : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int damage = 10; // Damage dealt by the slash
    public float moveSpeed = 5f; // Speed at which the slash moves
    public float lifetime = 0.5f; // How long the slash exists before disappearing

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, lifetime); // Automatically destroy the slash after its lifetime
    }

    private void Update()
    {
        // Move the slash in the correct direction
        Vector3 direction = new Vector3(1, 0, 0);
        if (spriteRenderer.flipX) // Flip direction if sprite is flipped
        {
            direction = direction * -1.0f;
        }
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Slash hit: {other.gameObject.name}");

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit by slash!");

            // Check if the object has a SlimeMonster script
            SlimeMonster slimeEnemy = other.GetComponent<SlimeMonster>();
            if (slimeEnemy != null)
            {
                Debug.Log($"Applying damage to SlimeMonster: {damage}");
                slimeEnemy.TakeDamage(damage);
                return; // Exit after applying damage to avoid unnecessary checks
            }

            // Check if the object has a BossController script
            BossController bossEnemy = other.GetComponent<BossController>();
            if (bossEnemy != null)
            {
                Debug.Log($"Applying damage to BossController: {damage}");
                bossEnemy.TakeDamage(damage);
                return; // Exit after applying damage to avoid unnecessary checks
            }

            // If neither script is found
            Debug.Log("No valid enemy script found on this object!");
        }
        else if (other.CompareTag("Projectile")) // Handle projectile deflection
        {
            Debug.Log("Slash hit a projectile!");

            // Check if the projectile has an ArmProjectileBehavior script
            ArmProjectileBehavior armProjectile = other.GetComponent<ArmProjectileBehavior>();
            if (armProjectile != null)
            {
                // Deflect the projectile back toward the boss
                GameObject boss = GameObject.FindGameObjectWithTag("Enemy");
                if (boss != null)
                {
                    Vector3 deflectDirection = (boss.transform.position - transform.position).normalized;
                    armProjectile.Deflect(deflectDirection);
                    Debug.Log($"Projectile deflected toward the boss! Direction: {deflectDirection}");
                }
                else
                {
                    Debug.LogWarning("No boss found to deflect the projectile toward!");
                }
            }
        }
    }
}
