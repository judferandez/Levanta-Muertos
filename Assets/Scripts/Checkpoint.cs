using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] int addedTime = 20;
    [SerializeField] AudioClip itemClip; 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.GameTime += addedTime;
            AudioSource.PlayClipAtPoint(itemClip, transform.position);
            Destroy(gameObject, 0.1f);
        }
    }
}
