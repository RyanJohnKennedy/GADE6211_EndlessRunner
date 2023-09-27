using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private LevelController lc;

    // Start is called before the first frame update
    void Start()
    {
        lc = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(0f, 0f, -lc.gameSpeed);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "PlayArea")
        {
            lc.SpawnPlatform(this.gameObject);
        }
    }
}
