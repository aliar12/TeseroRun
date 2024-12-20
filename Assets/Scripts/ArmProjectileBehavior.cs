using UnityEngine;

public class ArmProjectileBehavior : MonoBehaviour
{
    public int damage = 20; // Damage dealt by the projectile
    public float speed = 10f; // Speed of the projectile
    private Vector3 direction; // Direction of movement
    private bool isDeflected = false; // Whether the projectile has been deflected

    private void Update()
    {
        // Move the projectile in the current direction
        transform.position += direction * speed * Time.deltaTime;

        // Optional: Change color when deflected for visual feedback
        if (isDeflected)
        {
            GetComponent<SpriteRenderer>().color = Color.red; // Change to red
        }
    }

    public void Launch(Vector3 targetPosition)
    {
        direction = (targetPosition - transform.position).normalized;
    }

    public void Deflect(Vector3 newDirection)
    {
        isDeflected = true; // Mark as deflected
        direction = newDirection.normalized; // Update direction to new deflection direction
        Debug.Log($"Projectile deflected! New direction: {direction}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDeflected && other.CompareTag("Player"))
        {
            // Damage the player only if not deflected
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("Projectile hit the player!");
            }
            Destroy(gameObject); // Destroy the projectile after hitting the player
        }
        else if (isDeflected && other.CompareTag("Enemy"))
        {
            // Damage the boss if deflected
            BossController boss = other.GetComponent<BossController>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
                Debug.Log("Projectile hit the boss!");
            }
            Destroy(gameObject); // Destroy the projectile after hitting the boss
        }
        else if (isDeflected && other.CompareTag("Player"))
        {
            // Prevent deflected projectiles from affecting the player
            Debug.Log("Deflected projectile ignored the player.");
        }
    }

}
