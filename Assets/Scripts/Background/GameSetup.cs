using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameSetup : MonoBehaviour
{
    public int PlayerCount;

    public GameObject absoluteBackground;

    public GameObject planton;

    public float UnitWidth;

    public float UnitHeight;

    public float Width;

    public float Height;

    [NonSerialized] public float scale;

    // Start is called before the first frame update
    void Start()
    {
        scale = 0.3f + (PlayerCount * 0.008f * PlayerCount);
        transform.localScale = new Vector3(scale, scale);
        transform.position = Vector3.zero;

        UnitWidth = Width * scale;
        UnitHeight = Height * scale;

        CreatePlantons();

        absoluteBackground.transform.position = transform.position;
        absoluteBackground.transform.localScale = new Vector3(scale * 20, scale * 20);
    }

    void CreatePlantons()
    {
        var plantus = planton.GetComponent<Planton>();
        plantus.Reposition();
        for (int i = 0; i < (PlayerCount * 3) - 1; i++)
        {
            var newPlanton = Instantiate(planton);
            var newPlantus = newPlanton.GetComponent<Planton>();
            newPlantus.Reposition();
        }
    }
}
