using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [Header("Unit Settings")]
    public int health = 3;

    [Header("Gun Settings")]
    public int bulletSpeed;
    public int fireSpeed;
    private int fireCounter;

    [Header("GameObjects")]
    public GameObject body;
    public GameObject bulletSpawn;

    [Header("Material Objects")]
    private Material[] normalMaterial = new Material[3];
    public Material[] damageMaterial;

    private LevelController lc;

    private int damageCounter = 0;
    private int damageLast = 5;
    private bool damaged = false;
    private GameObject player;

    // Start is called before the first frame update

    private void Awake()
    {
        fireCounter = Random.Range(0, fireSpeed);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lc = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
        normalMaterial = body.GetComponent<MeshRenderer>().materials;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        MoveWithPlatforms();
        Shoot(new Vector3(transform.position.x, transform.position.y, transform.position.x - 500));

        if (damaged)
        {
            if (damageCounter == damageLast)
            {
                body.GetComponent<MeshRenderer>().materials = normalMaterial;
                damageCounter = 0;
                damaged = false;
            }
            else
            {
                damageCounter++;
            }
        }
    }

    void MoveWithPlatforms()
    {
        transform.position += new Vector3(0f, 0f, -lc.gameSpeed);
    }

    void Shoot(Vector3 target)
    {
        if (player.transform.position.z < this.transform.position.z)
        {
            if (fireCounter == fireSpeed)
            {
                lc.Shoot(bulletSpawn, target, 'E', bulletSpeed);
                fireCounter = 0;
            }
            else
            {
                fireCounter++;
            }
        }
    }

    public void LoseHealth(int damage)
    {
        health -= damage;
        body.GetComponent<MeshRenderer>().materials = damageMaterial;
        damaged = true;
        CheckHealth();
    }

    void CheckHealth()
    {
        if (health <= 0)
        {
            lc.DestroyGameObject(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            LoseHealth(1);
            Destroy(other.gameObject);
        }
        else if (other.tag == "Player")
        {
            LoseHealth(health);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            gameObject.transform.position = new Vector3(Random.Range(-10, 11), gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}
