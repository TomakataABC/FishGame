using Riptide;
using Riptide.Utils;
using UnityEngine;

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

    public Server server {get; private set; }

    [SerializeField] private ushort port;
    [SerializeField] private ushort maxClients;
    
    private void Awake() {
        Singleton = this;
    }

    private void Start() {

        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        server = new Server();
        server.Start(port, maxClients);
    }

    private void FixedUpdate() {
        server.Update();
    }

    private void OnApplicationQuit() {
        server.Stop();
    }

}
