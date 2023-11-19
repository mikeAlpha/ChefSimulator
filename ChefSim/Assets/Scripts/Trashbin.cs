using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashbin : Component
{

    public Item mItem;
    private GameObject mItem_obj;

    public override void CallForAction(float time)
    {
        base.CallForAction(time);
    }

    public void TrashItem(GameObject player)
    {
        Debug.Log(player.GetComponent<PlayerController>().mInventory.mItems.Count);

        if (IsAvailable && player.GetComponent<PlayerController>().mInventory.mItems.Count < 3)
        {
            var inv = player.GetComponent<PlayerController>().mInventory;
            mItem = inv.mItems[inv.mItems.Count - 1];
            EventHandler.ExecuteEvent<Item>(player,"RemovePickedItem", mItem);
            CallForAction(0f);
        }
    }
}
