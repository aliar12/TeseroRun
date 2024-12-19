using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    private bool isNPCActive = false; // Tracks if an NPC is active

    void Start()
    {
        scoreText.text = "Coins: " + score;
        keyText.text = "Remaining Keys: " + keyScore;
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
        score = score - points;
        UpdateText();
    }

    public void AddKey(int points)
    {
        keyScore = keyScore - points;
        UpdateText();

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
