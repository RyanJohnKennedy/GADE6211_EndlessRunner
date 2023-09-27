using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public int maxHealth;
    public AudioSource fireSound;
    public AudioSource hit;
    [HideInInspector]
    public int health;

    [Header("Movement")]
    public float movementSpeed;
    public int playAreaX, playAreaZ;
    public GameObject PlayerBody;
    Vector3 playAreaPos;

    [Header("Gun Settings")]
    public int fireSpeed;
    public int bulletSpeed;
    private int fireCounter;
    public GameObject bulletSpawn;
    public GameObject bulletDirection;

    [Header("PickUp Settings")]
    public GameObject shield;
    public int ShieldMaxHealth;
    public int powerUpTime = 1000;
    [HideInInspector]
    public bool fireRatePowerUp = false;
    [HideInInspector]
    public int fireRateCounter;
    int fireRateIncrease = 10;
    [HideInInspector]
    public bool shieldPowerUp = false;
    [HideInInspector]
    public int shieldCounter;

    private LevelController lc;

    private void Start()
    {
        fireRateCounter = powerUpTime;
        shieldCounter = powerUpTime;
        //health = maxHealth;
        fireCounter = fireSpeed;
        lc = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    private void LateUpdate()
    {
        KeepPlayerInPlayArea();
    }

    private void FixedUpdate()
    {
        Shoot();
        ClearPowerUps();
    }

    public void LoseHealth(int damage)
    {
        health -= damage; 
    }

    void Shoot()
    {
        if (fireCounter == fireSpeed)
        {
            if (Input.GetAxis("Fire1") == 1)
            {
                lc.Shoot(bulletSpawn, new Vector3(bulletDirection.transform.position.x, bulletDirection.transform.position.y, bulletDirection.transform.position.z), 'H', bulletSpeed);
                fireSound.Play();
                fireCounter = 0;
            }
        }
        else
        {
            fireCounter++;
        }
    }

    void MovePlayer()
    {
        float Horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        PlayerBody.transform.rotation = Quaternion.Euler(-90f, Input.GetAxis("Horizontal") * 40f, 0f);
        float Vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

        transform.Translate(Horizontal, 0f, Vertical);
    }

    void KeepPlayerInPlayArea()
    {
        playAreaPos = transform.position;
        playAreaPos.x = Mathf.Clamp(playAreaPos.x, -playAreaX, playAreaX);
        playAreaPos.z = Mathf.Clamp(playAreaPos.z, -playAreaZ, playAreaZ);
        transform.position = playAreaPos;
    }

    public void IncreaseFireSpeed()
    {
        if (!fireRatePowerUp)
        {
            fireSpeed = fireSpeed - fireRateIncrease;
            fireCounter = fireSpeed;
            fireRatePowerUp = true;
        }
        else
        {
            fireRateCounter = powerUpTime;
        }
    }

    public void ShieldOn()
    {
        if (!shieldPowerUp)
        {
            shield.SetActive(true);
            shield.GetComponent<Shield>().ResetHealth(ShieldMaxHealth);
            shieldPowerUp = true;
        }
        else
        {
            shieldCounter = powerUpTime;
        }
    }

    public void ClearPowerUps()
    {
        if(fireRatePowerUp && fireRateCounter <= 0)
        {
            fireSpeed = fireSpeed + fireRateIncrease;
            fireRateCounter = powerUpTime;
            fireRatePowerUp = false;
        }
        else if (fireRatePowerUp)
        {
            fireRateCounter--;
        }

        if (shieldPowerUp && shieldCounter <= 0)
        {
            shield.SetActive(false);
            shieldCounter = powerUpTime;
            shieldPowerUp = false;
        }
        else if (shieldPowerUp)
        {
            shieldCounter--;
        }
    }
}
