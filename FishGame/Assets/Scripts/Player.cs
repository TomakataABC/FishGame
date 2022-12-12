using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public bool IsLocal { get; private set; }

    private string username;

    private void OnDestroy()
    {
        list.Remove(Id);
    }

    private void Move(Vector2 pos) {
        transform.position = pos;
    }

    public static void Spawn(ushort id, string username, Vector2 position)
    {
        Player player;
        if (id == NetworkManager.Singleton.client.Id)
        {
            player = Instantiate(GameLogix.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = true;
        }
        else
        {
            player = Instantiate(GameLogix.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }

        player.name = $"Player {id} (username)";
        player.Id = id;
        player.username = username;

        list.Add(id, player);
    }

    [MessageHandler((ushort)ServerToClientId.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector2());
    }

    [MessageHandler((ushort)ServerToClientId.playerMovement)]
    private static void PlayerMovement(Message message) {
        if (list.TryGetValue(message.GetUShort(), out Player player)) player.Move(message.GetVector2());
    }

}