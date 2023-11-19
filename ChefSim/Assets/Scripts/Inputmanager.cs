using UnityEngine;

public class Inputmanager
{
    public static void UpdateInput(GameObject player)
    {
        if (player.GetComponent<PlayerController>().Id == 0)
        {

            if (Input.GetKeyDown(KeyCode.W))
                EventHandler.ExecuteEvent<Vector3>(player, "PlayerMove", Vector3.up);
            else if (Input.GetKeyDown(KeyCode.A))
                EventHandler.ExecuteEvent<Vector3>(player, "PlayerMove", Vector3.left);
            else if (Input.GetKeyDown(KeyCode.S))
                EventHandler.ExecuteEvent<Vector3>(player, "PlayerMove", Vector3.down);
            else if (Input.GetKeyDown(KeyCode.D))
                EventHandler.ExecuteEvent<Vector3>(player, "PlayerMove", Vector3.right);
            else if (Input.GetKeyDown(KeyCode.E))
                EventHandler.ExecuteEvent(player, "Interact");
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                EventHandler.ExecuteEvent<Vector3>(player, "PlayerMove", Vector3.up);
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                EventHandler.ExecuteEvent<Vector3>(player, "PlayerMove", Vector3.left);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                EventHandler.ExecuteEvent<Vector3>(player, "PlayerMove", Vector3.down);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                EventHandler.ExecuteEvent<Vector3>(player, "PlayerMove", Vector3.right);
            else if (Input.GetKeyDown(KeyCode.P))
                EventHandler.ExecuteEvent(player, "Interact");
        }
    }
}
