using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBoss : MonoBehaviour
{
    public Boss2 boss;
    int health;
    public Transform rocketLauncher;
    public GameObject rocket;
    public int rocketSpeed;
    AirStrikeController asc;
    private LevelController lc;

    Transform player;

    private bool moveLeft = false;
    int moveSpeed = 3;

    // Start is called before the first frame update
    void Start()
    {
        health = boss.bc.health / 2;
        asc = GameObject.FindGameObjectWithTag("AirStrikeController").GetComponent<AirStrikeController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        lc = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
        MoveAround();
    }

    public void BossBattle()
    {
        StartCoroutine(LaunchRocket());
    }

    void MoveAround()
    {
        if (moveLeft)
        {
            this.transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f));
        }
        else
        {
            this.transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0f, 0f));
        }

        if (this.transform.position.x < -lc.player.playAreaX)
        {
            moveLeft = false;
        }
        else if (this.transform.position.x > lc.player.playAreaX)
        {
            moveLeft = true;
        }
    }

    IEnumerator LaunchRocket()
    {
        GameObject _rocket = Instantiate(rocket, rocketLauncher.position, Quaternion.identity);
        _rocket.GetComponent<Rigidbody>().AddForce(Vector3.up * rocketSpeed);

        yield return new WaitForSeconds(1);

        asc.SpawnRocket(player.position);

        if (boss.rocketSpawn > 1)
        {
            for (int i = 1; i < boss.rocketSpawn; i++)
            {
                asc.SpawnRocket(new Vector3(Random.Range(-lc.player.playAreaX, lc.player.playAreaX), 0.0001f, Random.Range(-lc.player.playAreaZ, lc.player.playAreaZ)));
            }
        }

        yield return new WaitForSeconds(boss.rocketFireDelay);

        StartCoroutine(LaunchRocket());
    }

    void CheckHealth()
    {
        if(health <= 0)
        {
            boss.Rage();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Destroy(other.gameObject);
            if (boss.bossBattle)
            {
                health--;
                boss.bc.health--;
                boss.bc.FlashRed(this.gameObject);
            }
        }

        if (other.tag == "Boss")
        {
            if (other.transform.position.x > this.transform.position.x)
            {
                moveLeft = true;
            }
            else if (other.transform.position.x < this.transform.position.x)
            {
                moveLeft = false;
            }

        }
    }
}
