using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PowerUp : MonoBehaviourPunCallbacks
{
    [SerializeField] int itemAvaibleTimeMax = 6;
    [SerializeField] int itemAvaibleTimeMin = 4;
    PhotonView view;

    void Start(){
        view = GetComponent<PhotonView>();
        StartCoroutine(DestroyItemRutine());
    }

    public enum PowerUpType
    {
        FireRateIncrease,
        PowerShot,
        Medkit,
        Ammobox
    }

    IEnumerator DestroyItemRutine()
    {
        while (true)
        {
            int randomTime = Random.Range(itemAvaibleTimeMin, itemAvaibleTimeMax);
            yield return new WaitForSeconds(randomTime);
            Debug.Log("si esta por acá");
            DeletePowerUp();
        }
    }

    void DeletePowerUp(){
        Debug.Log("llego para destruir afuera");
        view.RPC("DeletePowerUpRPC", RpcTarget.MasterClient);
    }

    public void OnPowerUpcollected(){
        view.RPC("DeletePowerUpRPC", RpcTarget.MasterClient);
    }

    [PunRPC]
    void DeletePowerUpRPC(){
        //if (view.IsMine)
        //{
            PhotonNetwork.Destroy(gameObject);
        //}
    }



    public PowerUpType powerUpType;
}
