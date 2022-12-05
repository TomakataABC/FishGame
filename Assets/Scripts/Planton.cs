using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planton : MonoBehaviour
{
    public float Scale;

    public BackgroundSetup Background;

    public void Reposition()
    {
        transform.localScale = new Vector3(Scale, Scale);
        float x = Random.Range(-BackgroundSize(Background.Width), BackgroundSize(Background.Width));
        float y = Random.Range(-BackgroundSize(Background.Height), BackgroundSize(Background.Height));
        Debug.Log(Background.scale);
        Debug.Log(Background.scale);
        transform.position = new Vector3(x, y);
    }

    float BackgroundSize(float dimension)
    {
        return Background.scale * dimension;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
