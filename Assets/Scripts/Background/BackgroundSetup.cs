using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BackgroundSetup : MonoBehaviour
{
    public int PlayerCount;

    public GameObject planton;

    public float Width;

    public float Height;

    [NonSerialized] public float scale;

    // Start is called before the first frame update
    void Start()
    {
        scale = 1.5f + (PlayerCount * 0.005f * PlayerCount);
        transform.localScale = new Vector3(scale, scale);
        var plantus = planton.GetComponent("Planton") as Planton;
        plantus.Reposition();
    }
}
