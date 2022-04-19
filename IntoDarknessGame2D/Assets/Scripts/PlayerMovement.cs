using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private bool isOnGround = true;
    private float horizontalInput;

    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float jumpForce;
    [SerializeField]
    float fallMultiplier;
    [SerializeField]
    float lowJumpMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        // Gets the rigidbody component of player
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0)
        {
            playerRb.velocity = new Vector2(moveSpeed, playerRb.velocity.y);
        }
        else if (horizontalInput < 0)
        {
            playerRb.velocity = new Vector2(-moveSpeed, playerRb.velocity.y);
        }
        else
        {
            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
            isOnGround = false;
        }

        if (playerRb.velocity.y < 0)
        {
            playerRb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }
        else if (playerRb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            playerRb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }
}
