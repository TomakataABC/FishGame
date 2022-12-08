using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public FollowCamera followCamera;

    public Sprite lowFish;
    public Sprite highFish;
    public Sprite pufferFish;
    public Sprite octopus;
    public Sprite shark;

    public GameSetup GameSetup;

    [NonSerialized] public float HorizontalSpeed;
    [NonSerialized] public float VerticalSpeed;

    [NonSerialized] public float HorizontalSpeedOut;
    [NonSerialized] public float VerticalSpeedOut;

    public float MaxVerticalSpeed;
    public float MaxHorizontalSpeed;

    public enum Direction { Left, Right }

    public Direction PlayerDirection;

    public int Score;
    
    void Start()
    {
        HorizontalSpeedOut = 0;
        VerticalSpeedOut = 0;

        HorizontalSpeed = 0;
        VerticalSpeed = 0;

        Score = 5;

        tag = "Player";
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Turn();
        CheckSprite();
    }

    void CheckSprite()
    {
        if (Score >= 70)
        {
            GetComponentInParent<SpriteRenderer>().enabled = false;
            enabled = false;
        }
        else if (Score >= 50 &&  GetComponentInParent<SpriteRenderer>().sprite != shark)
        {
            GetComponentInParent<SpriteRenderer>().sprite = shark;

            StartCoroutine(SmoothScaleTransitionCoroutine(0.15f, 0.15f));
            StartCoroutine(SmoothCameraTransitionCoroutine(1f));
        }
        else if (Score >= 30 && Score < 50 && GetComponentInParent<SpriteRenderer>().sprite != octopus)
        {
            GetComponentInParent<SpriteRenderer>().sprite = octopus;

            transform.localScale = new Vector3(transform.localScale.x - 0.02f, transform.localScale.y - 0.02f);

            StartCoroutine(SmoothScaleTransitionCoroutine(0.085f, 0.085f));
            StartCoroutine(SmoothCameraTransitionCoroutine(1f));
        }
        else if (Score >= 18 && Score < 30 && GetComponentInParent<SpriteRenderer>().sprite != pufferFish)
        {
            GetComponentInParent<SpriteRenderer>().sprite = pufferFish;

            transform.localScale = new Vector3(transform.localScale.x - 0.035f, transform.localScale.y - 0.025f);

            StartCoroutine(SmoothScaleTransitionCoroutine(0.11f, 0.07f));
            StartCoroutine(SmoothCameraTransitionCoroutine(1f));
        }
        else if (Score >= 10 && Score < 18 && GetComponentInParent<SpriteRenderer>().sprite != highFish)
        {
            GetComponentInParent<SpriteRenderer>().sprite = highFish;

            transform.localScale = new Vector3(transform.localScale.x - 0.02f, transform.localScale.y - 0.03f);

            StartCoroutine(SmoothScaleTransitionCoroutine(0.04f, 0.06f));
            StartCoroutine(SmoothCameraTransitionCoroutine(1f));
        }
        else if (GetComponentInParent<SpriteRenderer>().sprite == null)
        {
            GetComponentInParent<SpriteRenderer>().sprite = lowFish;

            transform.localScale = new Vector3(0.09f, 0.13f);
            followCamera.z = 3.5f;
        }
    }

    IEnumerator SmoothScaleTransitionCoroutine(float difference, float differenceTwo)
    {
        for (int i = 0; i < 100; i++)
        {
            transform.localScale = new Vector3(transform.localScale.x + difference / 100, transform.localScale.y + differenceTwo / 100);
            yield return new WaitForSecondsRealtime(0.3f / 100);
        }
    }

    IEnumerator SmoothCameraTransitionCoroutine(float difference)
    {
        for (int i = 0; i < 100; i++) 
        {
            followCamera.z += difference / 100;
            yield return new WaitForSecondsRealtime(0.3f / 100);
        }
    }

    void Move()
    {
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
        float increment = 0.05f;
        if (isOutOfArea())
        {
            if (transform.position.x > GameSetup.UnitWidth)
            {
                HorizontalSpeedOut = PlayerDirection == Direction.Left ? HorizontalSpeedOut - increment : HorizontalSpeedOut + increment;
            }
            if (transform.position.x < -1 * GameSetup.UnitWidth)
            {
                HorizontalSpeedOut = PlayerDirection == Direction.Left ? HorizontalSpeedOut + increment : HorizontalSpeedOut - increment;
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
        else
        {
            if (HorizontalSpeedOut > 0)
            {
                HorizontalSpeedOut = HorizontalSpeedOut - increment;
            }
            else if (HorizontalSpeedOut < 0)
            {
                HorizontalSpeedOut = HorizontalSpeedOut + increment;
            }

            if (VerticalSpeedOut > 0)
            {
                VerticalSpeedOut = VerticalSpeedOut - increment;
            }
            else if (HorizontalSpeedOut < 0)
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Planton")
        {
            StartCoroutine(TriggerPlanton(other));
        }
    }

    IEnumerator TriggerPlanton(Collider2D planton)
    {
        yield return new WaitForSeconds(0.05f);
        var plantus = planton.GetComponentInParent<Planton>();
        plantus.enabled = false;
        plantus.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        plantus.Reposition();
        Score++;
        yield return new WaitForSecondsRealtime(3);
        plantus.enabled = true;
        plantus.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
