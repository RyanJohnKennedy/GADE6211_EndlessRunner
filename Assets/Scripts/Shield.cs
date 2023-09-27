using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public int shieldHealth;
    public PlayerController pc;
    LevelController lc;

    private void Start()
    {
        lc = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
    }

    public void ResetHealth(int shieldMaxHealth)
    {
        shieldHealth = shieldMaxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EBullet")
        {
            lc.DestroyGameObject(other.gameObject);
            shieldHealth--;
            CheckHealth();
        }
    }

    void CheckHealth()
    {
        if (shieldHealth == 0)
        {
            this.gameObject.SetActive(false);
            pc.shieldCounter = 0;
        }
    }
}
