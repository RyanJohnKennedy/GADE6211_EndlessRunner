using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : PickUpController
{
    // Start is called before the first frame update
    public override void Start()
    {
        Type = PickUpType.Health;
    }

    public override void Effect(GameObject Player)
    {
        EventManager.Instance.PostNotification(EVENT_TYPE.HealthPickUp, this, Player);
    }
}
