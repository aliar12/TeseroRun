using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject knife; // Reference to the Knife GameObject
    public int knifeDamage = 20;
    public float swingDuration = 0.2f;  // Duration of the knife swing

    private bool isSwinging = false;

    void Update()
    {
        // Check if the player presses Space to swing the knife
        if (Input.GetKeyDown(KeyCode.Space) && !isSwinging)
        {
            StartCoroutine(SwingKnife());
        }
    }

    private IEnumerator SwingKnife()
    {
        isSwinging = true;
        knife.GetComponent<Collider2D>().enabled = true;  // Enable the knife collider

        yield return new WaitForSeconds(swingDuration);

        knife.GetComponent<Collider2D>().enabled = false; // Disable the knife collider after the swing
        isSwinging = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // This method needs to be on the Knife GameObject for collision detection
        if (isSwinging && other.CompareTag("Enemy"))
        {
            // Attempt to get the Enemy script on the collided object
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(knifeDamage);
            }
        }
    }
}
