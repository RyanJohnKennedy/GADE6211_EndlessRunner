using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : MonoBehaviour
{
    public GameObject laser;
    bool firing = false;
    Transform player;
    public int bigLaserDelay;
    public float laserAimDelay;
    public float laserOnTime;
    bool fired = false;
    public GameObject platform;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        LookAtPlayer();
    }
    void LookAtPlayer()
    {
        if (!firing && player.position.z < this.transform.position.z)
            laser.transform.LookAt(player.position);

        if(platform.transform.position.x < 20 && !fired)
        {
            StartCoroutine(MainLazer());
            fired = true;
        }
    }

    IEnumerator MainLazer()
    {
        yield return new WaitForSeconds(bigLaserDelay);

        StartBigLazerAim();
        StartCoroutine(StopFollowing());

        yield return new WaitForSeconds(laserAimDelay);

        FireBigLasers();

        yield return new WaitForSeconds(laserOnTime);

        TurnOffBigLasers();
    }

    void StartBigLazerAim()
    {
        laser.GetComponent<Laser>().laserTarget.SetActive(true);
    }


    void FireBigLasers()
    {
        laser.GetComponent<Laser>().laserTarget.SetActive(false);
        laser.GetComponent<Laser>().laser.SetActive(true);
    }
    IEnumerator StopFollowing()
    {
        yield return new WaitForSeconds(laserAimDelay / 2);

        firing = true;
    }

    void TurnOffBigLasers()
    {
        laser.GetComponent<Laser>().laser.SetActive(false);
        firing = false;

        StartCoroutine(MainLazer());
    }
}
