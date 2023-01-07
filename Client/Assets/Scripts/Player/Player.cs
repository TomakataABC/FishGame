using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Riptide;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public bool IsLocal { get; private set; }

    private string username;

    public Sprite lowFish;
    public Sprite highFish;
    public Sprite pufferFish;
    public Sprite octopus;
    public Sprite shark;

    public int Score;
    [SerializeField] private Text scoreText;

    private void OnDestroy()
    {
        list.Remove(Id);
    }

    public static void Spawn(ushort id, string username, Vector3 position)
    {
        Player player;
        if (id == NetworkManager.Singleton.Client.Id)
        {
            player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = true;
        }
        else
        {
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }

        player.name = $"Player {id} (username)";
        player.Id = id;
        player.username = username;

        list.Add(id, player);
    }

    private void Move(Vector2 pos) {
        transform.position = pos;
    }

    public static void ScoreCheck(ushort id, int score) {
        Player player = list[id];

        player.Score = score;
        if (player.IsLocal)
            player.scoreText.text = score.ToString();
        player.CheckSprite();
    }

     void CheckSprite()
    {
        if (Score >= 70)
        {
            GetComponentInParent<SpriteRenderer>().enabled = false;
            enabled = false;
        }
        else if (Score >= 50 &&  GetComponentInParent<SpriteRenderer>().sprite != shark)
        {
            GetComponentInParent<SpriteRenderer>().sprite = shark;

            StartCoroutine(SmoothScaleTransitionCoroutine(0.15f, 0.15f));
        }
        else if (Score >= 30 && Score < 50 && GetComponentInParent<SpriteRenderer>().sprite != octopus)
        {
            GetComponentInParent<SpriteRenderer>().sprite = octopus;

            transform.localScale = new Vector3(0.015f, 0.055f, 1f);

            StartCoroutine(SmoothScaleTransitionCoroutine(0.085f, 0.085f));
        }
        else if (Score >= 18 && Score < 30 && GetComponentInParent<SpriteRenderer>().sprite != pufferFish)
        {
            GetComponentInParent<SpriteRenderer>().sprite = pufferFish;

            transform.localScale = new Vector3(0.035f, 0.075f, 1f);

            StartCoroutine(SmoothScaleTransitionCoroutine(0.11f, 0.07f));
        }
        else if (Score >= 10 && Score < 18 && GetComponentInParent<SpriteRenderer>().sprite != highFish)
        {
            GetComponentInParent<SpriteRenderer>().sprite = highFish;

            transform.localScale = new Vector3(0.07f, 0.1f, 1f);

            StartCoroutine(SmoothScaleTransitionCoroutine(0.04f, 0.06f));
        }
        else if (GetComponentInParent<SpriteRenderer>().sprite == null)
        {
            GetComponentInParent<SpriteRenderer>().sprite = lowFish;

            transform.localScale = new Vector3(0.09f, 0.13f, 1f);
        }
    }

    IEnumerator SmoothScaleTransitionCoroutine(float difference, float differenceTwo)
    {
        for (int i = 0; i < 100; i++)
        {
            transform.localScale = new Vector3(transform.localScale.x + difference / 100, transform.localScale.y + differenceTwo / 100, 1f);
            yield return new WaitForSecondsRealtime(0.3f / 100);
        }
    }

    [MessageHandler((ushort)ServerToClientId.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
    }

    [MessageHandler((ushort)ServerToClientId.playerMovement)]
    private static void PlayerMovement(Message message) {
        if (list.TryGetValue(message.GetUShort(), out Player player)) 
            player.Move(message.GetVector2());
    }

    [MessageHandler((ushort)ServerToClientId.playerScore)]
    private static void PlayerScore(Message message) {
        ScoreCheck(message.GetUShort(), message.GetInt());
    }


}