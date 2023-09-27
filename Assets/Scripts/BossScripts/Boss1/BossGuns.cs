using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGuns : MonoBehaviour
{
    public Transform[] targets;
    public BossController bc;

    LevelController lc;

    public bool shooting;

    int bulletSpeed;

    int target = 0;
    public int fireSpeed;
    int fireCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        lc = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
        shooting = false;
        bulletSpeed = bc.bulletSpeed;
    }

    private void FixedUpdate()
    {
        if (shooting)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (fireCounter == fireSpeed)
        {
            lc.Shoot(this.gameObject, targets[target].position, 'E', bulletSpeed);

            if(target >= targets.Length - 1)
            {
                if (fireSpeed > 5)
                {
                    fireSpeed--;
                }
                shooting = false;
                target = 0;
            }
            else
            {
                target++;
            }

            fireCounter = 0;
        }
        else
        {
            fireCounter++;
        }
    }
}
