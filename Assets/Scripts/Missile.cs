using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject target;
    public int damage;
    private AirStrikeController asc;

    // Start is called before the first frame update
    void Start()
    {
        asc = GameObject.FindGameObjectWithTag("AirStrikeController").GetComponent<AirStrikeController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MissileTarget")
        {
            asc.SpawnExplosion(other.transform);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if(other.tag == "Player")
        {
            other.GetComponent<PlayerRotation>().Player.health -= damage;
            asc.SpawnExplosion(other.transform);
            Destroy(target);
            Destroy(gameObject);
        }
    }
}
