using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Player player;
    private float[] inputs;

    public float movementSpeed;

    float horizontalSpeed;
    float verticalSpeed;

    private Vector2 moveInput;

    private Rigidbody2D rb;

    public void SetInput(float[] input) {
        inputs = input;
    }

     private void OnValidate()
    {
        if (player == null)
            player = GetComponent<Player>();
    }

    private void Start() {
        inputs = new float[2];
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        horizontalSpeed = inputs[0];
        verticalSpeed = inputs[1];

        if (horizontalSpeed > 0) GetComponent<SpriteRenderer>().flipX = false;
        if (horizontalSpeed < 0) GetComponent<SpriteRenderer>().flipX = true;

        Move();
    }

    private void Move() {
        moveInput.x = horizontalSpeed;
        moveInput.y = verticalSpeed;

        moveInput.Normalize();

        rb.velocity = moveInput * movementSpeed;

        SendMovement();
    }

    private void SendMovement() {
        Message message = Message.Create(MessageSendMode.Unreliable, ServerToClientId.playerMovement);
        message.AddUShort(player.Id);
        message.AddVector2(transform.position);
        message.AddBool(GetComponent<SpriteRenderer>().flipX);
        NetworkManager.Singleton.Server.SendToAll(message);
    }

}
