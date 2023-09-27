using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    [Header("Unit Settings")]
    public int health = 2;

    [Header("Gun Settings")]
    public int bulletSpeed;
    public int fireSpeed;
    private int fireCounter;

    [Header("GameObjects")]
    public GameObject body;
    public GameObject rotateBody;
    public GameObject bulletSpawn;
    public GameObject top;

    [Header("Material Objects")]
    public Material normalMaterial;
    public Material damageMaterial;

    [Header("Movement settings")]
    public Transform runSpot;
    public float runXMax;
    public float runXMin;

    private GameObject player;
    private LevelController lc;

    private int damageCounter = 0;
    private int damageLast = 5;
    private bool damaged = false;

    private void Awake()
    {
        fireCounter = Random.Range(0, fireSpeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        normalMaterial = top.GetComponent<MeshRenderer>().material;
        player = GameObject.FindGameObjectWithTag("Player");
        lc = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();

    }

    private void FixedUpdate()
    {
        MoveWithPlatforms();
        if (player.transform.position.z < this.transform.position.z)
        {
            LookAtPlayer();
            Shoot(player.transform.position);
        }

        if (damaged)
        {
            if (damageCounter == damageLast)
            {
                top.GetComponent<MeshRenderer>().material = normalMaterial;
                damageCounter = 0;
                damaged = false;
            }
            else
            {
                damageCounter++;
            }
        }
    }
    
    void CheckHealth()
    {
        if(health <= 0)
        {
            lc.DestroyGameObject(this.gameObject);
        }
    }

    void MoveWithPlatforms()
    {
        transform.position += new Vector3(0f, 0f, -lc.gameSpeed);
    }

    void LookAtPlayer()
    {
        top.transform.LookAt(new Vector3(player.transform.position.x, top.transform.position.y + 90, player.transform.position.z));
    }

    void Shoot(Vector3 target)
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

    public void LoseHealth(int damage)
    {
        health -= damage;
        top.GetComponent<MeshRenderer>().material = damageMaterial;
        damaged = true;
        CheckHealth();
    }

    public void FindNewRunSpot()
    {
        runSpot.position = new Vector3(Random.Range(runXMin, runXMax), 0f, this.transform.position.z);
    }

    void Run()
    {
        if(body.transform.position == runSpot.position)
        {
            FindNewRunSpot();
        }

        body.transform.position = Vector3.MoveTowards(body.transform.position, runSpot.position, 2f * Time.deltaTime);
        if(runSpot.position.x > body.transform.position.x)
        {
            rotateBody.transform.rotation = Quaternion.Euler(-90f, 180f, -40f);
        }
        else if(runSpot.position.x < body.transform.position.x)
        {
            rotateBody.transform.rotation = Quaternion.Euler(-90f, 180f, 40f);
        }
    }
}
