using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : Component
{
    public Dictionary<string,bool> mItemCombination;
    TextMesh[] text_meshes;
    int t_matched = 0;
    int p1_matched = 0 , p2_matched = 0;
    List<GameObject> players;
    bool IsDone = false;

    public GameObject slider;

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void Start()
    {
        base.Start();
        GenarateCombination();
    }

    public override void Update()
    {
        base.Update();
        if(!IsDone)
        {
            slider.transform.localScale = new Vector3(elapsedTime / TimeForAction, 1, 1);
            slider.transform.localPosition = new Vector3(((elapsedTime / TimeForAction) - 1f)* 0.5f , 0, 0);
        }
    }

    public override void CallForAction(float time)
    {
        base.CallForAction(time);
    }

    public void GenarateCombination()
    {
        text_meshes = GetComponentsInChildren<TextMesh>();
        mItemCombination = new Dictionary<string, bool>();
        var random_items = GameManager.instance.GenerateItems();

        for(int i = 0; i< random_items.Length; i++)
        {
            Item itm = new Item();

            itm.Name = random_items[i];
            mItemCombination.Add(itm.Name, false);
            text_meshes[i].text = itm.Name;
        }

        CallForAction(120f);
    }

    public void CheckCombination(GameObject player)
    {
        if (IsDone)
            return;
        
        players = new List<GameObject>();
        int length = GameManager.instance.GetPlayers().Count;
        var plyCnts = GameManager.instance.GetPlayers();
        for (int i = 0; i < length; i++)
        {
            players.Add(plyCnts[i].gameObject);
        }

        if (!IsAvailable)
        {
            var ply = player.GetComponent<PlayerController>();
            var inv = ply.mInventory;
            var mItems = inv.mItems;

            for(int i = 0; i<mItems.Count; i++)
            {
                Debug.Log(mItems[i].IsCut + " " + mItemCombination.ContainsKey(mItems[i].Name));
                if (mItemCombination.ContainsKey(mItems[i].Name) && mItems[i].IsCut)
                {
                    int idx = Array.IndexOf(mItemCombination.Keys.ToArray(), mItems[i].Name);
                    mItemCombination[mItems[i].Name] = true;
                    text_meshes[idx].color = Color.red;
                    EventHandler.ExecuteEvent<Item>(player, "RemovePickedItem", mItems[i]);
                    t_matched++;

                    if (ply.Id == 0)
                        p1_matched++;
                    else
                        p2_matched++;
                }
            }

            if (t_matched == mItemCombination.Count)
            {
                Debug.Log("Customer is happy..ingredients are combined");
                
                if (p1_matched > p2_matched)
                {
                    EventHandler.ExecuteEvent<GameObject, int>("UpdateScore", players[0], 100);
                    EventHandler.ExecuteEvent<GameObject, int>("UpdateScore", players[1], 50);
                }
                else
                {
                    EventHandler.ExecuteEvent<GameObject, int>("UpdateScore", players[1], 100);
                    EventHandler.ExecuteEvent<GameObject, int>("UpdateScore", players[0], 50);
                }

                if (t_matched == mItemCombination.Count && percentage > 70f)
                {
                    if (p1_matched > p2_matched)
                    {
                        EventHandler.ExecuteEvent<GameObject>("CreatePickup", players[0]);
                    }
                    
                    else
                    {
                        EventHandler.ExecuteEvent<GameObject>("CreatePickup", players[1]);
                    }
                }
            }
        }

        else if(IsAvailable && t_matched < mItemCombination.Count)
        {
            Debug.Log("Customer is angry");
            EventHandler.ExecuteEvent<GameObject, int>("UpdateScore", players[0], -50);
            EventHandler.ExecuteEvent<GameObject, int>("UpdateScore", players[1], -50);
        }

        IsDone = true;
    }
}
