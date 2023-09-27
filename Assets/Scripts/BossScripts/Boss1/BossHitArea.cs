using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitArea : MonoBehaviour
{
    public int health;

    public Boss1 boss;
    private LevelController lc;

    public GameObject arm;
    public BossGuns gun;
    public GameObject spinGun;

    // Start is called before the first frame update
    void Start()
    {
        lc = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            lc.DestroyGameObject(other.gameObject);
            if (boss.bc.bossBattle)
            {
                health--;
                boss.bc.health--;
                boss.bc.FlashRed(arm);
            }
        }
    }

    void CheckHealth()
    {
        if(health <= 0)
        {
            boss.RemoveGun(arm, gun, spinGun);
        }
    }
}
