using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int gameTime = 60;
    public float difficulty = 0.1f;
    public bool gameOver;
    [SerializeField] int score;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            UIManager.Instance.UpdateUIScore(score);
            if (score % 1000 == 0)
            {
                difficulty++;
            }
        }
    }

    public int GameTime
    {
        get => gameTime;
        set
        {
            gameTime = value;
            UIManager.Instance.UpdateUITime(gameTime);
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UIManager.Instance.UpdateUITime(GameTime);
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        while (GameTime > 0)
        {
            yield return new WaitForSeconds(1);
            GameTime--;
        }

        gameOver = true;
        UIManager.Instance.ShowGameOverScreen();
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }
}
