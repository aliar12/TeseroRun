using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText;
    private int score;
    public TMP_Text keyText;
    private int keyScore = 10;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: 0";
        keyText.text = "Remaining Keys: 10";
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // You can add any update logic here if needed
    }
    public void AddScore(int points)
    {
        score += points;
        UpdateText();
    }
    void UpdateText()
    {
        scoreText.text = "Score: " + score;
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
    }

}