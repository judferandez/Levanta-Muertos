using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class SpawnPlayers : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject playerPrefab;

    [SerializeField]
    private float minX;
    [SerializeField]
    private float maxX;
    [SerializeField]
    private float minY;
    [SerializeField]
    private float maxY;

    
    void Start()
    {
        Vector2 randomPosition = new Vector2 (Random.Range(minX,maxX),Random.Range(minY,maxY));
        PhotonNetwork.Instantiate(playerPrefab.name,randomPosition,Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
