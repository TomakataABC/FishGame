using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planton : MonoBehaviour
{
    public float Scale;

    public GameSetup GameSetup;

    private void Start()
    {
        tag = "Planton";
    }

    public void Reposition()
    {
        transform.localScale = new Vector3(Scale, Scale);
        float x = Random.Range(-GameSetup.UnitWidth, GameSetup.UnitHeight);
        float y = Random.Range(-GameSetup.UnitWidth, GameSetup.UnitHeight);
        transform.position = new Vector3(x, y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
