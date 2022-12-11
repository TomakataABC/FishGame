using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public string Username { get; private set; }
    
    private void OnDestroy() {
        list.Remove(Id);
    }
    
    public static void Spawn (ushort id, string username) {

        foreach (Player otherPlayer in list.Values) otherPlayer.SendSpawned(id);

        Player player = Instantiate(GameLogix.Singleton.PlayerPrefab, new Vector2(0f, 0f), Quaternion.identity).GetComponent<Player>();
        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
        player.Id = id;
        player.Username = string.IsNullOrEmpty(username) ? $"Guest {id}" : username;

        player.SendSpawned();
        list.Add(id, player);

    }

    private void SendSpawned() {
        NetworkManager.Singleton.server.SendToAll(
            AddSpawnData(
                Message.Create(
                    MessageSendMode.Reliable, (ushort)ServerToClientId.playerSpawned)));
    }

    private void SendSpawned(ushort toClientId) {
        NetworkManager.Singleton.server.Send(
            AddSpawnData(
                Message.Create(
                    MessageSendMode.Reliable, (ushort)ServerToClientId.playerSpawned)), toClientId);
    }

    private Message AddSpawnData(Message message) {
        message.AddUShort(Id);
        message.AddString(Username);
        message.AddVector2(transform.position);
        return message;
    }
    
    [MessageHandler((ushort)ClientToServerId.name)]
    private static void Name(ushort fromClientId, Message message) {
        Spawn(fromClientId, message.GetString());
    }
    
}
