using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    private Rigidbody2D monsterRb;
    private int direction = 1;
    private bool seePlayer;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float seePlayerMoveSpeed;
    [SerializeField]
    private Transform playerPos;
    [SerializeField]
    private float range;
    
    // Start is called before the first frame update
    void Start()
    {
        monsterRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        monsterRb.velocity = new Vector2(moveSpeed * direction, monsterRb.velocity.y);
        Vector2 heading = playerPos.position - transform.position;
        Vector2 raycastDirection = heading / heading.magnitude;
        if (Mathf.Round(raycastDirection.x) == direction) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, heading.magnitude);
            if ((!PlayerMovement.playerHide || seePlayer) && hit){
                Debug.DrawRay(transform.position, raycastDirection * heading.magnitude, Color.red);
                monsterRb.velocity = new Vector2(seePlayerMoveSpeed * direction, monsterRb.velocity.y);
                seePlayer = true;
            }else{
                seePlayer = false;
            }
        }else{
            seePlayer = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Makes sure player is on the ground
        if (collision.gameObject.CompareTag("Player"))
        {
            GameStateManager.GameOver();
            Debug.Log("GAME OVER");
        }else if (collision.gameObject.CompareTag("Ground")){
            Flip();
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
        direction *= -1;
        transform.localScale = new Vector2(transform.localScale.x * direction, transform.localScale.y);
    }
}
