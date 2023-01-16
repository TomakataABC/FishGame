using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class GameLogic : MonoBehaviour
{
    private static GameLogic _singleton;
    public static GameLogic Singleton {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(GameLogic)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    private System.Random r = new System.Random();

    public GameObject PlayerPrefab => playerPrefab;
    public GameObject PlanktonPrefab => planktonPrefab;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject planktonPrefab;

    [Header("Stats")]
    [SerializeField] public int playerCount;
    [SerializeField] public int planktonCount;
    [SerializeField] public int totalPlanktonCount;

    [Header("Map")]
    [SerializeField] public float XYSize;

    private void Awake()
    {
        Singleton = this;
    }
    
    private void Update() {
        updatePlanktons();
    }

    private void updatePlanktons() {
        while ((playerCount * 2) > planktonCount) {
            Plankton.Spawn((ushort)(totalPlanktonCount + 1), new Vector3((float)(r.NextDouble() * (XYSize * 2) - XYSize) * 10, (float)(r.NextDouble() * (XYSize * 2) - XYSize) * 10, 0f));
        }
    }

    public void changeMap() {
        GameObject bg = GameObject.Find("Bg");
        bg.transform.localScale = new Vector2(XYSize, XYSize);
        SendBgChange();
    }

    private void SendBgChange() {
        NetworkManager.Singleton.Server.SendToAll(AddBgData(Message.Create(MessageSendMode.Reliable, ServerToClientId.bgChange)));
    }

    private Message AddBgData(Message message) {
        GameObject bg = GameObject.Find("Bg");
        message.AddVector2(bg.transform.localScale);
        return message;
    }

}
