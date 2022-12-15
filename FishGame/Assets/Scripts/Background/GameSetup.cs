using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class GameSetup : MonoBehaviour
{
    public int PlayerCount;

    public GameObject absoluteBackground;

    [NonSerialized] public float UnitWidth;

    [NonSerialized] public float UnitHeight;

    public float Width;

    public float Height;

    private static GameObject bg;
    private static GameSetup gs;

    [NonSerialized] public float scale;

    // Start is called before the first frame update
    void Start()
    {
        ChangeMapSize(PlayerCount);
    }
    
    public void ChangeMapSize(int pCount) {
        scale = 0.8f + (pCount * 0.01f * pCount);
        transform.localScale = new Vector2(scale, scale);
        transform.position = Vector3.zero;

        UnitWidth = Width * scale;
        UnitHeight = Height * scale;

        absoluteBackground.transform.position = transform.position;
        absoluteBackground.transform.localScale = new Vector2(scale * 20, scale * 20);
    }

    [MessageHandler((ushort)ServerToClientId.mapSize)]
    private static void ChangeRoundScore(Message message) {
        bg = GameObject.Find("Background");
        gs = bg.GetComponent<GameSetup>();
        gs.ChangeMapSize(message.GetInt());
    }

}
