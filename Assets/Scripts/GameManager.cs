using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText;
    private int score;
    public TMP_Text keyText;
    private int keyScore = 10;
    private Rigidbody2D rb;
    public GameObject NPCHelper;
    public GameObject NPCPrefab;

    private GameObject currentNPC; // Tracks the currently spawned NPC

    private string[] sceneNames = { "Level1", "Level3" };

    private bool isNPCActive = false; // Tracks if an NPC is active

    void Start()
    {
        scoreText.text = "Coins: 0";
        keyText.text = "Remaining Keys: 10";
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if the GameManager is active
        if (!gameObject.activeSelf)
        {
            Debug.LogWarning("GameManager is inactive. Update logic will not execute.");
            return;
        }

        // Check if 10 keys have been collected and move to the next level
        if (keyScore <= 0)
        {
            LoadNextLevel();
        }

        // Enable NPC Helper if score is 10 and no NPC is currently active
        if (score >= 10)
        {
            NPCHelper.gameObject.SetActive(true);
        }

    }


    public void AddScore(int points)
    {
        score += points;
        UpdateText();
    }

    void UpdateText()
    {
        scoreText.text = "Coins: " + score;
        keyText.text = "Remaining Keys: " + keyScore;
    }

    public void RemovePoints(int points)
    {
        score = score -= points;
        UpdateText();
    }

    public void AddKey(int points)
    {
        keyScore = keyScore - points;
        UpdateText();

        if (keyScore <= 0)
        {
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        int currentLevelIndex = System.Array.IndexOf(sceneNames, SceneManager.GetActiveScene().name);
        int nextLevelIndex = currentLevelIndex + 1;

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
