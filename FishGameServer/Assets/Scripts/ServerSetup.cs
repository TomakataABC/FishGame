using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Riptide;

public class ServerSetup : MonoBehaviour
{
    public int PlayerCount;

    public GameObject absoluteBackground;

    public GameObject plankton;

    public IList<Plankton> Plantons;

    private Vector2[] planktonPos;

    public float Width;

    public float Height;

    [NonSerialized] public float scale;

    [Header("Debug")]
    public float UnitWidth;
    public float UnitHeight;

    private void Start() {
        scale = 0.8f + (PlayerCount * 0.01f * PlayerCount);
        transform.localScale = new Vector2(scale, scale);
        transform.position = Vector3.zero;

        UnitWidth = Width * scale;
        UnitHeight = Height * scale;

        Plantons = new List<Plankton>();

        CreatePlantons();

        absoluteBackground.transform.position = transform.position;
        absoluteBackground.transform.localScale = new Vector2(scale * 20, scale * 20);        
    }

    private void Update() {
        PlayerCount = Player.list.Count + 1;
        scale = 0.8f + (PlayerCount * 0.01f * PlayerCount);
        transform.localScale = new Vector2(scale, scale);
        UnitWidth = Width * scale;
        UnitHeight = Height * scale;
        absoluteBackground.transform.position = transform.position;
        absoluteBackground.transform.localScale = new Vector2(scale * 20, scale * 20); 
        SendNewMap();
    }

    private void CreatePlantons()
    {
        for (int i = 0; i < (PlayerCount * 2); i++)
        {
            var newPlanton = Instantiate(plankton);
            var newPlantus = newPlanton.GetComponent<Plankton>();
            newPlanton.name = $"Plankton {i}";
            newPlantus.Reposition();
            Plantons.Add(newPlantus);
        }
        plankton.SetActive(false);
    }

    private void SendNewMap() {
        Message message = Message.Create(MessageSendMode.Unreliable, ServerToClientId.mapSize);
        message.AddInt(PlayerCount);
        NetworkManager.Singleton.server.SendToAll(message);
    }

    public void SendPlanktonSpawnSingle(ushort id) {
        Message message = Message.Create(MessageSendMode.Reliable, ServerToClientId.planktonSpawn);
        message.AddInt(Plantons.Count);
        NetworkManager.Singleton.server.Send(message, id);
    }

}
