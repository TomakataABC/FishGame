using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class PlayerController : MonoBehaviour
{
    private float[] inputs;

    public enum Direction { Left, Right }
    public Direction PlayerDirection;

    private void Start()
    {
        inputs = new float[2];
    }

    private void Update()
    {
        float horizontalSpeed = Input.GetAxis("Horizontal");
        float verticalSpedd = Input.GetAxis("Vertical");

        inputs[0] = horizontalSpeed;
        inputs[1] = verticalSpedd;
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
        NetworkManager.Singleton.Client.Send(message);
    }
}
