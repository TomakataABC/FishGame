using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawbPLayers : MonoBehaviour
{
    public GameObject playerPrefab;

    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    void Start() {
        Vector2 randomPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        PhotonNetwork.Instantiate(playerPrefab.name, randomPos, Quaternion.identity);
    }

}
