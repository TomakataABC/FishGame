using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class Plankton : MonoBehaviour
{
    public static Dictionary<ushort, Plankton> pList = new Dictionary<ushort, Plankton>();

    public ushort Id { get; private set; }

    private void OnDestroy()
    {
        pList.Remove(Id);
    }

    public static void Spawn(ushort id, Vector3 poz) {
        Plankton plankton = Instantiate(GameLogic.Singleton.PlanktonPrefab, poz, Quaternion.identity).GetComponent<Plankton>();

        plankton.Id = id;
        pList.Add(id, plankton);

        GameLogic.Singleton.planktonCount += 1;
        GameLogic.Singleton.totalPlanktonCount += 1;

        plankton.SendSpawned();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            SendDeath();
            GameLogic.Singleton.planktonCount -= 1;
            Destroy(this.gameObject);
        }
    }

    private void SendSpawned() {
        NetworkManager.Singleton.Server.SendToAll(AddSpawnData(Message.Create(MessageSendMode.Reliable, ServerToClientId.planktonSpawn)));
    }

    public void SendSpawned(ushort toClientId) {
        NetworkManager.Singleton.Server.Send(AddSpawnData(Message.Create(MessageSendMode.Reliable, ServerToClientId.planktonSpawn)), toClientId);
    }

    private void SendDeath() {
        NetworkManager.Singleton.Server.SendToAll(AddDeathData(Message.Create(MessageSendMode.Reliable, ServerToClientId.planktonDie)));
    }

    private Message AddSpawnData(Message message) {
        message.AddUShort(Id);
        message.AddVector3(transform.position);
        return message;
    }

    private Message AddDeathData(Message message) {
        message.AddUShort(Id);
        return message;
    }

}
