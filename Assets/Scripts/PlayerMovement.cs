using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float HorizontalSpeed;
    public float VerticalSpeed;

    public float MaxVerticalSpeed;
    public float MaxHorizontalSpeed;

    public enum Direction { Left, Right }

    public Direction PlayerDirection;

    public int Score;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Turn();
    }

    void Move()
    {
        ReactToAreaBoundries();
        ReactToInput();
        if (PlayerDirection == Direction.Right)
        {
            transform.Translate(-(HorizontalSpeed * Time.deltaTime), (VerticalSpeed * Time.deltaTime), 0);
        }
        else
        {
            transform.Translate(HorizontalSpeed * Time.deltaTime, (VerticalSpeed * Time.deltaTime), 0);
        }
    }

    void ReactToInput()
    {
        float horizontalSpeed = Input.GetAxis("Horizontal");
        float verticalSpeed = Input.GetAxis("Vertical");
        HorizontalSpeed = (HorizontalSpeed + horizontalSpeed * (float)Math.Log(MaxHorizontalSpeed - HorizontalSpeed)) * 0.5f;
        VerticalSpeed = (VerticalSpeed + verticalSpeed * (float)Math.Log(MaxVerticalSpeed - VerticalSpeed)) * 0.5f;

        HorizontalSpeed = PlayerDirection == Direction.Left ? HorizontalSpeed / (Math.Abs(VerticalSpeed * Time.deltaTime) + 1) : -1 * (HorizontalSpeed / (Math.Abs(VerticalSpeed * Time.deltaTime) - 1));
        VerticalSpeed = VerticalSpeed / (Math.Abs(HorizontalSpeed * Time.deltaTime) + 1);
    }

    void Turn()
    {
        float horizontalSpeed = Input.GetAxis("Horizontal");
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

    void ReactToAreaBoundries()
    {

    }
}
