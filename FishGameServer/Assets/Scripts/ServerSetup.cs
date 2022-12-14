using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ServerSetup : MonoBehaviour
{
    public int PlayerCount;

    public GameObject absoluteBackground;

    public GameObject plankton;

    public IList<Plankton> Plantons;

    public float PlayerHeight;
    public float PlayerWidth;

    public bool[,] isOccupied;



    public float Width;

    public float Height;

    [NonSerialized] public float scale;

    [Header("Debug")]
    public float UnitWidth;
    public float UnitHeight;

    private void Start() {
        scale = 0.8f + (PlayerCount * 0.01f * PlayerCount);
        transform.localScale = new Vector3(scale, scale);
        transform.position = Vector3.zero;

        UnitWidth = Width * scale;
        UnitHeight = Height * scale;

        Plantons = new List<Plankton>();

        CreatePlantons();

        absoluteBackground.transform.position = transform.position;
        absoluteBackground.transform.localScale = new Vector3(scale * 20, scale * 20);        
    }

    private void Update() {
        PlayerCount = Player.list.Count + 1;
        scale = 0.8f + (PlayerCount * 0.01f * PlayerCount);
        transform.localScale = new Vector3(scale, scale);
        UnitWidth = Width * scale;
        UnitHeight = Height * scale;
        absoluteBackground.transform.position = transform.position;
        absoluteBackground.transform.localScale = new Vector3(scale * 20, scale * 20); 
    }

    private void CreatePlantons()
    {
        var plantus = plankton.GetComponent<Plankton>();
        plantus.Reposition();
        Plantons.Add(plantus);
        for (int i = 0; i < (PlayerCount * 2) - 1; i++)
        {
            var newPlanton = Instantiate(plankton);
            var newPlantus = newPlanton.GetComponent<Plankton>();
            newPlantus.Reposition();
            Plantons.Add(newPlantus);

        }
    }
}
