using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText;
    private int score;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: 0";
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
    }

    public void removePoints(int points)
    {
        score = score - points;
        UpdateText();
    }

}