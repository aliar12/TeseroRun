using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    public int damage = 20; // Damage dealt by the laser
    public float speed = 10f; // Speed of the laser
    private Vector3 direction; // Direction the laser is traveling
    private bool isActive; // Whether the laser is currently active

    private void Update()
    {
        if (isActive)
        {
            // Move the laser in the specified direction at the set speed
            transform.position += direction * Time.deltaTime * speed;
        }
    }

    public void ActivateLaser(Vector3 newDirection)
    {
        direction = newDirection.normalized; // Normalize the direction
        isActive = true;

        // Rotate the laser to face the direction it's traveling
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Flip the laser if it's moving left
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        Debug.Log($"Laser activated with direction: {direction}, angle: {angle}, speed: {speed}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("Laser hit the player!");
            }
            isActive = false;
            gameObject.SetActive(false); // Deactivate laser
        }
    }
}
