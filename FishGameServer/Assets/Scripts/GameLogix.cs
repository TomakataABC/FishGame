using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogix : MonoBehaviour
{
    private static GameLogix _singleton;

    public static GameLogix Singleton {

        get => _singleton;
        private set {
            if (_singleton == null) _singleton = value;
            else {
                Debug.Log($"{nameof(GameLogix)} already exists");
                Destroy(value);
            }
        }

    }

    public GameObject PlayerPrefab => playerPrefab;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;

    private void Awake() {
        Singleton = this;
    }

}
