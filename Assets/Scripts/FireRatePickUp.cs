using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRatePickUp : PickUpController
{
    public override void Start()
    {
        Type = PickUpType.FireRate;
    }

    public override void Effect(GameObject Player)
    {
        EventManager.Instance.PostNotification(EVENT_TYPE.SpeedPickUp, this, Player);
    }
}
