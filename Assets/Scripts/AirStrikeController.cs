using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStrikeController : MonoBehaviour
{
    LevelController lc;
    bool spawn = false;

    public GameObject target;
    public GameObject missile;
    public int missleSpawnHeight;
    public int damage;
    public GameObject explosion;

    public float spawnTime;

    int spawnCounter = 0;

    int airstrikeAmount = 1;

    public BoxCollider moveArea;

    // Start is called before the first frame update
    void Start()
    {
        lc = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lc.playArea.platformsPassed > 3 && spawn == false)
        {
            StartCoroutine(CallAirstrike());
            spawn = true;
        }
    }

    IEnumerator CallAirstrike()
    {
        yield return new WaitForSeconds(spawnTime);

        SpawnAirstrike();
    }

    void SpawnAirstrike()
    {
        for (int i = 0; i < airstrikeAmount; i++)
        {
            SpawnRocket(new Vector3(Random.Range(-lc.player.playAreaX, lc.player.playAreaX), 0.0001f, Random.Range(-lc.player.playAreaZ, lc.player.playAreaZ)));
        }

        spawnCounter++;

        if (spawnCounter % 10 == 0)
        {
            airstrikeAmount++;
        }

        if (!lc.boss)
        {
            StartCoroutine(CallAirstrike());
        }
    }

    public void SpawnRocket(Vector3 _target)
    {
        GameObject t = Instantiate(target, new Vector3(_target.x, 0.0001f, _target.z), Quaternion.Euler(90f, 0f, 0f));
        GameObject missileTemp = Instantiate(missile, new Vector3(t.transform.position.x, missleSpawnHeight, t.transform.position.z), Quaternion.Euler(180, 0, 0));
        missileTemp.GetComponent<Missile>().target = t;
        missileTemp.GetComponent<Missile>().damage = damage;
    }

    public void SpawnExplosion(Transform location)
    {
        GameObject ex = Instantiate(explosion, location.position, Quaternion.identity);
        ex.GetComponent<AudioSource>().volume = ex.GetComponent<AudioSource>().volume * GameManager.Instance.volume;

        StartCoroutine(RemoveExplosion(ex));
    }

    IEnumerator RemoveExplosion(GameObject ex)
    {
        yield return new WaitForSeconds(1);

        Destroy(ex);
    }
}
