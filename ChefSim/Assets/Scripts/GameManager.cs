using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> players;
    public string[] items;

    public GameObject[] playerUIs;
    private Dictionary<GameObject, GameObject> mUi_player_mapping;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        players = new List<GameObject>();
        mUi_player_mapping = new Dictionary<GameObject, GameObject>();
    }

    private void OnEnable()
    {
        EventHandler.RegisterEvent<GameObject>("RegisterPlayer", RegisterPlayer);
        EventHandler.RegisterEvent<GameObject,int>("UpdateScore", UpdateScore);
        EventHandler.RegisterEvent<GameObject, int>("UpdateTimer", UpdateTimer);
        EventHandler.RegisterEvent("CheckPlayerTime", CheckBothPlayerEnd);
    }


    private void OnDisable()
    {
        EventHandler.UnregisterEvent<GameObject>("RegisterPlayer", RegisterPlayer);
        EventHandler.UnregisterEvent<GameObject,int>("UpdateScore", UpdateScore);
        EventHandler.UnregisterEvent<GameObject, int>("UpdateTimer", UpdateTimer);
        EventHandler.UnregisterEvent("CheckPlayerTime", CheckBothPlayerEnd);
    }

    private void Update()
    {
        if (players.Count < 3)
        {
            Inputmanager.UpdateInput(players[0]);
            Inputmanager.UpdateInput(players[1]);
        }
    }


    private void RegisterPlayer(GameObject player)
    {
        if(players == null)
            players = new List<GameObject>();
        players.Add(player);

        if (players.Count == 2)
        {
            players.Sort();

            mUi_player_mapping.Add(players[0], playerUIs[0]);
            mUi_player_mapping.Add(players[1], playerUIs[1]);
        }
    }

    private void UpdateTimer(GameObject player, int val)
    {
        if (mUi_player_mapping.ContainsKey(player))
        {
            GameObject ui = mUi_player_mapping[player];
            ui.GetComponent<Text>().text = val.ToString();
        }
    }

    private void CheckBothPlayerEnd()
    {
        int count = 0;
        foreach(GameObject ply in players)
        {
            if (ply.GetComponent<PlayerController>().IsTimeEnded)
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

        if (player_1_score > player_2_score)
            Debug.Log("Player 1 wins");
        else if (player_1_score < player_2_score)
            Debug.Log("Player 2 wins");
        else
            Debug.Log("Its a tie");
    }

    private void UpdateScore(GameObject player, int val)
    {
        if (mUi_player_mapping.ContainsKey(player))
        {
            GameObject ui = mUi_player_mapping[player];
            ui.GetComponent<Text>().text = val.ToString();
        }
    }


    public string[] GenerateItems()
    {
        return UniqueElements.SelectRandomUniqueValues(items, 3);
    }
}
