using UnityEngine;

public class Inputmanager
{
    public static void UpdateInput(GameObject player)
    {
        if (Input.GetKeyDown(KeyCode.W))
            EventHandler.ExecuteEvent<Vector3>(player,"PlayerMove", Vector3.up);
        else if(Input.GetKeyDown(KeyCode.A))
            EventHandler.ExecuteEvent<Vector3>(player, "PlayerMove", Vector3.left);
        else if (Input.GetKeyDown(KeyCode.S))
            EventHandler.ExecuteEvent<Vector3>(player, "PlayerMove", Vector3.down);
        else if (Input.GetKeyDown(KeyCode.D))
            EventHandler.ExecuteEvent<Vector3>(player, "PlayerMove", Vector3.right);
        else if (Input.GetKeyDown(KeyCode.E))
            EventHandler.ExecuteEvent(player, "Interact");
    }
}
