using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Riptide;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Player player;

    [NonSerialized] public float HorizontalSpeed;
    [NonSerialized] public float VerticalSpeed;

    private GameObject sss;
    private ServerSetup ss;

    [NonSerialized] public float HorizontalSpeedOut;
    [NonSerialized] public float VerticalSpeedOut;

    public float MaxVerticalSpeed;
    public float MaxHorizontalSpeed;

    public enum Direction { Left, Right }

    public Direction PlayerDirection;

    private float[] inputs;

    float horizontalSpeed;
    float verticalSpeed;

    private void OnValidate() {
        if (player == null) player = GetComponent<Player>();
    }

    private void Start() {
        inputs = new float[2];

        HorizontalSpeedOut = 0;
        VerticalSpeedOut = 0;

        HorizontalSpeed = 0;
        VerticalSpeed = 0;

        sss = GameObject.Find("Bg");
        ss = sss.GetComponent<ServerSetup>();
    }

    private void FixedUpdate() {
        if (inputs[0] != 0) horizontalSpeed = inputs[0];
        if (inputs[1] != 0) verticalSpeed = inputs[1];
        Move();
        Turn();
    }

    private void Move() {
        ReactToAreaBoundries();
        ReactToInput();
        if (PlayerDirection == Direction.Right)
        {
            transform.Translate(-(HorizontalSpeed * Time.deltaTime) + (HorizontalSpeedOut * Time.deltaTime), (VerticalSpeed * Time.deltaTime)+(VerticalSpeedOut * Time.deltaTime), 0);
        }
        else
        {
            transform.Translate((HorizontalSpeed * Time.deltaTime) + (HorizontalSpeedOut * Time.deltaTime), (VerticalSpeed * Time.deltaTime) + (VerticalSpeedOut * Time.deltaTime), 0);
        }

        SendMovement();
    }

    private void ReactToInput()
    {
        HorizontalSpeed = (HorizontalSpeed + horizontalSpeed * (float)Math.Log(MaxHorizontalSpeed - HorizontalSpeed)) * 0.5f;
        VerticalSpeed = (VerticalSpeed + verticalSpeed * (float)Math.Log(MaxVerticalSpeed - VerticalSpeed)) * 0.5f;

        HorizontalSpeed = PlayerDirection == Direction.Left ? HorizontalSpeed / (Math.Abs(VerticalSpeed * Time.deltaTime) + 1) : -1 * (HorizontalSpeed / (Math.Abs(VerticalSpeed * Time.deltaTime) - 1));
        VerticalSpeed = VerticalSpeed / (Math.Abs(HorizontalSpeed * Time.deltaTime) + 1);
    }

    private void Turn()
    {
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

    private void ReactToAreaBoundries()
    {
        float increment = 0.05f;
        if (isOutOfArea())
        {
            if (transform.position.x > ss.UnitWidth)
            {
                HorizontalSpeedOut = PlayerDirection == Direction.Left ? HorizontalSpeedOut - increment : HorizontalSpeedOut + increment;
            }
            if (transform.position.x < -1 * ss.UnitWidth)
            {
                HorizontalSpeedOut = PlayerDirection == Direction.Left ? HorizontalSpeedOut + increment : HorizontalSpeedOut - increment;
            }
            if (transform.position.y > ss.UnitHeight)
            {
                VerticalSpeedOut = VerticalSpeedOut - increment;
            }
            if (transform.position.y < -1 * ss.UnitHeight)
            {
                VerticalSpeedOut = VerticalSpeedOut + increment;
            }
        }
        else
        {
            if ((HorizontalSpeedOut < increment && HorizontalSpeedOut > 0) || (HorizontalSpeedOut > -increment && HorizontalSpeedOut < 0))
            {
                HorizontalSpeedOut = 0;
            }
            else if (HorizontalSpeedOut > 0)
            {
                HorizontalSpeedOut = HorizontalSpeedOut - increment;
            }
            else if (HorizontalSpeedOut < 0)
            {
                HorizontalSpeedOut = HorizontalSpeedOut + increment;
            }

            if ((VerticalSpeedOut < increment && VerticalSpeedOut > 0) || (VerticalSpeedOut > -increment && VerticalSpeedOut < 0))
            {
                VerticalSpeedOut = 0;
            }
            else if(VerticalSpeedOut > 0)
            {
                VerticalSpeedOut = VerticalSpeedOut - increment;
            }
            else if (VerticalSpeedOut < 0)
            {
                VerticalSpeedOut = VerticalSpeedOut + increment;
            }
        }
    }

    private bool isOutOfArea() {
        if (Math.Abs(transform.position.x) > ss.UnitWidth || Math.Abs(transform.position.y) > ss.UnitHeight)
        {
            return true;
        }
        return false;
    }


    public void SetInputs(float[] inputs) {
        this.inputs = inputs;
    } 
    
    private void SendMovement() {
        Message message = Message.Create(MessageSendMode.Unreliable, ServerToClientId.playerMovement);
        message.AddUShort(player.Id);
        message.AddVector3(transform.position);
        NetworkManager.Singleton.server.SendToAll(message);
    }

}
