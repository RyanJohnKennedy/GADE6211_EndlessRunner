using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    public int platformsPassed = 0;

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Platform")
        {
            Destroy(other.gameObject);
        }
        else
        {
            platformsPassed++;
        }
    }
}
