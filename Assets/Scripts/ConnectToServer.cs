using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    float time;
    // Start is called before the first frame update
    void Start()
    {
        time =0;
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update(){
        time +=  Time.deltaTime;
        if(time > 10){
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("MainMenu");
            time =0;
        }
    }

    // Update is called once per frame
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby"); 
    }
}
