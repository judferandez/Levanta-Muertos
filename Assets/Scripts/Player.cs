using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool gunLoaded = true;
    float h;
    float v;
    [SerializeField] float fireRate = 1;
    [SerializeField] float speed = 3;
    [SerializeField] int health = 10;
    Vector3 moveDirection;
    Vector2 facingDirection;
    [SerializeField] Transform aim;
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Player Movement
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        moveDirection.x = h;
        moveDirection.y = v;

        transform.position += moveDirection * Time.deltaTime * speed;

        // Aim Movement
        facingDirection = mainCamera.ScreenToWorldPoint(Input.mousePosition - transform.position);
        aim.position = transform.position + (Vector3)facingDirection.normalized;

        // Shooting Control
        if (Input.GetMouseButton(0) && gunLoaded)
        {
            gunLoaded = false;
            float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Instantiate(bulletPrefab, transform.position, targetRotation);
            StartCoroutine(ReloadGun());
        }
    }

    IEnumerator ReloadGun()
    {
        yield return new WaitForSeconds(1/fireRate);
        gunLoaded = true;
    }

    public void TakeDamage()
    {
        health--;
        if(health <= 0)
        {
            // GameOver
        }
    }
}
