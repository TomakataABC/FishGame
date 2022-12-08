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

    public IList<Planton> Plantons;

    public IList<PlayerMovement> Players;

    public PlayerMovement Player;

    public float PlayerHeight;
    public float PlayerWidth;

    public bool[,] isOccupied;

    [NonSerialized] public float UnitWidth;

    [NonSerialized] public float UnitHeight;

    public float Width;

    public float Height;

    [NonSerialized] public float scale;

    // Start is called before the first frame update
    void Start()
    {
        scale = 0.3f + (PlayerCount * 0.008f * PlayerCount);
        transform.localScale = new Vector3(scale, scale);
        transform.position = Vector3.zero;

        SetIsOccupied();

        UnitWidth = Width * scale;
        UnitHeight = Height * scale;

        Plantons = new List<Planton>();
        Players = new List<PlayerMovement>();

        Players.Add(Player);

        foreach(var player in Players)
        {
            CreatePlayerStartPosition(player);
        }

        CreatePlantons();

        absoluteBackground.transform.position = transform.position;
        absoluteBackground.transform.localScale = new Vector3(scale * 20, scale * 20);
    }

    private void SetIsOccupied()
    {
        isOccupied = new bool[4, 4];

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                isOccupied[i, j] = false;
            }
        }
    }

    public void CreatePlayerStartPosition(PlayerMovement player)
    {
        bool isFound = false;
        float x = 0;
        float y = 0;
        float[] values = { -1, -0.5f, 0, 0.5f };

        while (!isFound)
        {
            int i = UnityEngine.Random.Range(0, 4);
            int j = UnityEngine.Random.Range(0, 4);
            if (!isOccupied[i, j])
            {
                isFound = true;
                Debug.Log(isFound);
                Debug.Log(i + " " + j);
                isOccupied[i, j] = true;
                x = values[i] * UnitWidth;
                y = values[j] * UnitHeight;
            }
        }

        float xIncrement = UnityEngine.Random.value * (UnitWidth / 2 - 2 * PlayerWidth);
        float yIncrement = UnityEngine.Random.value * (UnitHeight / 2 - 2 * PlayerWidth);

        Debug.Log(UnitWidth + " " + UnitHeight);
        Debug.Log(xIncrement + " " + yIncrement + " " + x + " " + y);
        Debug.Log(x + xIncrement + PlayerWidth);
        Debug.Log(y + yIncrement + PlayerHeight);

        player.transform.position = Vector3.zero;
        player.transform.Translate(x + xIncrement + PlayerWidth, y + yIncrement + PlayerHeight, 0);
    }

    private void CreatePlantons()
    {
        var plantus = planton.GetComponent<Planton>();
        plantus.Reposition();
        Plantons.Add(plantus);
        for (int i = 0; i < (PlayerCount * 3) - 1; i++)
        {
            var newPlanton = Instantiate(planton);
            var newPlantus = newPlanton.GetComponent<Planton>();
            newPlantus.Reposition();
            Plantons.Add(newPlantus);

        }
    }
}
