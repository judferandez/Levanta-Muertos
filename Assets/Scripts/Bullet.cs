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
    int viewID;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        time = 0;
        
    }

    void Update()
    {
        if(view.IsMine){
            transform.position += transform.right * Time.deltaTime * speed;
            time += Time.deltaTime;
            if(time>5){
                PhotonNetwork.Destroy(gameObject);
            } 
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            viewID = collision.GetComponent<PhotonView>().ViewID;
            collision.GetComponent<Enemy>().TakeDamage();

            if (!powerShot)
            {
                photonView.RPC("DestroyBullet", RpcTarget.MasterClient, viewID);
            }
            health--;

            if(health <= 0)
            {
                photonView.RPC("DestroyBullet", RpcTarget.MasterClient, viewID);
            }
        }
        if (collision.CompareTag("Wall"))
        {
            photonView.RPC("DestroyBullet", RpcTarget.MasterClient, viewID);
        }
    }


    [PunRPC]
    public void DestroyBullet(int viewID)
    {
        PhotonNetwork.Destroy(PhotonView.Find(viewID).gameObject);
    }
}
