using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Riptide;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;

    public static UIManager Singleton {

        get => _singleton;
        private set {
            if (_singleton == null) _singleton = value;
            else {
                Debug.Log($"{nameof(UIManager)} already exists");
                Destroy(value);
            }
        }

    }

    [Header("Connect")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private InputField username;
    [SerializeField] private InputField ipPort;

    private void Awake() {
        Singleton = this;
    }

    public void ConnectClicked() {
        ipPort.interactable = false;
        username.interactable = false;
        mainMenu.SetActive(false);

        NetworkManager.Singleton.Connect(ipPort.text);
    }

    public void BackToMain() {
        ipPort.interactable = true;
        username.interactable = true;
        mainMenu.SetActive(true);
    }
    
    public void SendName() {
        Message message = Message.Create(MessageSendMode.Reliable, ClientToServerId.name);
        message.AddString(username.text);
        NetworkManager.Singleton.client.Send(message);
    }

}
