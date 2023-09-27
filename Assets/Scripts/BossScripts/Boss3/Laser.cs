using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject laser;
    public GameObject laserTarget;
    public bool mainLaser;

    public int[] moveSpeed;
    int turnAngle;
    public Boss3 boss;

    // Start is called before the first frame update
    void Start()
    {
        turnAngle = Random.Range(-40, 40);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        if (!mainLaser && laser.activeSelf == false && laserTarget.activeSelf == false)
        {
            transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(0f, turnAngle, 0f), boss.laserTurnspeed * Time.deltaTime);
        }
    }

    public void RandomSpeed()
    {
        turnAngle = Random.Range(-40, 40);
    }
}
