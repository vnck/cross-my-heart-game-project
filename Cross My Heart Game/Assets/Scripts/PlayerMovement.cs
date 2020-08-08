using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb2d;

    private Vector2 movement;
    private float timeDownX = 0.0f; //time which A/D was pressed
    private float timeDownY= 0.0f;  //time which W/S was pressed
    private Animator playerAnimator;
    private AudioSource[] gameOverSFX;
    private PlayerPossession playerPossession;
    private Camera main;

    public bool isDead = false;

    // Use this for initialization
	void Start()
	{
        rb2d = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        if (GameObject.FindGameObjectsWithTag("Player").Length > 1) {
            Destroy(gameObject);
        }
        playerPossession = GetComponent<PlayerPossession>();
        gameOverSFX = GetComponents<AudioSource>();
        main = Camera.main;
	}

    void FixedUpdate() 
    {
        if (!isDead) {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            
            if (playerPossession.isPossessing || playerPossession.isDepossessing)
            {
                movement.x = 0;
                movement.y = 0;
            }
            
            //if A/D pressed, save time it was pressed
            if (movement.x != 0) {
                if (timeDownX == 0.0f) //if we don't have a stored time for it
                timeDownX = Time.time;
            }
            else {
                timeDownX = 0.0f; //reset time if no button is being pressed
            }
            
            //if W/S pressed, save time it was pressed
            if (movement.y != 0) {
                if (timeDownY == 0.0f) //if we don't have a stored time for it
                timeDownY = Time.time;
            } 
            else {
                timeDownY = 0.0f; //reset time if no button is being pressed
            }
            
            //check which button was hit last to determine direction
            if (timeDownX > timeDownY) {
                movement = Vector2.right * movement.x;
            }
            else {
                movement = Vector2.up * movement.y;
            }
            // Debug.Log(movement);
            UpdateAnimationAndMove();
        }
    }

    void UpdateAnimationAndMove()
    {
        // nest it to ensure that the direction the character faces remains even without pressing any keys
        if (movement != Vector2.zero){
            rb2d.MovePosition(rb2d.position + movement * Time.deltaTime * speed);
            
            playerAnimator.SetFloat("moveX", movement.x);
            playerAnimator.SetFloat("moveY", movement.y);
            playerAnimator.SetBool("moving", true);
        }
        else {
            playerAnimator.SetBool("moving", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Enemy") && !isDead)
        {
            bool chasing = other.gameObject.GetComponent<EnemyMovementLoop>().isChasing();
            if (IsStill() && playerPossession.isPossessed && !chasing) {return;}
            isDead = true;
            playerAnimator.SetBool("moving", false);
            playerAnimator.SetBool("isDead", true);
            main.GetComponents<AudioSource>()[0].enabled = false;
            main.GetComponents<AudioSource>()[1].enabled = false;
            StartCoroutine(waitForDeathAnim());
        }
    }

    public bool IsStill()
    {
        return movement.x == 0 && movement.y == 0;
    }

    IEnumerator waitForDeathAnim() {
        yield return new WaitForSeconds(0.8f);
        gameOverSFX[1].Play(0);
        SaySmt.Line("", "GAME OVER!", true);
        // This is to enable the Coffin Dance music if we want 
        // main.GetComponents<AudioSource>()[2].enabled = true;
    }
}
