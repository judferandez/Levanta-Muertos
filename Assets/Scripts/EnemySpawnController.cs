using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefab;
    [Range(1, 10)][SerializeField] float spawnRate = 1;
    
    GameObject[] spawnPoints;
    int randomSpawnPoint;
    Vector3 spawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(1/spawnRate);
            float random = Random.Range(0.0f, 1.0f);

            randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            spawnPoint = spawnPoints[randomSpawnPoint].transform.position;


            if(random < GameManager.Instance.difficulty * 0.1f)
            {
                PhotonNetwork.Instantiate(enemyPrefab[0].name,spawnPoint,Quaternion.identity);
            } else
            {
                PhotonNetwork.Instantiate(enemyPrefab[1].name,spawnPoint,Quaternion.identity);
            }
        }
    }
}
