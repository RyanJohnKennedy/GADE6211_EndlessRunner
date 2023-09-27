using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : PickUpController
{
    public override void Start()
    {
        Type = PickUpType.Shield;
    }

    public override void Effect(GameObject Player)
    {
        EventManager.Instance.PostNotification(EVENT_TYPE.ShieldPickUp, this, Player);
    }
}
