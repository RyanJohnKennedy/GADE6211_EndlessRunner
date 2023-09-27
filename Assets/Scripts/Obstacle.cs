using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject obstacleObject;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(Random.Range(-10, 11), this.transform.position.y, this.transform.position.z);
        obstacleObject.transform.rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet" || other.tag == "EBullet")
        {
            Destroy(other.gameObject);
        }
        else if(other.tag == "Boss" || other.tag == "BossDamageSpot")
        {
            Destroy(this.gameObject);
        }
    }
}
