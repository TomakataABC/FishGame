using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Plankton : MonoBehaviour
{
    public float Scale;

    public ServerSetup GameSetup;

    private void Start()
    {
        //tag = "Plankton";
    }

    public void Reposition()
    {
        transform.localScale = new Vector3(Scale, Scale);
        float x = UnityEngine.Random.Range(-GameSetup.UnitWidth, GameSetup.UnitHeight);
        float y = UnityEngine.Random.Range(-GameSetup.UnitWidth, GameSetup.UnitHeight);
        transform.position = new Vector3(x, y);
    }

    private void Update() {
        if (isOutOfArea()) Reposition();
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
