using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private CapsuleCollider2D playerCc;
    private bool isOnGround = true;
    private float horizontalInput;
    public static bool playerHide = false;

    [SerializeField]
    private float moveSpeed;
    private float oldMoveSpeed;
    [SerializeField]
    private float jumpForce;
    private float oldJumpForce;
    [SerializeField]
    private float fallMultiplier;
    [SerializeField]
    private float lowJumpMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        // Gets the rigidbody component of player
        playerRb = GetComponent<Rigidbody2D>();
        playerCc = GetComponent<CapsuleCollider2D>();
        oldMoveSpeed = moveSpeed;
        oldJumpForce = jumpForce;
    }

    // Update is called once per frame
    void Update()
    {
        // Gets the horizontal input of the keyboard 1 if right -1 if left
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0)
        {
            // Moves player to the right 
            playerRb.velocity = new Vector2(moveSpeed, playerRb.velocity.y);
        }
        else if (horizontalInput < 0)
        {
            // Moves player to the left
            playerRb.velocity = new Vector2(-moveSpeed, playerRb.velocity.y);
        }
        else
        {
            // Makes player do an immediate stop when neither left or right is being pressed
            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
        }

        // Code to make player jump
        // Checks if player in on ground before being able to jump again
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
            // Makes it so player can't jump while in the air.
            isOnGround = false;
        }

        if (playerRb.velocity.y < 0)
        {
            // Makes the player character fall faster 
            playerRb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }
        else if (playerRb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            // Makes it possible for the player to tap jump
            playerRb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Makes sure player is on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Checks if player has fallen to death using collider as trigger
        if (collision.gameObject.CompareTag("Bottomless Pit"))
        {
            GameStateManager.GameOver();
            Debug.Log("Game Over");
        }

        if (collision.gameObject.CompareTag("Door"))
        {
            GameStateManager.NewGame();
            Debug.Log("You win!");
        }
    }

    public void ToggleHide(){
        if (playerHide == false && isOnGround){
            playerHide = true;
            playerRb.velocity = new Vector2(0, 0);
            playerRb.gravityScale = 0;
            playerCc.isTrigger = true;
            moveSpeed = 0;
            jumpForce = 0;
        }else{
            playerHide = false;
            playerRb.gravityScale = 1;
            playerCc.isTrigger = false;
            moveSpeed = oldMoveSpeed;
            jumpForce = oldJumpForce;
        }
    }
}
