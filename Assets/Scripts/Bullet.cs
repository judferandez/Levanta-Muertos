using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//holi
public class Bullet : MonoBehaviourPunCallbacks
{
    [SerializeField] float speed = 8;
    [SerializeField] int health = 3;
    public bool powerShot;
    PhotonView view;
    float time = 0;

    //Avoid calling the Destroy method twice
    private bool _isDestroyed = false;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (_isDestroyed)
        {
            return;
        }
        
        if(view.IsMine){
            transform.position += transform.right * Time.deltaTime * speed;
            time += Time.deltaTime;
            if(time > 5){
                DestroyBullet();
            } 
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isDestroyed || view == null || !view.IsMine)
        {
            return;
        }
        
        PhotonView photonView = PhotonView.Get(this);
        if (collision.CompareTag("Enemy"))
        {
            int score = collision.GetComponent<Enemy>().TakeDamage();
            GameManager.Instance.Score += score;
            health--;

            if(!powerShot || health <= 0)
            {
                DestroyBullet();
            }
        }
        
        if (collision.CompareTag("Wall"))
        {
            DestroyBullet();
        }
    }
    
    void DestroyBullet()
    {
        _isDestroyed = true;
        PhotonNetwork.Destroy(gameObject);
    }
}
