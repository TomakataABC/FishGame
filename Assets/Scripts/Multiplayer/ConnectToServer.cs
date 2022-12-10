using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.CreateRoom("AmongY");
    }

    public override void OnJoinedRoom() {
        SceneManager.LoadScene("GameScene");
    }

    public void OnJoinRoomFailed() {
        Debug.Log("No open room found, creating a new one");
        PhotonNetwork.CreateRoom("AmongY");
    }

}
