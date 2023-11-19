using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public struct Inventory
{
    public List<Item> mItems;
}

public class PlayerController : MonoBehaviour, IComparable<PlayerController>
{
    
    private Vector3[] directions;
    BoxCollider2D boxCollider;

    public Inventory mInventory;

    public float timeToMove = 0.02f;

    private float TotalTime = 120f;
    private float elapsedTime = 0f;

    private Tilemap MainTileMap;

    public int Id = 0;

    public bool IsTimeEnded = false;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        elapsedTime = TotalTime;
    }

    private void OnEnable()
    {
        EventHandler.RegisterEvent<GameObject, int>("UpdateScore", AddScore);
        EventHandler.RegisterEvent<Vector3>(gameObject,"PlayerMove", Move);
        EventHandler.RegisterEvent(gameObject, "AddSpeed", AddSpeed);
        EventHandler.RegisterEvent(gameObject, "AddTime", AddTime);
        EventHandler.RegisterEvent(gameObject, "Interact", Interact);
        EventHandler.RegisterEvent<Item>(gameObject, "AddPickedItem", AddInventoryItem);
        EventHandler.RegisterEvent<Item>(gameObject, "RemovePickedItem", RemoveInventoryItem);
    }

    private void OnDisable()
    {
        EventHandler.UnregisterEvent<Vector3>(gameObject, "PlayerMove", Move);
        EventHandler.UnregisterEvent(gameObject, "AddSpeed", AddSpeed);
        EventHandler.UnregisterEvent(gameObject, "AddTime", AddTime);
        EventHandler.UnregisterEvent(gameObject, "Interact", Interact);
        EventHandler.UnregisterEvent<Item>(gameObject, "AddPickedItem", AddInventoryItem);
        EventHandler.UnregisterEvent<Item>(gameObject, "RemovePickedItem", RemoveInventoryItem);
    }

    private void Start()
    {
        MainTileMap = GameManager.instance.MainTileMap;
        EventHandler.ExecuteEvent<PlayerController>("RegisterPlayer", this);
    }


    int score = 0;
    private void AddScore(GameObject player,int val)
    { 
        if (gameObject != player)
            return;

        score += val; 
    }
    public int GetScore() { return score; }

    private void Move(Vector3 inputVector)
    {
        StartCoroutine(ActualMove(inputVector)); 
    }

    private IEnumerator ActualMove(Vector3 inputVector)
    {
        float elapsedTime = 0f;
        while (elapsedTime < timeToMove)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Vector3 dir = transform.position + inputVector;
        dir.z = 0;

        Vector3Int wall = MainTileMap.WorldToCell(dir);
        if (MainTileMap.GetTile(wall) == null)
        {
            transform.Translate(inputVector);
        }
    }

    void Update()
    {
        DetectCollision();
        UpdateTime();
    }

    private void UpdateTime()
    {
        if (!IsTimeEnded)
        {
            elapsedTime -= Time.deltaTime;
            if (elapsedTime <= 0)
            {
                Debug.Log("Player Time Over");
                EventHandler.ExecuteEvent("CheckPlayerTime");
                IsTimeEnded = true;
            }
            EventHandler.ExecuteEvent<GameObject, int>("UpdateTimer", gameObject, (int)elapsedTime);
        }
    }


    private void Interact()
    {
        if(DetectCollision() != null && DetectCollision().layer == 8)
        {
            var obj = DetectCollision();
            if (obj.GetComponent<ItemSelection>() != null)
            {
                Debug.Log("Picked");
                obj.GetComponent<ItemSelection>().PickItem(gameObject);
            }
            else if (obj.GetComponent<Board>() != null)
            {
                Debug.Log("Cut");
                obj.GetComponent<Board>().CutItem(gameObject);
            }
            else if (obj.GetComponent<Plate>() != null)
            {
                Debug.Log("Store");
                obj.GetComponent<Plate>().StoreItem(gameObject);
            }

            else if (obj.GetComponent<Trashbin>() != null)
            {
                Debug.Log("Trash");
                obj.GetComponent<Trashbin>().TrashItem(gameObject);
            }
            else if (obj.GetComponent<Customer>() != null)
            {
                Debug.Log("Customer");
                obj.GetComponent<Customer>().CheckCombination(gameObject);
            }
            else if (obj.GetComponent<Pickup>() != null)
            {
                Debug.Log("Pickup");
                obj.GetComponent<Pickup>().PickupPower(gameObject);
            }
        }
    }

    private void AddSpeed()
    {
        StartCoroutine(ActualAddSpeed(10f));
    }

    private void AddTime()
    {
        elapsedTime += 20f;
    }

    private IEnumerator ActualAddSpeed(float val)
    {
        timeToMove = 0.1f;
        float elapsedTime = 0f;
        while (elapsedTime < 5f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        timeToMove = 0.02f;
    }


    private void AddInventoryItem(Item item)
    {
        if (mInventory.mItems == null)
            mInventory.mItems = new List<Item>();

        if (mInventory.mItems.Count == 2)
        {
            Debug.Log("Inventory is full");
            return;
        }

        Debug.Log(item.Name + " picked item");
        mInventory.mItems.Add(item);
        var text_mesh = GetComponentInChildren<TextMesh>();
        for (int i = 0; i < mInventory.mItems.Count; i++)
        {
            if (text_mesh.text.Contains(mInventory.mItems[i].Name))
                continue;
            else
            {
                text_mesh.text = text_mesh.text.Replace("\t", string.Empty);
                text_mesh.text += mInventory.mItems[i].Name + "\t";
            }
        }
    }

    private void RemoveInventoryItem(Item item)
    {
        if (mInventory.mItems == null)
            mInventory.mItems = new List<Item>();

        if (mInventory.mItems.Contains(item))
        {
            Debug.Log("Here");
            var text_mesh = GetComponentInChildren<TextMesh>();
            text_mesh.text=text_mesh.text.Replace(item.Name, "");
            text_mesh.text = text_mesh.text.Replace("\t", string.Empty);
            mInventory.mItems.Remove(item);
        }
    }

    private GameObject DetectCollision()
    {
        directions = new Vector3[4];
        directions[0] = transform.up;
        directions[1] = -transform.up;
        directions[2] = transform.right;
        directions[3] = -transform.right;

        for (int i = 0; i < directions.Length; i++)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(boxCollider.bounds.center, directions[i], 1f);
            Debug.DrawRay(boxCollider.bounds.center, directions[i], Color.white);
            if (hitInfo.collider != null)
            {
                Debug.DrawRay(boxCollider.bounds.center, directions[i], Color.red);
                return hitInfo.collider.gameObject;
            }
        }

        return null;
    }

    public int CompareTo(PlayerController other)
    {
        return Id.CompareTo(other.Id);
    }
}
