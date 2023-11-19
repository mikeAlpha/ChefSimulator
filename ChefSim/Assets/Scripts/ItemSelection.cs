using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct Item
{
    public string Name;
    public bool IsCut;
}

public class ItemSelection : Component
{
    public Item mItem;

    public override void CallForAction(float time)
    {
        base.CallForAction(time);
    }

    public void PickItem(GameObject player)
    {
        if (IsAvailable && player.GetComponent<PlayerController>().mInventory.mItems.Count < 3)
        {
            EventHandler.ExecuteEvent<Item>(player,"AddPickedItem", mItem);
            CallForAction(5f);
        }
        else
        {
            Debug.Log("Not available");
        }
    }
}
