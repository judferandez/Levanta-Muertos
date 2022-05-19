using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] Text healthText;
    [SerializeField] Text scoreText;
    [SerializeField] Text timeText;
    [SerializeField] Text finalScore;
    [SerializeField] GameObject gameOverScreen;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public void UpdateUIScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }

    public void UpdateUIHealth(int newHealth)
    {
        scoreText.text = newHealth.ToString();
    }

    public void UpdateUITime(int newTime)
    {
        scoreText.text = newTime.ToString();
    }

    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        finalScore.text = "" + GameManager.Instance.Score;
    }
}
