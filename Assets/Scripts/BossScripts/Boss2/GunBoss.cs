using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBoss : MonoBehaviour
{
    public Boss2 boss;
    int health;
    public GameObject gun;
    public Transform shootingTarget;
    public GameObject bullet;
    private LevelController lc;
    private int bulletCount = 0;

    private bool moveLeft = true;
    private int moveSpeed = 3;

    // Start is called before the first frame update
    void Start()
    {
        health = boss.bc.health / 2;
        lc = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
        MoveAround();
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

        if(this.transform.position.x < -lc.player.playAreaX)
        {
            moveLeft = false;
        }
        else if(this.transform.position.x > lc.player.playAreaX)
        {
            moveLeft = true;
        }
    }

    public void BossBattle()
    {
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(boss.shootingRoundDelay);

        StartCoroutine(ShootBullet());
    }

    IEnumerator ShootBullet()
    {
        shootingTarget.position = new Vector3(Random.Range(-lc.player.playAreaX, lc.player.playAreaX), shootingTarget.position.y, shootingTarget.position.z);
        lc.Shoot(gun, shootingTarget.position, 'E', boss.bc.bulletSpeed);
        bulletCount++;

        yield return new WaitForSeconds(boss.shootingBulletDelay);

        if(bulletCount < boss.shootingRounds)
        {
            StartCoroutine(ShootBullet());
        }
        else
        {
            bulletCount = 0;
            StartCoroutine(Shoot());
        }
    }

    void CheckHealth()
    {
        if (health <= 0)
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
