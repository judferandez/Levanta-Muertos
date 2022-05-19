using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool gunLoaded = true;
    float h;
    float v;
    [SerializeField] float fireRate = 1;
    [SerializeField] float speed = 2.5f;
    [SerializeField] float invulnerableTime = 3;
    [SerializeField] int health = 10;
    bool powerShotEnabled;
    [SerializeField] bool invulnerable;
    Vector3 moveDirection;
    Vector2 facingDirection;
    [SerializeField] Transform aim;
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform bulletPrefab;

    public int Health
    {
        get => health;
        set
        {
            health = value;
            //UIManager.Instance.UpdateUIHealth(health);
        }
    }
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
        facingDirection = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        aim.position = transform.position + (Vector3)facingDirection.normalized;

        // Shooting Control
        if (Input.GetMouseButton(0) && gunLoaded)
        {
            gunLoaded = false;
            float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Transform bulletClone = Instantiate(bulletPrefab, transform.position, targetRotation);
            if (powerShotEnabled)
            {
                bulletClone.GetComponent<Bullet>().powerShot = true;
            }
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
        if (invulnerable)
        {
            return;
        }

        Health--;
        invulnerable = true;

        StartCoroutine(MakeVulnerableAgain());
        if (Health <= 0)
        {
            GameManager.Instance.gameOver = true;
            //UIManager.Instance.ShowGameOverScreen();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            switch (collision.GetComponent<PowerUp>().powerUpType)
            {
                case PowerUp.PowerUpType.FireRateIncrease:
                    fireRate++;
                    break;
                case PowerUp.PowerUpType.PowerShot:
                    powerShotEnabled = true;
                    break;
            }
            Destroy(collision.gameObject, 0.1f);
        }
        if(collision.CompareTag("Water"))
        {
            speed = 0.3f;
            StartCoroutine(MakeDriedAgain());
        }
    }

    IEnumerator MakeVulnerableAgain()
    {
        yield return new WaitForSeconds(invulnerableTime);
        invulnerable = false;
    }

    IEnumerator MakeDriedAgain()
    {
        while(speed < 2.5f)
        {
            yield return new WaitForSeconds(0.2f);
            speed = speed + 0.1f;
        }
    }
}
