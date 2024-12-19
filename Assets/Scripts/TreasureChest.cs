using UnityEngine;
using System.Collections;

public class TreasureChest : MonoBehaviour
{
    private Animator animator;
    private bool canOpen = false; // Tracks if the player is in range to open the chest

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check if the player presses 'E' and is in range
        if (canOpen && Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = true;
            Debug.Log("Player in range. Press 'E' to open the chest.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = false;
            Debug.Log("Player out of range.");
        }
    }

    private void OpenChest()
    {
        Debug.Log("Opening the treasure chest...");
        animator.SetTrigger("OpenChest");
        StartCoroutine(LoadEndingScene());
    }

    private IEnumerator LoadEndingScene()
    {
        yield return new WaitForSeconds(2f); // Wait for the open animation to complete
        Debug.Log("Loading ending scene...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Ending"); // Replace with your scene name
    }
}
