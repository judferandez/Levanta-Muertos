using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void CreateRoom(){
        PhotonNetwork.CreateRoom("Lobby1");
    }

    public void JoinRoom(){
        PhotonNetwork.JoinRoom("Lobby1");
    }

    public override void OnJoinedRoom(){
        PhotonNetwork.LoadLevel("Game");
    }

    void Update()
    {
        
    }
}
