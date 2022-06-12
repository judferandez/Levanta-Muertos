using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health = 1;
    [SerializeField] float speed = 2;
    [SerializeField] int scorePoint = 100;
    [SerializeField] AudioClip impactClip;
    [SerializeField] AudioClip enemyDeathClip;
    GameObject[] players;
    Vector2 direction;
    Vector2 predirection;
    PhotonView view;
    float distance1;
    float distance2;



    private void Start()
    {
        view = view = GetComponent<PhotonView>();
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    private void Update()
    {
        if(view.IsMine){
            direction = players[0].transform.position-transform.position;
            for(int i=1;i<players.Length;i++){
                distance1 = Vector3.Distance(direction,transform.position);
                distance2 = Vector3.Distance(players[i].transform.position,transform.position);
                if(distance2<distance1){
                    direction = players[i].transform.position - transform.position;
                }
            }
        transform.position += (Vector3)direction.normalized * Time.deltaTime * speed;
        }
        
    }
    
    public void TakeDamage()
    {
        if(view.IsMine){
            health--;
            AudioSource.PlayClipAtPoint(impactClip, transform.position);
            if(health <= 0)
            {
                GameManager.Instance.Score += scorePoint;
                AudioSource.PlayClipAtPoint(enemyDeathClip, transform.position);
                PhotonNetwork.Destroy(gameObject);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(view.IsMine){
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Player>().TakeDamage();
            }
        }
        
    }
}
