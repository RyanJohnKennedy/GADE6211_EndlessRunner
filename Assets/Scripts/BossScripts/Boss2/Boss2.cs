using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    public RocketBoss rb;
    public GunBoss gb;

    public BossController bc;

    public float rocketFireDelay;
    public float rocketFireDelayRage = 0.2f;
    public int rocketSpawn = 1;

    public float shootingRoundDelay;
    public float shootingBulletDelay;
    public int shootingRounds;

    public float shootingRoundDelayRage = 0.1f;
    public float shootingBulletDelayRage = 0.2f;

    public bool bossBattle = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Rage()
    {
        rocketFireDelay = rocketFireDelayRage;
        shootingRoundDelay = shootingRoundDelayRage;
        shootingBulletDelay = shootingBulletDelayRage;
        rocketSpawn = 5;
    }

    private void Update()
    {
        if(bc.bossBattle && !bossBattle)
        {
            BossBattle();
            bossBattle = true;
        }
    }

    public void BossBattle()
    {
        rb.BossBattle();
        gb.BossBattle();
    }
}
