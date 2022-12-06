using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public GameSetup GameSetup;

    public float HorizontalSpeed;
    public float VerticalSpeed;

    public float HorizontalSpeedOut;
    public float VerticalSpeedOut;

    public float MaxVerticalSpeed;
    public float MaxHorizontalSpeed;

    public enum Direction { Left, Right }

    public Direction PlayerDirection;

    public int Score;

    private void Start()
    {
        HorizontalSpeedOut = 0;
        VerticalSpeedOut = 0;

        HorizontalSpeed = 0;
        VerticalSpeed = 0;

        float x = UnityEngine.Random.Range(-GameSetup.UnitWidth, GameSetup.UnitHeight);
        float y = UnityEngine.Random.Range(-GameSetup.UnitWidth, GameSetup.UnitHeight);
        transform.position = new Vector3(x, y);
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
        
    }

    bool isOutOfArea()
    {
        if (Math.Abs(transform.position.x) > GameSetup.UnitWidth || transform.position.y > GameSetup.UnitHeight)
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
        Debug.Log("Planton taken.");
        yield return new WaitForSecondsRealtime(3);
        plantus.enabled = true;
        plantus.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
