using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3 : MonoBehaviour
{
    public BossController bc;
    public GameObject SafeArea;
    public GameObject mainLaser;
    public GameObject mainLaserTarget;

    Transform player;

    public List<GameObject> lasers;
    public float laserWaitDelay;
    public float laserOnTime;
    public int laserTurnspeed;

    public float laserAimDelay;

    public int bigLaserDelay;

    bool battleStarted = false;
    bool firing = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();

        if (bc.bossBattle && !battleStarted)
        {
            StartCoroutine(NormalLazers());
            battleStarted = true;
            StartCoroutine(MainLazer());
        }
    }

    void LookAtPlayer()
    {
        if(!firing)
            mainLaser.transform.LookAt(player.position);
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

    IEnumerator StopFollowing()
    {
        yield return new WaitForSeconds(laserAimDelay / 2);

        firing = true;
    }

    IEnumerator NormalLazers()
    {
        yield return new WaitForSeconds(laserWaitDelay);

        StartSmallLazerAim();

        yield return new WaitForSeconds(laserAimDelay);

        FireSmallLasers();

        yield return new WaitForSeconds(laserOnTime);

        TurnOffSmallLasers();
    }

    void StartSmallLazerAim()
    {
        foreach (GameObject go in lasers)
        {
            go.GetComponent<Laser>().laserTarget.SetActive(true);
        }
    }

    void FireSmallLasers()
    {
        foreach (GameObject go in lasers)
        {
            go.GetComponent<Laser>().laserTarget.SetActive(false);
            go.GetComponent<Laser>().laser.SetActive(true);
        }
    }

    void TurnOffSmallLasers()
    {
        foreach (GameObject go in lasers)
        {
            go.GetComponent<Laser>().laser.SetActive(false);
            go.GetComponent<Laser>().RandomSpeed();
        }

        StartCoroutine(NormalLazers());
    }

    void StartBigLazerAim()
    {
        mainLaser.GetComponent<Laser>().laserTarget.SetActive(true);
    }

    void FireBigLasers()
    {
        mainLaser.GetComponent<Laser>().laserTarget.SetActive(false);
        mainLaser.GetComponent<Laser>().laser.SetActive(true);
    }

    void TurnOffBigLasers()
    {
        mainLaser.GetComponent<Laser>().laser.SetActive(false);
        firing = false;

        StartCoroutine(MainLazer());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            if (bc.bossBattle)
            {
                Destroy(other.gameObject);
                bc.health--;
                bc.FlashRed(this.gameObject);
            }
        }
    }
}
