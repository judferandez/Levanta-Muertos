using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//holi
public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 8;
    [SerializeField] int health = 3;
    public bool powerShot;
    PhotonView view;
    float time;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        time = 0;
        
    }

    void Update()
    {
        if(view.IsMine){
            transform.position += transform.right * Time.deltaTime * speed;
        }
        time += Time.deltaTime;
        if(time>5){
            PhotonNetwork.Destroy(gameObject);
        } 
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage();

            if (!powerShot)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            health--;

            if(health <= 0)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        if (collision.CompareTag("Wall"))
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
