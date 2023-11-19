using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    SPEED = 0,
    TIME = 1,
    SCORE = 2
}


public class Pickup : Component
{
    public PickupType mPickupType;
    public GameObject mAssignedPlayer;

    public override void OnEnable()
    {
        base.OnEnable();
        CallForAction(7f);
    }
    public override void CallForAction(float time)
    {
        base.CallForAction(time);
    }

    public void PickupPower(GameObject player)
    {
        if (!IsAvailable && player == mAssignedPlayer)
        {
            switch (mPickupType) {
                case PickupType.SPEED:
                    EventHandler.ExecuteEvent(player,"AddSpeed");
                    break;
                case PickupType.TIME:
                    EventHandler.ExecuteEvent(player,"AddTime");
                    break;
                default:
                    EventHandler.ExecuteEvent("UpdateScore",player,50);
                    break;

            }
        }
        StartCoroutine(TimeOff());
    }

    //To avoid the initial logic check to be true
    private IEnumerator TimeOff()
    {
        yield return new WaitForSeconds(0.5f);
        if (IsAvailable)
            Destroy(gameObject);
    }
}
