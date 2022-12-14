using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlanktonMovement : MonoBehaviour
{
    private System.Random random;

    private float HorizontalSpeedOut;
    private float VerticalSpeedOut;

    private float HorizontalSpeed;
    private float VerticalSpeed;

    int isTurningLeft;

    public float Increment;

    public float MaxSpeed;

    void Start()
    {
        HorizontalSpeed = 0;
        VerticalSpeed = 0;

        HorizontalSpeedOut = 0;
        VerticalSpeedOut = 0;

        random = new System.Random();
        isTurningLeft = random.Next(0, 2);
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        HorizontalSpeed = IncrementTo(HorizontalSpeed); 
        VerticalSpeed = IncrementTo(VerticalSpeed);
        transform.Translate(new Vector3((HorizontalSpeed * Time.deltaTime) + (HorizontalSpeedOut * Time.deltaTime), (VerticalSpeed * Time.deltaTime) + (VerticalSpeedOut * Time.deltaTime)));
    }

    float IncrementTo(float theBasis)
    {
        if (Mathf.Abs(theBasis) < MaxSpeed)
        {
            int boolean = random.Next(0,2);
            if (boolean == 0)
            {
                theBasis += Increment;
            }
            else
            {
                theBasis -= Increment;
            }
        }
        else if (theBasis < 0)
        {
            theBasis += Increment;
        }
        else if (theBasis > 0)
        {
            theBasis -= Increment;
        }

        return theBasis;
    }
}
