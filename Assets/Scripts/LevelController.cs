using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject bulletGameobject;
    public PlayerController player;
    public Transform mousePoint;
    public GameObject mousePointer;
    public PlayArea playArea;
    
    [Header("Platform")]
    public GameObject[] platformGameobjects;
    public GameObject lastPlatform;
    public float gameSpeed;

    [Header("UI")]
    public Text scoreText;
    public Slider healthBar;
    public Image shieldBar;
    public Image fireRateBar;
    public Slider bossHealthBar;
    public GameObject PauseMenu;

    [Header("Sound")]
    public AudioSource music;
    private float musicVolume;
    public AudioSource gunFire;
    private float gunFireVolume;

    [Header("Pick Up Settings")]
    public GameObject[] pickUps;
    public float pickUpChance;
    public int pickUpTime;
    int pickUpCounter = 0;

    [Header("Enemy Settings")]
    public int maxEnemies;
    public int minEnemies;
    public int enemySpawnTimer;
    public GameObject[] enemyGameobjects;
    int enemySpawnCounter = 200;
    public Material bulletMaterial;

    [Header("Enemy Chances")]
    public float enemy1Chance;
    public float enemy2Chance;

    [Header("Boss Settings")]
    public bool boss = false;
    public GameObject bossGameobject;
    public BossController bc;
    public int spawnsUntilBoss;
    [HideInInspector]
    public int spawnsCounter = 0;
    bool isBossSpawned = false;
    public bool bossDead = false;
    private DBManager dataBase;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        GameManager.Instance.lc = this;

        dataBase = DBManager.Instance;
        healthBar.maxValue = player.maxHealth;
        musicVolume = music.volume;
        gunFireVolume = gunFire.volume;

        if(GameManager.Instance.playerHealth == 0)
        {
            GameManager.Instance.playerHealth = player.maxHealth;
            player.health = GameManager.Instance.playerHealth;
        }
        else
        {
            player.health = GameManager.Instance.playerHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MousePointerMove();
        UpdateUI();
        CheckPlayersHealth();
        PauseGame();


        music.volume = musicVolume * GameManager.Instance.volume;
        gunFire.volume = gunFireVolume * GameManager.Instance.volume;
    }

    private void FixedUpdate()
    {
        if (!boss)
        {
            SpawnEnemy();
        }
        else
        {
            BossBattle();
        }

        SpawnPickUps();
    }

    public void EndLevel()
    {
        GameManager.Instance.playerHealth = player.health;
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        GameManager.Instance.bossesDefeated++;

        yield return new WaitForSeconds(3);

        EventManager.Instance.PostNotification(EVENT_TYPE.BossKilled, this);
    }

    void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void UpdateUI()
    {
        healthBar.value = player.health;

        
        if (isBossSpawned)
        {
            bossHealthBar.value = bc.health;
        }

        if (player.fireRatePowerUp)
        {
            fireRateBar.gameObject.SetActive(true);
        }
        else
        {
            fireRateBar.gameObject.SetActive(false);
        }

        if (player.fireRatePowerUp)
        {
            fireRateBar.gameObject.SetActive(true);
            fireRateBar.fillAmount = Mathf.InverseLerp(0f, player.powerUpTime, player.fireRateCounter);
        }
        else
        {
            fireRateBar.gameObject.SetActive(false);
        }

        if (player.shieldPowerUp)
        {
            shieldBar.gameObject.SetActive(true);
            shieldBar.fillAmount = Mathf.InverseLerp(0f, player.powerUpTime, player.shieldCounter);
        }
        else
        {
            shieldBar.gameObject.SetActive(false);
        }
        
        scoreText.text = "Score: " + GameManager.Instance.score;
    }

    void CheckPlayersHealth()
    {
        if (player.health <= 0)
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.Death, this);
        }
    }

    public void DestroyGameObject(GameObject objectToDestroy)
    {
        EventManager.Instance.PostNotification(EVENT_TYPE.KillEnemy, this, objectToDestroy);
    }

    void SpawnPickUps()
    {
        if (playArea.platformsPassed > 3)
        {
            if (pickUpCounter == pickUpTime)
            {
                if (Random.Range(0f, 1f) < pickUpChance)
                {
                    Instantiate(pickUps[Random.Range(0, pickUps.Length)], new Vector3(Random.Range(-10, 11), 0.5f, 32f), Quaternion.identity);
                }

                pickUpCounter = 0;
            }
            else
            {
                pickUpCounter++;
            }
        }
    }

    public void SpawnPlatform(GameObject oldPlatform)
    {
        if (!isBossSpawned)
        {
            lastPlatform = Instantiate(platformGameobjects[Random.Range(0, platformGameobjects.Length)], new Vector3(0f, -0.5f, lastPlatform.transform.position.z + 20), Quaternion.identity);
        }
        else
        {
            lastPlatform = Instantiate(platformGameobjects[0], new Vector3(0f, -0.5f, lastPlatform.transform.position.z + 20), Quaternion.identity);
        }
        Destroy(oldPlatform);
    }

    public void Shoot(GameObject shootStartPoint, Vector3 shootingToPoint, char team, int bulletSpeed)
    {
        GameObject bullet = Instantiate(bulletGameobject, shootStartPoint.transform.position, Quaternion.identity);

        if (team == 'E')
        {
            bullet.tag = "EBullet";
            bullet.GetComponent<MeshRenderer>().material = bulletMaterial;
        }

        bullet.transform.LookAt(new Vector3(shootingToPoint.x, bullet.transform.position.y, shootingToPoint.z));
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed);
    }

    void MousePointerMove()
    {
        Vector2 curserPos = Camera.main.WorldToScreenPoint(mousePoint.position);
        mousePointer.transform.position = curserPos;
    }

    void SpawnEnemy()
    {
        if (playArea.platformsPassed > 3)
        {
            if (enemySpawnCounter == enemySpawnTimer)
            {
                spawnsCounter++;
                int spawnZ = 32;
                for (int i = 0; i < Random.Range(minEnemies, maxEnemies); i++)
                {
                    if (Random.Range(0f, 1f) < enemy2Chance)
                    {
                        Instantiate(enemyGameobjects[1], new Vector3(Random.Range(-10, 11), 0f, spawnZ), Quaternion.Euler(0f, 180f, 0f));
                    }
                    else
                    {
                        Instantiate(enemyGameobjects[0], new Vector3(Random.Range(-10, 11), 0f, spawnZ), Quaternion.Euler(0f, 180f, 0f));
                    }
                    spawnZ += 2;
                }

                if (spawnsCounter % 10 == 0 && minEnemies < 5)
                {
                    minEnemies++;
                }

                if (spawnsCounter % 10 == 0 && maxEnemies < 7)
                {
                    maxEnemies++;
                }

                enemySpawnCounter = 0;
            }
            else
            {
                enemySpawnCounter++;
            }
        }
        //For when boss is implemented
        if (spawnsCounter == spawnsUntilBoss)
        {
            boss = true;
            enemySpawnCounter = 0;
        }
    }

    void BossBattle()
    {
        if (enemySpawnCounter == enemySpawnTimer && !isBossSpawned)
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.SpawnBoss, this);
            isBossSpawned = true;
        }
        else if(!isBossSpawned)
        {
            enemySpawnCounter++;
        }       
    }
}
