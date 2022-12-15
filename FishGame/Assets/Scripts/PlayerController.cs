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

        if (horizontalSpeed != 0)
            inputs[0] = Input.GetAxis("Horizontal");

        if (Input.GetAxis("Vertical") != 0)
            inputs[1] = Input.GetAxis("Vertical");


        if (horizontalSpeed < 0 && PlayerDirection == Direction.Left)
        {
            transform.Rotate(new Vector3(0, 180));
            PlayerDirection = Direction.Right;
        }
        else if (horizontalSpeed > 0 && PlayerDirection == Direction.Right)
        {
            transform.Rotate(new Vector3(0, 180));
            PlayerDirection = Direction.Left;
        }
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
