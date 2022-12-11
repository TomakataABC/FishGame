using UnityEngine;
using Riptide;
using Riptide.Utils;
using System;

public enum ServerToClientId : ushort {
    playerSpawned = 1,
}

public enum ClientToServerId : ushort {
    name = 1,
}

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;

    public static NetworkManager Singleton {

        get => _singleton;
        private set {
            if (_singleton == null) _singleton = value;
            else {
                Debug.Log($"{nameof(NetworkManager)} already exists");
                Destroy(value);
            }
        }

    }

    public Client client { get; private set; }

    [SerializeField] private string ip;
    [SerializeField] private ushort port;

    private void Awake() {
        Singleton = this;
    }

    private void Start() {

        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        client = new Client();

        client.Connected += DidConnect;
        client.ConnectionFailed += FailedToConnect;
        client.ClientDisconnected += PlayerLeft;
        client.Disconnected += DidDisconnect;
        
    }

    private void FixedUpdate() {
        client.Update();
    }

    private void OnApplicationQuit() {
        client.Disconnect();
    }

    public void Connect(string ipPort) {
        client.Connect(ipPort);
    }

    private void DidConnect(object sender, EventArgs e) {
        UIManager.Singleton.SendName();
    }
    
    private void FailedToConnect(object sender, EventArgs e) {
        UIManager.Singleton.BackToMain();
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e) {
        Destroy(Player.list[e.Id].gameObject);
    }

    private void DidDisconnect(object sender, EventArgs e) {
        UIManager.Singleton.BackToMain();
    }

}
