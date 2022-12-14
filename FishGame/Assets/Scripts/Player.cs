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

    public FollowCamera followCamera;

    public Sprite lowFish;
    public Sprite highFish;
    public Sprite pufferFish;
    public Sprite octopus;
    public Sprite shark;

    public int RoundScore;
    public int GlobalScore;

    public Text ScoreText;

    private void OnDestroy()
    {
        list.Remove(Id);
    }

    private void Move(Vector3 pos) {
        transform.position = pos;
    }

    public static void Spawn(ushort id, string username, Vector2 position)
    {
        Player player;
        if (id == NetworkManager.Singleton.client.Id)
        {
            player = Instantiate(GameLogix.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = true;
        }
        else
        {
            player = Instantiate(GameLogix.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }

        player.name = $"Player {id} (username)";
        player.Id = id;
        player.username = username;

        list.Add(id, player);
    }

    private void ChangeScore(int value, int mode) {
         if (mode == 1) {
            RoundScore = value;
         } else {
            GlobalScore = value;
         }
    }

    void Update()
    {
        CheckSprite();
        ScoreText.text = RoundScore.ToString();
    }

    void CheckSprite()
    {
        if (RoundScore >= 70)
        {
            GetComponentInParent<SpriteRenderer>().enabled = false;
            enabled = false;
        }
        else if (RoundScore >= 50 &&  GetComponentInParent<SpriteRenderer>().sprite != shark)
        {
            GetComponentInParent<SpriteRenderer>().sprite = shark;

            StartCoroutine(SmoothScaleTransitionCoroutine(0.15f, 0.15f));
            StartCoroutine(SmoothCameraTransitionCoroutine(1f));
        }
        else if (RoundScore >= 30 && RoundScore < 50 && GetComponentInParent<SpriteRenderer>().sprite != octopus)
        {
            GetComponentInParent<SpriteRenderer>().sprite = octopus;

            transform.localScale = new Vector3(transform.localScale.x - 0.02f, transform.localScale.y - 0.02f);

            StartCoroutine(SmoothScaleTransitionCoroutine(0.085f, 0.085f));
            StartCoroutine(SmoothCameraTransitionCoroutine(1f));
        }
        else if (RoundScore >= 18 && RoundScore < 30 && GetComponentInParent<SpriteRenderer>().sprite != pufferFish)
        {
            GetComponentInParent<SpriteRenderer>().sprite = pufferFish;

            transform.localScale = new Vector3(transform.localScale.x - 0.035f, transform.localScale.y - 0.025f);

            StartCoroutine(SmoothScaleTransitionCoroutine(0.11f, 0.07f));
            StartCoroutine(SmoothCameraTransitionCoroutine(1f));
        }
        else if (RoundScore >= 10 && RoundScore < 18 && GetComponentInParent<SpriteRenderer>().sprite != highFish)
        {
            GetComponentInParent<SpriteRenderer>().sprite = highFish;

            //transform.localScale = new Vector3(transform.localScale.x - 0.02f, transform.localScale.y - 0.03f);

            StartCoroutine(SmoothScaleTransitionCoroutine(0.04f, 0.06f));
            StartCoroutine(SmoothCameraTransitionCoroutine(1f));
        }
        else if (GetComponentInParent<SpriteRenderer>().sprite == null)
        {
            GetComponentInParent<SpriteRenderer>().sprite = lowFish;

            transform.localScale = new Vector3(0.09f, 0.13f);
            followCamera.z = 3.5f;
        }
    }

    IEnumerator SmoothScaleTransitionCoroutine(float difference, float differenceTwo)
    {
        for (int i = 0; i < 100; i++)
        {
            transform.localScale = new Vector3(transform.localScale.x + difference / 100, transform.localScale.y + differenceTwo / 100);
            yield return new WaitForSecondsRealtime(0.3f / 100);
        }
    }

    IEnumerator SmoothCameraTransitionCoroutine(float difference)
    {
        for (int i = 0; i < 100; i++) 
        {
            followCamera.z += difference / 100;
            yield return new WaitForSecondsRealtime(0.3f / 100);
        }
    }

    [MessageHandler((ushort)ServerToClientId.playerSpawned)]
    private static void SpawnPlayer(Message message) {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
    }

    [MessageHandler((ushort)ServerToClientId.playerMovement)]
    private static void PlayerMovement(Message message) {
        if (list.TryGetValue(NetworkManager.Singleton.client.Id, out Player player)) 
            player.Move(message.GetVector3());
    }

    [MessageHandler((ushort)ServerToClientId.roundScoreChange)]
    private static void ChangeRoundScore(Message message) {
        if (list.TryGetValue(NetworkManager.Singleton.client.Id, out Player player))
            player.ChangeScore(message.GetInt(), 1);
    }

    [MessageHandler((ushort)ServerToClientId.globalScoreChange)]
    private static void ChangeGlobalScore(Message message) {
        if (list.TryGetValue(NetworkManager.Singleton.client.Id, out Player player))
            player.ChangeScore(message.GetInt(), 0);
    }

}
