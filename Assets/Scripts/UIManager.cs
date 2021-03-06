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
    [SerializeField] Text ammoText;
    [SerializeField] AudioClip gameOverClip;
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
        healthText.text = newHealth.ToString();
    }

    public void UpdateUITime(int newTime)
    {
        timeText.text = newTime.ToString();
    }

    public void UpdateUIAmmo(int newAmmo)
    {
        ammoText.text = newAmmo.ToString();
    }

    public void ShowGameOverScreen()
    {
        AudioSource.PlayClipAtPoint(gameOverClip, transform.position);
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
        finalScore.text = "Score: " + GameManager.Instance.Score;
    }
}
