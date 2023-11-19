using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : Component
{
    public Item mItem;
    private GameObject mItem_obj;

    public override void CallForAction(float time)
    {
        base.CallForAction(time);
    }

    public void CutItem(GameObject player)
    {
        Debug.Log(player.GetComponent<PlayerController>().mInventory.mItems.Count);

        if(IsAvailable && !string.IsNullOrEmpty(mItem.Name))
        {
            mItem.IsCut = true;
            EventHandler.ExecuteEvent<Item>(player,"AddPickedItem", mItem);
            mItem.Name = string.Empty;
            Destroy(mItem_obj);
            return;
        }


        if (IsAvailable && player.GetComponent<PlayerController>().mInventory.mItems.Count < 3)
        {
            var inv = player.GetComponent<PlayerController>().mInventory;
            mItem = inv.mItems[inv.mItems.Count-1];

            //if (!mItem.IsCut)
            //    return;


            GameObject itm = Resources.Load("item") as GameObject;
            mItem_obj = Instantiate(itm, transform);
            mItem_obj.transform.localPosition = Vector3.zero;
            mItem_obj.transform.localScale = new Vector3(0.8f, 0.8f, 1f);

            mItem_obj.GetComponentInChildren<TextMesh>().text = mItem.Name;

            EventHandler.ExecuteEvent<Item>(player,"RemovePickedItem", mItem);
            CallForAction(5f);
        }
        else
        {
            Debug.Log("chopping on " + gameObject.name);
        }
    }
}
