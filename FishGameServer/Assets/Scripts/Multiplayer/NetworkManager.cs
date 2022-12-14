using Riptide;
using Riptide.Utils;
using UnityEngine;

public enum ServerToClientId : ushort {
    playerSpawned = 1,
    playerMovement,
}

public enum ClientToServerId : ushort {
    name = 1,
    input,
}

public class NetworkManager : MonoBehaviour {
    
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

    public Server server { get; private set; }

    [SerializeField] private ushort port;
    [SerializeField] private ushort maxClients;
    
    private void Awake() {
        Singleton = this;
    }

    private void Start() {

        Application.targetFrameRate = 60;

        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        server = new Server();
        server.Start(port, maxClients);

        server.ClientDisconnected += PlayerLeft;

    }

    private void FixedUpdate() {
        server.Update();
    }

    private void OnApplicationQuit() {
        server.Stop();
    }

    private void PlayerLeft(object sender, ServerDisconnectedEventArgs e) {
        if (Player.list.TryGetValue(e.Client.Id, out Player player)) Destroy(player.gameObject);
    }

}
