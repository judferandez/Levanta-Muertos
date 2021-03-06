using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    bool gunLoaded = true;
    bool powerShotEnabled;
    float h;
    float v;
    public float speed = 2.5f;

    Vector3 moveDirection;
    Vector2 facingDirection;
    PhotonView view;
    GameObject powerUp;

    [SerializeField] float fireRate = 5;
    [SerializeField] float fireRateTime = 6;
    [SerializeField] float invulnerableTime = 3;
    [SerializeField] float powerShootTime = 5;
    [SerializeField] int maxAmountOfBullets = 120;
    [SerializeField] int minAmountOfBullets = 30;
    [SerializeField] int health = 10;
    [SerializeField] int ammo = 60;
    [SerializeField] float blinkRate = 0.01f;
    [SerializeField] bool invulnerable;
    [SerializeField] Transform aim;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] AudioClip itemClip;
    [SerializeField] CinemachineVirtualCamera vcam;




    public int Health
    {
        get => health;
        set
        {
            health = value;
            UIManager.Instance.UpdateUIHealth(health);
        }
    }

    public int Ammo
    {
        get=> ammo;
        set
        {
            ammo = value;
            UIManager.Instance.UpdateUIAmmo(ammo);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        mainCamera = Camera.main;
        if (view.IsMine)
        {
            vcam.gameObject.SetActive(true);
            vcam.Follow = transform;
            vcam.LookAt = transform;
        }
        else{
            vcam.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(view.IsMine){
            vcam.Follow = transform;
            vcam.LookAt = transform;
            ReadInput();

            // Player movement
            transform.position += moveDirection * Time.deltaTime * speed;

            // Aim Movement
            facingDirection = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            // Shooting Control
            if (Input.GetMouseButton(0) && gunLoaded && Ammo > 0)
            {
                Shoot();
            }
        }
        
        
        //Update Graphics: This should be done on all clients
        aim.position = transform.position + (Vector3)facingDirection.normalized;
        UpdatePlayerGraphics();
        
    }

    void UpdatePlayerGraphics()
    {
        anim.SetFloat("Speed", moveDirection.magnitude);
        if (aim.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else if (aim.position.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
    }

    void Shoot()
    {
        gunLoaded = false;
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        GameObject bulletClone = PhotonNetwork.Instantiate(bulletPrefab.name,transform.position,targetRotation);
        if (powerShotEnabled)
        {
            bulletClone.GetComponent<Bullet>().powerShot = true;
        }
        Ammo--;
        StartCoroutine(ReloadGun());
    }

    void ReadInput()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        moveDirection.x = h;
        moveDirection.y = v;
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
            UIManager.Instance.ShowGameOverScreen();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(view.IsMine){
            if (collision.CompareTag("PowerUp"))
            {
                switch (collision.GetComponent<PowerUp>().powerUpType)
                {
                    case PowerUp.PowerUpType.FireRateIncrease:
                        fireRate +=3;
                        StartCoroutine(DisableFireIncrease());
                        break;
                    case PowerUp.PowerUpType.PowerShot:
                        powerShotEnabled = true;
                        StartCoroutine(DisablePowerShoot());
                        break;
                    case PowerUp.PowerUpType.Ammobox:
                        int randomAmountOfBullets = Random.Range(minAmountOfBullets, maxAmountOfBullets);
                        Ammo += randomAmountOfBullets;
                        break;
                    case PowerUp.PowerUpType.Medkit:
                        Health++;
                        break;
                }
                AudioSource.PlayClipAtPoint(itemClip, transform.position);
                collision.GetComponent<PowerUp>().OnPowerUpcollected();
            }
        }
    }

    IEnumerator MakeVulnerableAgain()
    {
        StartCoroutine(BlinkRoutine());
        yield return new WaitForSeconds(invulnerableTime);
        invulnerable = false;
    }

    IEnumerator BlinkRoutine()
    {
        int numberOfBlinks = 10;
        while (numberOfBlinks > 0)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(numberOfBlinks * blinkRate);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(numberOfBlinks * blinkRate);
            numberOfBlinks--;
        }
    }

    IEnumerator DisablePowerShoot()
    {
        yield return new WaitForSeconds(powerShootTime);
        powerShotEnabled = false;
    }

    IEnumerator DisableFireIncrease()
    {
        yield return new WaitForSeconds(fireRateTime);
        fireRate--;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //Send data to other clients
        if (stream.IsWriting)
        {
            stream.SendNext(moveDirection);
            stream.SendNext(facingDirection);
        }
        //Copy data from
        else if(stream.IsReading)
        {
            moveDirection = (Vector3) stream.ReceiveNext();
            facingDirection = (Vector2) stream.ReceiveNext();
        }
    }
}
