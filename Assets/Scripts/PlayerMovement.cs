using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float HorizontalSpeed { get; set; } = 0f;
    [SerializeField] float VerticalSpeed { get; set; } = 0f;

    [SerializeField] float MaxVerticalSpeed { get; set; } = 1000f;
    [SerializeField] float MaxHorizontalSpeed { get; set; } = 1000f;

    [SerializeField] int Score { get; set; } = 0;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        ReactToAreaBoundries();
        ReactToInput();
        transform.Translate(HorizontalSpeed * Time.deltaTime, VerticalSpeed * Time.deltaTime,0);
    }

    void ReactToInput()
    {
        float horizontalSpeed = Input.GetAxis("Horizontal");
        float verticalSpeed = Input.GetAxis("Vertical");
        HorizontalSpeed = (HorizontalSpeed + horizontalSpeed * (float)Math.Log(MaxHorizontalSpeed - HorizontalSpeed)) * 0.8f;
        VerticalSpeed = (VerticalSpeed + verticalSpeed * (float)Math.Log(MaxVerticalSpeed - VerticalSpeed)) * 0.8f;

        HorizontalSpeed = HorizontalSpeed / (Math.Abs(VerticalSpeed * Time.deltaTime) + 1);
        VerticalSpeed = VerticalSpeed / (Math.Abs(HorizontalSpeed * Time.deltaTime) + 1);

        Debug.Log(HorizontalSpeed);
    }

    void ReactToAreaBoundries()
    {

    }
}
