using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public int score = 0;
    public int pickUpsCollected = 0;
    public int bossesDefeated = 0;
    public float volume = 1;
    public int playerHealth = 0;

    public bool finishedFirstLoop = false;

    public LevelController lc;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.KillEnemy, KillEnemy);
        EventManager.Instance.AddListener(EVENT_TYPE.SpawnBoss, SpawnBoss);
        EventManager.Instance.AddListener(EVENT_TYPE.BossKilled, LoadNextLevel);
        EventManager.Instance.AddListener(EVENT_TYPE.Death, Death);
        EventManager.Instance.AddListener(EVENT_TYPE.ShieldPickUp, TurnOnShield);
        EventManager.Instance.AddListener(EVENT_TYPE.SpeedPickUp, IncreaseFireRate);
        EventManager.Instance.AddListener(EVENT_TYPE.HealthPickUp, Heal);
        EventManager.Instance.AddListener(EVENT_TYPE.UpdateDatabase, SendDatabaseUpdate);
    }

    void KillEnemy(EVENT_TYPE Event_Type, Component Sender, object Param)
    {
        GameObject objectToDestroy = (GameObject)Param;

        if (objectToDestroy.tag == "Enemy")
        {
            GameManager.Instance.score++;
        }

        Destroy(objectToDestroy);
    }

    void SpawnBoss(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        GameObject bossTemp = Instantiate(lc.bossGameobject, new Vector3(0f, 0f, 40f), Quaternion.identity);
        lc.bc = bossTemp.GetComponent<BossController>();
        lc.bossHealthBar.gameObject.SetActive(true);
        lc.bossHealthBar.maxValue = lc.bc.maxHealth;
    }

    void LoadNextLevel(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        if (!finishedFirstLoop)
        {
            switch (SceneManager.GetActiveScene().name)
            {
                default:
                    Debug.Log("Error loading new scene");
                    break;
                case "Level1":
                    SceneManager.LoadScene("Level2");
                    break;
                case "Level2":
                    SceneManager.LoadScene("Level3");
                    GameManager.Instance.finishedFirstLoop = true;
                    break;
            }
        }
        else
        {
            SceneManager.LoadScene(Random.Range(1, 4));
        }
    }

    void Death(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {

        EventManager.Instance.PostNotification(EVENT_TYPE.UpdateDatabase, this);
        SceneManager.LoadScene("GameOver");
    }

    void SendDatabaseUpdate(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        if (DBManager.Instance.logIn)
        {
            DBManager.Instance.currentUser.pickUpsCollected += pickUpsCollected;
            DBManager.Instance.currentUser.bossesDefeated += bossesDefeated;
            if (DBManager.Instance.currentUser.bestScore < score)
            {
                DBManager.Instance.currentUser.bestScore = score;
            }
            DBManager.Instance.UpdatePlayerData();
        }
    }

    void TurnOnShield(EVENT_TYPE Event_Type, Component Sender, object Param)
    {
        GameObject Player = (GameObject)Param;
        Player.GetComponentInParent<PlayerController>().ShieldOn();
    }

    void IncreaseFireRate(EVENT_TYPE Event_Type, Component Sender, object Param)
    {
        GameObject Player = (GameObject)Param;
        Player.GetComponentInParent<PlayerController>().IncreaseFireSpeed();
    }

    void Heal(EVENT_TYPE Event_Type, Component Sender, object Param)
    {
        GameObject Player = (GameObject)Param;
        if (Player.GetComponentInParent<PlayerController>().health < Player.GetComponentInParent<PlayerController>().maxHealth - 1)
        {
            Player.GetComponentInParent<PlayerController>().health += 2;
        }
        else
        {
            Player.GetComponentInParent<PlayerController>().health = Player.GetComponentInParent<PlayerController>().maxHealth;
        }
    }
}
