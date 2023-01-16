using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;
using System;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public string Username { get; private set; }
    public PlayerMovement Movement => movement;

    [SerializeField] private PlayerMovement movement;

    public int Score;
    public bool isAlive;

    private bool trigger = false;
    private float timer = 5f;
    private float timr = 7f;

    private System.Random r = new System.Random();

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

        foreach (Plankton plank in Plankton.pList.Values) {
            plank.SendSpawned(id);
        }

        GameLogic.Singleton.playerCount += 1;
        GameLogic.Singleton.XYSize += (float)0.3;

        GameLogic.Singleton.changeMap();

        Player player = Instantiate(GameLogic.Singleton.PlayerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity).GetComponent<Player>();
        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
        player.Id = id;
        player.Username = string.IsNullOrEmpty(username) ? $"Guest {id}" : username;
        player.isAlive = true;
        player.timr = 7f;

        player.SendSpawned();
        player.Score = 5;
        player.SendScore();
        list.Add(id, player);

    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.tag == "Player") {
            int otherScore = other.GetComponent<Player>().Score;
            int thisScore = GetComponentInParent<Player>().Score;

            if (otherScore > thisScore ) {
                Die();
                GetComponentInParent<CircleCollider2D>().enabled = false;
                isAlive = false;
            }

            if (otherScore < thisScore) {
                GetComponentInParent<Player>().Score += (int)Math.Round(Math.Sqrt(otherScore));
                SendScore();
            }
        } else if (other.tag == "Plankton") {
            GetComponentInParent<Player>().Score += 1;
            SendScore();
        } else if (other.tag == "Background") {
            timer = 5f;
            trigger = false;
        }

    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Background") {
            trigger = true;
        }
    }

    private void Update() {
        if (trigger)
            timer -= Time.deltaTime;

        if (!isAlive)
            timr -= Time.deltaTime;

        if (0 >= timer) {
            trigger = false;
            isAlive = false;
            timer = 5f;
            Die();
        }

        if (0 >= timr)
            Respawn();
    }

    private void Die() {
        GetComponentInParent<CircleCollider2D>().enabled = false;
        SendDeath();
        Score = 5;
    }

    private void Respawn() {
        GetComponentInParent<CircleCollider2D>().enabled = true;
        isAlive = true;
        timr = 7f;
        transform.position = new Vector3((float)(r.NextDouble() * (GameLogic.Singleton.XYSize * 2) - GameLogic.Singleton.XYSize) * 10, (float)(r.NextDouble() * (GameLogic.Singleton.XYSize * 2) - GameLogic.Singleton.XYSize) * 10, 0f);
        SendScore();
        SendRespawn();
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

    private void SendDeath() {
        NetworkManager.Singleton.Server.SendToAll(AddDeathData(Message.Create(MessageSendMode.Reliable, ServerToClientId.playerDeath)));
    }

    private void SendRespawn() {
        NetworkManager.Singleton.Server.SendToAll(AddRespawnData(Message.Create(MessageSendMode.Reliable, ServerToClientId.playerRespawn)));
    }

    private Message AddSpawnData(Message message)
    {
        message.AddUShort(Id);
        message.AddString(Username);
        message.AddBool(isAlive);
        message.AddVector3(transform.position);
        return message;
    }

    private Message AddScoreData(Message message) {
        message.AddUShort(Id);
        message.AddInt(Score);
        return message;
    }

    private Message AddDeathData(Message message) {
        message.AddUShort(Id);
        return message;
    }

    private Message AddRespawnData(Message message) {
        message.AddUShort(Id);
        message.AddVector3(transform.position);
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
            if(player.isAlive)
                player.Movement.SetInput(message.GetFloats(2));
    }
}
