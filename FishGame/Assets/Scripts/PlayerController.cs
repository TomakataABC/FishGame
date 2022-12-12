using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class PlayerController : MonoBehaviour
{
    private float[] inputs;

    private void Start()
    {
        inputs = new float[2];
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
            inputs[0] = Input.GetAxis("Horizontal");

        if (Input.GetAxis("Vertical") != 0)
            inputs[1] = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        SendInput();

        for (int i = 0; i < inputs.Length; i++)
            inputs[i] = 0;
    }

    private void SendInput()
    {
        Message message = Message.Create(MessageSendMode.Unreliable, ClientToServerId.input);
        message.AddFloats(inputs, false);
        NetworkManager.Singleton.client.Send(message);
    }

}
