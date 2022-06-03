using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool canMove = true;
    private Rigidbody2D playerRb;
    private CapsuleCollider2D playerCapCol;
    private float horizontalInput;
    public static bool playerHide = false;
    private SpriteRenderer spriteR;
    private AudioSource jumpSound;
    private AudioSource stepSound;
    [SerializeField]
    private SpriteRenderer player;
    [SerializeField]
    private LayerMask platformLayerMask;

    public Animator animator;

    [SerializeField]
    private PhysicsMaterial2D noFriction;
    [SerializeField]
    private PhysicsMaterial2D fullFriction;

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
        playerCapCol = transform.GetComponent<CapsuleCollider2D>();
        stepSound = transform.GetChild(1).GetComponent<AudioSource>();
        jumpSound = transform.GetChild(2).GetComponent<AudioSource>();
        spriteR = transform.GetComponent<SpriteRenderer>();
        oldMoveSpeed = moveSpeed;
        oldJumpForce = jumpForce;
    }

    // Update is called once per frame
    void Update()
    {
        // Gets the horizontal input of the keyboard 1 if right -1 if left
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0 && canMove)
        {
            // Moves player to the right
            player.flipX = false;
            playerRb.velocity = new Vector2(moveSpeed, playerRb.velocity.y);
            animator.SetFloat("Speed", horizontalInput);
            playerRb.sharedMaterial = noFriction;

            if (!stepSound.isPlaying && IsGrounded())
            {
                stepSound.Play();
            }
        }
        else if (horizontalInput < 0 && canMove)
        {
            // Moves player to the left
            player.flipX = true;
            playerRb.velocity = new Vector2(-moveSpeed, playerRb.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
            playerRb.sharedMaterial = noFriction;

            if (!stepSound.isPlaying && IsGrounded())
            {
                stepSound.Play();
            }
        }
        else
        {
            // Makes player do an immediate stop when neither left or right is being pressed
            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
            animator.SetFloat("Speed", 0);

            // Prevents player from sliding down slopes when not moving
            playerRb.sharedMaterial = fullFriction;
        }
        //Step sound check
        if (horizontalInput == 0 || !IsGrounded())
        {
            stepSound.Stop();
        }

        // Code to make player jump
        // Checks if player in on ground before being able to jump again
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space) && canMove)
        {
            // Makes it so player can't jump while in the air.
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
            animator.SetBool("IsJumping", true);

            jumpSound.Play();
            /*isOnGround = false;*/
        }
        else if (IsGrounded() && !Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("IsJumping", false);
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

    private bool IsGrounded()
    {
        float extraHeightText = .075f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerCapCol.bounds.center, playerCapCol.bounds.size, 0f, Vector2.down, extraHeightText, platformLayerMask);
        
        // Draws the rays
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(playerCapCol.bounds.center + new Vector3(playerCapCol.bounds.extents.x, 0), Vector2.down * (playerCapCol.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(playerCapCol.bounds.center - new Vector3(playerCapCol.bounds.extents.x, 0), Vector2.down * (playerCapCol.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(playerCapCol.bounds.center - new Vector3(playerCapCol.bounds.extents.x, playerCapCol.bounds.extents.y + extraHeightText), Vector2.right * 2 * (playerCapCol.bounds.extents.x), rayColor);
        //

        return raycastHit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            GameStateManager.GameOver();
            Debug.Log("Dead");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Checks if player has fallen to death using collider as trigger
        if (collision.gameObject.CompareTag("Bottomless Pit"))
        {
            Debug.Log("Game Over");
            GameStateManager.GameOver();
        }

        if (collision.gameObject.CompareTag("Door"))
        {
            GameStateManager.NextLevel();
            GameStateManager.NewGame();
            Debug.Log("You win!");
        }
    }

    public void ToggleHide(){
        if (playerHide == false && IsGrounded())
        {
            playerHide = true;
            spriteR.sortingOrder = 1;
            playerRb.velocity = new Vector2(0, 0);
            playerRb.gravityScale = 0;
            playerCapCol.isTrigger = true;
            moveSpeed = 0;
            jumpForce = 0;
            spriteR.color = new Color(.2f, .2f, .2f);
            canMove = false;
        }else
        {
            playerHide = false;
            playerRb.gravityScale = 1;
            playerCapCol.isTrigger = false;
            moveSpeed = oldMoveSpeed;
            jumpForce = oldJumpForce;
            spriteR.sortingOrder = 3;
            spriteR.color = new Color(1, 1, 1);
            canMove = true;
        }
    }
}