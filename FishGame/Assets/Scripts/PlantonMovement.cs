using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantonMovement : MonoBehaviour
{
    public GameSetup GameSetup;

    private System.Random random;

    private float HorizontalSpeedOut;
    private float VerticalSpeedOut;

    private float HorizontalSpeed;
    private float VerticalSpeed;

    int isTurningLeft;

    public float Increment;

    public float MaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        HorizontalSpeed = 0;
        VerticalSpeed = 0;

        HorizontalSpeedOut = 0;
        VerticalSpeedOut = 0;

        random = new System.Random();
        isTurningLeft = random.Next(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        HorizontalSpeed = IncrementTo(HorizontalSpeed); 
        VerticalSpeed = IncrementTo(VerticalSpeed);
        ReactToAreaBoundries();
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

    void ReactToAreaBoundries()
    {
        float increment = 0.03f;
        if (isOutOfArea())
        {
            if (transform.position.x > GameSetup.UnitWidth)
            {
                HorizontalSpeedOut = HorizontalSpeedOut - increment;
            }
            if (transform.position.x < -1 * GameSetup.UnitWidth)
            {
                HorizontalSpeedOut = HorizontalSpeedOut + increment;
            }
            if (transform.position.y > GameSetup.UnitHeight)
            {
                VerticalSpeedOut = VerticalSpeedOut - increment;
            }
            if (transform.position.y < -1 * GameSetup.UnitHeight)
            {
                VerticalSpeedOut = VerticalSpeedOut + increment;
            }
        }
    }

    bool isOutOfArea()
    {
        if (Math.Abs(transform.position.x) > GameSetup.UnitWidth || Math.Abs(transform.position.y) > GameSetup.UnitHeight)
        {
            return true;
        }
        return false;
    }
}
