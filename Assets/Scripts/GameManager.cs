using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText;
    private int score;
    public TMP_Text keyText;
    private int keyScore = 10; // Number of keys to collect
    private Rigidbody2D rb;

    private string[] sceneNames = { "Level1", "Level3" };

    // Dark overlay and boss spawn references
    public GameObject darkOverlay; // Reference to the dark overlay GameObject
    public GameObject bossSpawnScene; // Reference to the boss spawn GameObject
    private BossController bossController; // Reference to BossController
    public Slider bossHealthBar; // Reference to the health bar UI element
    public int totalCoins; // Total number of coins in the level
    private bool bossSpawned = false; // To track if the boss has been spawned

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Coins: 0";
        keyText.text = "Remaining Keys: " + keyScore;
        rb = GetComponent<Rigidbody2D>();

        // Ensure dark overlay and boss spawn are properly set up
        if (darkOverlay != null)
        {
            darkOverlay.SetActive(true); // Dark overlay starts active
        }
        if (bossSpawnScene != null)
        {
            bossSpawnScene.SetActive(false); // Boss spawn scene starts inactive
            bossController = bossSpawnScene.GetComponentInChildren<BossController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if all keys are collected and trigger boss spawn
        if (keyScore <= 0 && !bossSpawned)
        {
            TriggerBossSpawn(); // Trigger the boss spawn
        }
    }

    public void AddScore(int points)
    {
        score += points; // Increment coins
        UpdateText();

        // Debug for clarity
        if (score >= totalCoins)
        {
            Debug.Log("All coins collected, but boss spawn depends on keys.");
        }
    }

    public void AddKey(int points)
    {
        keyScore -= points; // Decrement keys
        UpdateText();

        Debug.Log($"Current Keys: {Mathf.Max(0, keyScore)}");

        // Check if all keys are collected and trigger boss spawn
        if (keyScore <= 0 && !bossSpawned)
        {
            TriggerBossSpawn();
        }
    }

    void UpdateText()
    {
        scoreText.text = "Coins: " + score;
        keyText.text = "Remaining Keys: " + Mathf.Max(0, keyScore);
    }

    public void removePoints(int points)
    {
        score -= points; // Decrement coins
        UpdateText();
    }

    private void TriggerBossSpawn()
    {
        bossSpawned = true; // Mark the boss as spawned to prevent multiple triggers

        Debug.Log("Boss Spawn Triggered!");

        // Remove the dark overlay
        if (darkOverlay != null)
        {
            darkOverlay.SetActive(false);
            Debug.Log("Dark overlay removed!");
        }

        // Delay the boss spawn by 3 seconds
        StartCoroutine(DelayedBossSpawn());
    }

    private IEnumerator DelayedBossSpawn()
    {
        Debug.Log("Boss will spawn after 3 seconds...");
        yield return new WaitForSeconds(3);

        if (bossSpawnScene != null)
        {
            bossSpawnScene.SetActive(true);
            Debug.Log("Boss has spawned!");

            // Enable the BossController script
            if (bossController != null)
            {
                bossController.enabled = true; // Enable boss logic
                bossController.StartBossBehavior(); // Trigger the boss behavior

                // Enable and initialize the health bar
                if (bossHealthBar != null)
                {
                    bossHealthBar.gameObject.SetActive(true);
                    bossHealthBar.maxValue = bossController.GetMaxHealth();
                    bossHealthBar.value = bossController.GetCurrentHealth();
                }
            }
            else
            {
                Debug.LogWarning("BossController script not found on spawned Boss!");
            }
        }
    }


    void LoadNextLevel()
    {
        if (bossSpawned)
        {
            Debug.Log("Level transition blocked: Boss battle active.");
            return; // Prevent transition during the boss battle
        }

        int currentLevelIndex = System.Array.IndexOf(sceneNames, SceneManager.GetActiveScene().name); // Get current scene index
        int nextLevelIndex = currentLevelIndex + 1; // Increment to load the next scene

        // Check if the next level exists in the array
        if (nextLevelIndex < sceneNames.Length)
        {
            SceneManager.LoadScene(sceneNames[nextLevelIndex]);
        }
        else
        {
            Debug.Log("Game Over or All Levels Complete!");
            SceneManager.LoadScene(sceneNames[0]);
        }
    }
}
