using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdminCommands : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.F1))
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.BossKilled, this);
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.F2))
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>().spawnsCounter = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>().spawnsUntilBoss;
        }
    }
}
