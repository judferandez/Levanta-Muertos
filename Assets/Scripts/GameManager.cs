using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private const byte INIT_STATE = 1;
    private const byte SCORE_EVENT_ID = 2;
    private const byte TIME_EVENT_ID = 3;
    
    public static GameManager Instance;
    
    
    public int gameTime = 60;
    public float difficulty = 0.1f;
    public bool gameOver;
    [SerializeField] int score;

    public int Score => score;
    
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

    public void AddScore(int points)
    {
        //Only the server should handle the score
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        
        score++;

        //Update local UI
        UIManager.Instance.UpdateUIScore(score);
        if (score % 1000 == 0)
        {
            difficulty++;
        }

        //Send data to all clients
        SendScoreEvent();
    }

    public void AddTime(int seconds)
    {
        //Only the server should handle the score
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        
        gameTime+=seconds;

        //Update local UI
        UIManager.Instance.UpdateUITime(gameTime);

        //Send data to all clients
        SendTimeEvent();
    }
    
    #region PUN CALLBACKS

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        //Send the current state to the new player connected. 
        if (!newPlayer.IsMasterClient && PhotonNetwork.IsMasterClient)
        {
            SendInitState();
        }
    }

    #endregion
    
    

    #region Events

    private void SendInitState()
    {
        object[] content = new object[] { score, difficulty, gameTime };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(INIT_STATE, content, raiseEventOptions, SendOptions.SendReliable);
    }

    private void SendScoreEvent()
    {
        object[] content = new object[] { score, difficulty };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(SCORE_EVENT_ID, content, raiseEventOptions, SendOptions.SendReliable);
    }

    
    private void SendTimeEvent()
    {
        object[] content = new object[] { gameTime };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(TIME_EVENT_ID, content, raiseEventOptions, SendOptions.SendReliable);
    }
    
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == INIT_STATE)
        {
            OnInitStateEvent(photonEvent);
        }
        else if (eventCode == SCORE_EVENT_ID)
        {
            OnScoreEvent(photonEvent);
        }
        else if(eventCode == TIME_EVENT_ID){
            OnTimeEvent(photonEvent);
        }
    }
    
    private void OnInitStateEvent(EventData photonEvent)
    {
        object[] data = (object[])photonEvent.CustomData;

        int serverScore = (int)data[0];
        float serverDifficulty = (float)data[1];
        int serverGameTime = (int)data[2];

        score = serverScore;
        difficulty = serverDifficulty;
        gameTime = serverGameTime;
            
        UIManager.Instance.UpdateUIScore(score);
    }
    
    private void OnScoreEvent(EventData photonEvent)
    {
        object[] data = (object[])photonEvent.CustomData;

        int serveScore = (int)data[0];
        float serveDifficulty = (float)data[1];

        score = serveScore;
        difficulty = serveDifficulty;
            
        UIManager.Instance.UpdateUIScore(score);
    }

    

    private void OnTimeEvent(EventData photonEvent)
    {
        object[] data = (object[])photonEvent.CustomData;

        int serverTime = (int)data[0];

        gameTime = serverTime;
            
        UIManager.Instance.UpdateUITime(gameTime);
    }
    #endregion
    
}
