using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    public EnemyUnit Enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Enemy.LoseHealth(1);
            Destroy(other.gameObject);
        }
        else if(other.tag == "Player")
        {
            Enemy.LoseHealth(Enemy.health);
        }
        else if(other.tag == "Obstacle")
        {
            if(other.gameObject.transform.position.x > Enemy.gameObject.transform.position.x)
            {
                Enemy.runXMax = Enemy.transform.position.x;
                Enemy.FindNewRunSpot();
            }
            else if(other.gameObject.transform.position.x < Enemy.gameObject.transform.position.x)
            {
                Enemy.runXMin = Enemy.transform.position.x;
                Enemy.FindNewRunSpot();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if(other.tag == "Obstacle")
        //{
        //    Enemy.gameObject.transform.position = new Vector3(Random.Range(-10, 11), Enemy.gameObject.transform.position.y, Enemy.gameObject.transform.position.z);
        //}
    }
}
