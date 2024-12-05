using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText;
    private int score;
    public TMP_Text keyText;
    private int keyScore = 10;
    private Rigidbody2D rb;

    private string[] sceneNames = { "Level1", "Level3" };

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Coins: 0";
        keyText.text = "Remaining Keys: 10";
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if 10 keys have been collected and move to next level
        if (keyScore <= 0)
        {
            LoadNextLevel();
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

    public void removePoints(int points)
    {
        score = score - points;
        UpdateText();
    }

    public void AddKey(int points)
    {
        keyScore = keyScore - points;
        UpdateText();

        // Check if 10 keys are collected and move to next level
        if (keyScore <= 0)
        {
            LoadNextLevel();
        }
    }
    void LoadNextLevel()
    {
        int currentLevelIndex = System.Array.IndexOf(sceneNames, SceneManager.GetActiveScene().name); // Get current scene index
        int nextLevelIndex = currentLevelIndex + 1; // Increment to load the next scene

        // Check if the next level exists in the array
        if (nextLevelIndex < sceneNames.Length)
        {
            // Load the next scene by name
            SceneManager.LoadScene(sceneNames[nextLevelIndex]);
        }
        else
        {
            // If no more levels, show a message or restart the game
            Debug.Log("Game Over or All Levels Complete!");
            // Optionally restart from the first level (Level1)
            SceneManager.LoadScene(sceneNames[0]);
        }
    }
}