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

    public int Score;

    public Text ScoreText;

    private void OnDestroy()
    {
        list.Remove(Id);
    }

    private void Move(Vector2 pos) {
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

    void Update()
    {
        CheckSprite();
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
            StartCoroutine(SmoothCameraTransitionCoroutine(1f));
        }
        else if (Score >= 30 && Score < 50 && GetComponentInParent<SpriteRenderer>().sprite != octopus)
        {
            GetComponentInParent<SpriteRenderer>().sprite = octopus;

            transform.localScale = new Vector3(transform.localScale.x - 0.02f, transform.localScale.y - 0.02f);

            StartCoroutine(SmoothScaleTransitionCoroutine(0.085f, 0.085f));
            StartCoroutine(SmoothCameraTransitionCoroutine(1f));
        }
        else if (Score >= 18 && Score < 30 && GetComponentInParent<SpriteRenderer>().sprite != pufferFish)
        {
            GetComponentInParent<SpriteRenderer>().sprite = pufferFish;

            transform.localScale = new Vector3(transform.localScale.x - 0.035f, transform.localScale.y - 0.025f);

            StartCoroutine(SmoothScaleTransitionCoroutine(0.11f, 0.07f));
            StartCoroutine(SmoothCameraTransitionCoroutine(1f));
        }
        else if (Score >= 10 && Score < 18 && GetComponentInParent<SpriteRenderer>().sprite != highFish)
        {
            GetComponentInParent<SpriteRenderer>().sprite = highFish;

            transform.localScale = new Vector3(transform.localScale.x - 0.02f, transform.localScale.y - 0.03f);

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
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector2());
    }

    [MessageHandler((ushort)ServerToClientId.playerMovement)]
    private static void PlayerMovement(Message message) {
        if (list.TryGetValue(message.GetUShort(), out Player player)) player.Move(message.GetVector2());
    }

}
