using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameSetup : MonoBehaviour
{
    public int PlayerCount;

    public GameObject absoluteBackground;

    [NonSerialized] public float UnitWidth;

    [NonSerialized] public float UnitHeight;

    public float Width;

    public float Height;

    [NonSerialized] public float scale;

    // Start is called before the first frame update
    void Start()
    {
        scale = 0.8f + (PlayerCount * 0.01f * PlayerCount);
        transform.localScale = new Vector3(scale, scale);
        transform.position = Vector3.zero;

        UnitWidth = Width * scale;
        UnitHeight = Height * scale;

        absoluteBackground.transform.position = transform.position;
        absoluteBackground.transform.localScale = new Vector3(scale * 20, scale * 20);
    }
}
