using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText;
    private static int score;
    public TMP_Text keyText;
    private static int keyScore = 10;
    private Rigidbody2D rb;
    public GameObject NPCHelper;
    public GameObject NPCPrefab;
    public static int LevelScore = 0;
    public static GameManager Instance;

    private GameObject currentNPC; // Tracks the currently spawned NPC

    private string[] sceneNames = { "Level1", "Level3" };

    // Dark overlay and boss spawn references
    public GameObject darkOverlay; // Reference to the dark overlay GameObject
    public GameObject bossSpawnScene; // Reference to the boss spawn GameObject
    private BossController bossController; // Reference to BossController
    public Slider bossHealthBar; // Reference to the health bar UI element
    public int totalCoins; // Total number of coins in the level
    private bool bossSpawned = false; // To track if the boss has been spawned

    // Start is called before the first frame update
    private bool isNPCActive = false; // Tracks if an NPC is active

    void Start()
    {
        scoreText.text = "Coins: " + score;
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

    void Update()
    {
        // Check if all keys are collected and trigger boss spawn
        if (keyScore <= 0 && !bossSpawned)
        {
            TriggerBossSpawn(); // Trigger the boss spawn
        // Check if the GameManager is active
        if (!gameObject.activeSelf)
        {
            Debug.LogWarning("GameManager is inactive. Update logic will not execute.");
            return;
        }

        // Enable NPC Helper if score is 10 and no NPC is currently active
        if (score >= 80)
        {
            NPCHelper.gameObject.SetActive(true);
        }
        else 
        {
            NPCHelper.gameObject.SetActive(false);
        }

    }

    public int GetKeyScore() // New method to get the current score
    {
        return keyScore;
    }

    public void setKeyScore(int newKeyScore) // New method to get the current score
    {
        keyScore = newKeyScore;
        UpdateText();
    }

    public int GetScore() // New method to get the current score
    {
        return score;
    }

    public void setScore(int newScore) // New method to get the current score
    {
        score = newScore;
        UpdateText();
    }

    public void AddScore(int points)
    {
        score += points; // Increment coins
        UpdateText();
    }

    void UpdateText()
    {
        scoreText.text = "Coins: " + score;
        keyText.text = "Remaining Keys: " + keyScore;
    }

    public void RemovePoints(int points)
    {
        return score; // Return the current score as coins
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
        score = Mathf.Max(0, score); // Ensure score doesn't go negative
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
            Transform bossTransform = bossSpawnScene.transform;
            bossTransform.position = new Vector3(112f, 9f, 0f);

            Debug.Log($"Boss spawned at position: {bossTransform.position}");

            if (bossController != null)
            {
                bossController.enabled = true;
                bossController.StartBossBehavior();

                if (bossHealthBar != null)
                {
                    bossHealthBar.gameObject.SetActive(true);
                    bossHealthBar.maxValue = bossController.GetMaxHealth();
                    bossHealthBar.value = bossController.GetCurrentHealth();
                }
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
        //if (keyScore <= 0)
        //{
        //    LoadNextScene();
        //    keyScore = 10;
        //}
    }

    //void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}
