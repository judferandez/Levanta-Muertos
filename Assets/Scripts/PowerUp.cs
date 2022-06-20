using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PowerUp : MonoBehaviour
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
            PhotonNetwork.Destroy(gameObject);
        }
    }


    public PowerUpType powerUpType;
}
