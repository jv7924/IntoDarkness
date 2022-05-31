using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    private Rigidbody2D monsterRb;
    private AudioSource monsterSound;
    private bool monsterSoundBool = false;
    private int monDirection = 1;
    private bool seePlayer;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float seePlayerMoveSpeed;
    [SerializeField]
    private float range;
    [SerializeField]
    private Transform playerPos;
    [SerializeField]
    private LayerMask playerSeeLayer;
    [SerializeField]
    private Transform platformCheckPos;
    [SerializeField]
    private LayerMask platformLayer;
    [SerializeField]
    private Collider2D bodyCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        monsterRb = GetComponent<Rigidbody2D>();
        monsterSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        monsterRb.velocity = new Vector2(moveSpeed * monDirection, monsterRb.velocity.y);
        Vector2 heading = playerPos.position - transform.position;
        Vector2 raycastDirection = heading / heading.magnitude;
        if (Mathf.Round(raycastDirection.x) == monDirection) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, heading.magnitude, playerSeeLayer);
            if (hit && heading.magnitude <= range && hit.transform.name == "Player" && (!PlayerMovement.playerHide || seePlayer)){
                monsterRb.velocity = new Vector2(seePlayerMoveSpeed * monDirection, monsterRb.velocity.y);
                seePlayer = true;
                if (!monsterSoundBool && !monsterSound.isPlaying){
                    monsterSound.Play();
                    monsterSoundBool = true;
                }
            }else{
                seePlayer = false;
                monsterSoundBool = false;
            }
        }else{
            seePlayer = false;
            monsterSoundBool = false;
        }
        if (!Physics2D.OverlapCircle(platformCheckPos.position, 0.1f, platformLayer) || bodyCollider.IsTouchingLayers(platformLayer)){
            Flip();
            monsterRb.velocity = new Vector2(0, monsterRb.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Makes sure player is on the ground
        if (collision.gameObject.CompareTag("Player"))
        {
            GameStateManager.GameOver();
            Debug.Log("GAME OVER");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Checks if player has fallen to death using collider as trigger
        if (collision.gameObject.CompareTag("Player") && seePlayer)
        {
            GameStateManager.GameOver();
            Debug.Log("Game Over");
        }
    }

    void Flip(){
        if (!seePlayer){
            monDirection = -monDirection;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }
}
