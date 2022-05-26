using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health = 1;
    [SerializeField] float speed = 2;
    [SerializeField] int scorePoint = 100;
    [SerializeField] AudioClip impactClip;
    [SerializeField] AudioClip enemyDeathClip;
    Transform player;

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
        transform.position = spawnPoints[randomSpawnPoint].transform.position;
    }

    private void Update()
    {
        Vector2 direction = player.position - transform.position;
        transform.position += (Vector3)direction.normalized * Time.deltaTime * speed;
    }
    
    public void TakeDamage()
    {
        health--;
        AudioSource.PlayClipAtPoint(impactClip, transform.position);
        if(health <= 0)
        {
            GameManager.Instance.Score += scorePoint;
            AudioSource.PlayClipAtPoint(enemyDeathClip, transform.position);
            Destroy(gameObject, 0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage();
        }
    }
}
