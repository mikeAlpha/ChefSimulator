using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private List<PlayerController> players;
    public string[] items;
    public GameObject[] playerUIs;
    private Dictionary<GameObject, GameObject> mUi_player_mapping;

    public GameObject mResultPanel;
    public Tilemap MainTileMap;
    private List<Vector3> walkableTiles = new List<Vector3>();

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        players = new List<PlayerController>();
        mUi_player_mapping = new Dictionary<GameObject, GameObject>();
        StoreAllWalkableTiles();
        StartCoroutine(ConfigureCustomers());
        mResultPanel.GetComponentInChildren<Button>().onClick.AddListener(RestartGame);
    }

    private void OnEnable()
    {
        EventHandler.RegisterEvent<PlayerController>("RegisterPlayer", RegisterPlayer);
        EventHandler.RegisterEvent<GameObject,int>("UpdateScore", UpdateScore);
        EventHandler.RegisterEvent<GameObject, int>("UpdateTimer", UpdateTimer);
        EventHandler.RegisterEvent("CheckPlayerTime", CheckBothPlayerEnd);
        EventHandler.RegisterEvent<GameObject>("CreatePickup", CreatePickup);
    }


    private void OnDisable()
    {
        EventHandler.UnregisterEvent<PlayerController>("RegisterPlayer", RegisterPlayer);
        EventHandler.UnregisterEvent<GameObject,int>("UpdateScore", UpdateScore);
        EventHandler.UnregisterEvent<GameObject, int>("UpdateTimer", UpdateTimer);
        EventHandler.UnregisterEvent("CheckPlayerTime", CheckBothPlayerEnd);
        EventHandler.UnregisterEvent<GameObject>("CreatePickup", CreatePickup);
    }

    private void Update()
    {
        if (players.Count < 3)
        {
            Inputmanager.UpdateInput(players[0].gameObject);
            Inputmanager.UpdateInput(players[1].gameObject);
        }
    }

    private IEnumerator ConfigureCustomers()
    {
        var customers = FindObjectsOfType<Customer>();
        foreach (Customer c in customers)
        {
            c.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void CreatePickup(GameObject player)
    {
        //if(player.GetComponent<PlayerController>().Id == 0)

        GameObject obj = Resources.Load<GameObject>("Pickup") as GameObject;
        var position = GetRandomPositions();
        var pick_itm = Instantiate(obj,position,Quaternion.identity);

        if (player.GetComponent<PlayerController>().Id == 0)
            pick_itm.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(41, 135, 206);
        else
            pick_itm.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255, 41, 206);

        var pick_itm_comp = pick_itm.GetComponent<Pickup>();
        pick_itm_comp.mAssignedPlayer = player;

        System.Random random = new System.Random();
        Type type = typeof(PickupType);
        Array values = type.GetEnumValues();
        int index = random.Next(values.Length);
        pick_itm_comp.mPickupType = (PickupType)values.GetValue(index);
        pick_itm_comp.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(pick_itm_comp.mPickupType.ToString()) as Sprite;
    }

    private void StoreAllWalkableTiles()
    {
        BoundsInt bounds = MainTileMap.cellBounds;
        TileBase[] allTiles = MainTileMap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile == null)
                {
                   walkableTiles.Add(MainTileMap.CellToWorld(new Vector3Int(x, y, 0)));
                }
            }
        }
    }

    private Vector3 GetRandomPositions()
    {
        return walkableTiles[UnityEngine.Random.Range(0, walkableTiles.Count)];
    }

    public List<PlayerController> GetPlayers()
    {
        return players;
    }

    private void RegisterPlayer(PlayerController player)
    {
        if(players == null)
            players = new List<PlayerController>();
        players.Add(player);

        if (players.Count == 2)
        {

            mUi_player_mapping.Add(players[0].gameObject, playerUIs[0]);
            mUi_player_mapping.Add(players[1].gameObject, playerUIs[1]);
        }
    }

    private void UpdateTimer(GameObject player, int val)
    {
        if (mUi_player_mapping.ContainsKey(player))
        {
            GameObject ui = mUi_player_mapping[player];
            ui.GetComponent<PlayerUIObject>().mTime.text = "Time:"+val.ToString();
        }
    }

    private void CheckBothPlayerEnd()
    {
        int count = 0;
        foreach(PlayerController ply in players)
        {
            if (ply.IsTimeEnded)
                count++;
        }

        if (count == 2)
        {
            Debug.Log("Game Ended");
            UpdateWinner();
        }
    }

    private void UpdateWinner()
    {
        var player_1_score = players[0].GetComponent<PlayerController>().GetScore();
        var player_2_score = players[1].GetComponent<PlayerController>().GetScore();

        mResultPanel.SetActive(true);
        string msg = string.Empty;
        if (player_1_score > player_2_score)
            msg = "Player 1 wins";
        else if (player_1_score < player_2_score)
            msg = "Player 1 wins";
        else
            msg = "It's a tie";

        mResultPanel.transform.GetChild(1).GetComponent<Text>().text = msg;
    }

    private void UpdateScore(GameObject player, int val)
    {
        if (mUi_player_mapping.ContainsKey(player))
        {
            GameObject ui = mUi_player_mapping[player];
            ui.GetComponent<PlayerUIObject>().mScore.text = "Score:"+val.ToString();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }


    public string[] GenerateItems()
    {
        return UniqueElements.SelectRandomUniqueValues(items, 3);
    }
}
