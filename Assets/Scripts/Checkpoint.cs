using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] int addedTime = 20;
    [SerializeField] AudioClip itemClip;
    [SerializeField] int itemAvaibleTimeMax = 6;
    [SerializeField] int itemAvaibleTimeMin = 4;
    PhotonView view;


    void Start(){
        view = GetComponent<PhotonView>();
        StartCoroutine(DestroyItemRutine());
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(view.IsMine){
            if (collision.CompareTag("Player"))
            {
                //if(collision.GetComponent<PhotonView>().IsMine){
                GameManager.Instance.AddTime(addedTime);
                AudioSource.PlayClipAtPoint(itemClip, transform.position);

                DeleteCheckPoint(gameObject);
                //}
                
            }
        }
        
    }

    IEnumerator DestroyItemRutine()
    {
        while (true)
        {
            int randomTime = Random.Range(itemAvaibleTimeMin, itemAvaibleTimeMax);
            yield return new WaitForSeconds(randomTime);
            DeleteCheckPoint(gameObject);
        }
    }

    void DeleteCheckPoint(GameObject PowerUp){
        view.RPC("DeleteCheckPointRPC", RpcTarget.All);
    }

    [PunRPC]
    void DeleteCheckPointRPC(){
        if (view.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }


}
