using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    SPEED,
    TIME,
    SCORE
}


public class Pickup : Component
{
    public PickupType mPickupType;

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
        if (!IsAvailable)
        {
            switch (mPickupType) {
                case PickupType.SPEED:
                    EventHandler.ExecuteEvent("AddSpeed");
                    break;
                case PickupType.TIME:
                    EventHandler.ExecuteEvent("AddTime");
                    break;
                default:
                    EventHandler.ExecuteEvent("AddScore");
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
