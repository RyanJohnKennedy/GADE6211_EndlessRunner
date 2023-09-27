using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    public BossController bc;
    int fireSpeedCounter;
    public int fireSpeed;
    int gun = 0;
    public List<BossGuns> guns;
    public GameObject body;
    public List<GameObject> arms;
    public List<GameObject> gunGameobjects;


    // Start is called before the first frame update
    void Start()
    {
        fireSpeedCounter = fireSpeed - 10;
    }

    private void FixedUpdate()
    {
        if (bc.bossBattle)
        {
            RotateGuns();
            BossBattle();
        }

        if (gun >= guns.Count)
        {
            if (fireSpeed > 50)
            {
                fireSpeed -= 5;
            }
            gun = 0;
        }
    }

    public void BossBattle()
    {
        if (fireSpeed == fireSpeedCounter)
        {
            if (guns.Count > 1)
            {
                guns[gun].shooting = true;
            }
            else
            {
                guns[0].shooting = true;
            }

            fireSpeedCounter = 0;

            gun++;
        }
        else
        {
            fireSpeedCounter++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Destroy(other.gameObject);
            if (guns.Count <= 1)
            {
                bc.health--;
                bc.FlashRed(body);
            }
        }
    }

    public void RemoveGun(GameObject arm, BossGuns gunToRemove, GameObject spinGun)
    {
        guns.Remove(gunToRemove);
        arm.SetActive(false);
        gunGameobjects.Remove(spinGun);
    }

    void RotateGuns()
    {
        foreach (GameObject go in gunGameobjects)
        {
            go.transform.Rotate(0f, 10f, 0f);
        }
    }
}
