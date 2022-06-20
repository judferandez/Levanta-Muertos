using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health = 1;
    [SerializeField] float speed = 2;
    [SerializeField] int scorePoint = 5;
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
        view = GetComponent<PhotonView>();
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    private void FixedUpdate()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
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

    public int TakeDamage()
    {
        //int viewId = Player.GetComponent<PhotonView>().ViewID;
        view.RPC("OnTakeDamage", RpcTarget.AllViaServer);
        return scorePoint;
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

    [PunRPC]
    public void OnTakeDamage()
    {
        AudioSource.PlayClipAtPoint(impactClip, transform.position);

        if (view.IsMine)
        {
            health--;
            if (health <= 0)
            {
                //Ths need to be done on all clients -> Move this to a new RPC?
                //GameManager.Instance.Score += scorePoint;
                AudioSource.PlayClipAtPoint(enemyDeathClip, transform.position);
                //
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

}
