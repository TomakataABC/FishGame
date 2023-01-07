using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public string Username { get; private set; }
    public PlayerMovement Movement => movement;

    [SerializeField] private PlayerMovement movement;

    public int Score;

    private void OnDestroy()
    {
        list.Remove(Id);
    }

    public static void Spawn(ushort id, string username)
    {
        foreach (Player otherPlayer in list.Values) {
            otherPlayer.SendSpawned(id);
            otherPlayer.SendScore();
        }

        Player player = Instantiate(GameLogic.Singleton.PlayerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity).GetComponent<Player>();
        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
        player.Id = id;
        player.Username = string.IsNullOrEmpty(username) ? $"Guest {id}" : username;

        player.SendSpawned();
        player.Score = 5;
        player.SendScore();
        list.Add(id, player);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        
    }

    private void SendSpawned()
    {
        NetworkManager.Singleton.Server.SendToAll(AddSpawnData(Message.Create(MessageSendMode.Reliable, ServerToClientId.playerSpawned)));
    }

    private void SendSpawned(ushort toClientId)
    {
        NetworkManager.Singleton.Server.Send(AddSpawnData(Message.Create(MessageSendMode.Reliable, ServerToClientId.playerSpawned)), toClientId);
    }

    private void SendScore() {
        NetworkManager.Singleton.Server.SendToAll(AddScoreData(Message.Create(MessageSendMode.Reliable, ServerToClientId.playerScore)));
    }

    private void SendScore(ushort id) {
        NetworkManager.Singleton.Server.Send(AddScoreData(Message.Create(MessageSendMode.Reliable, ServerToClientId.playerScore)), id);
    }

    private Message AddSpawnData(Message message)
    {
        message.AddUShort(Id);
        message.AddString(Username);
        message.AddVector3(transform.position);
        return message;
    }

    private Message AddScoreData(Message message) {
        message.AddUShort(Id);
        message.AddInt(Score);
        return message;
    }

    [MessageHandler((ushort)ClientToServerId.name)]
    private static void Name(ushort fromClientId, Message message)
    {
        Spawn(fromClientId, message.GetString());
    }

    [MessageHandler((ushort)ClientToServerId.input)]
    private static void Input(ushort fromClientId, Message message)
    {
        if (list.TryGetValue(fromClientId, out Player player))
            player.Movement.SetInput(message.GetFloats(2));
    }
}
