﻿using System.Collections;
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
    private PlayerPossession playerPossession;

    // Use this for initialization
	void Start()
	{
        rb2d = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        if (GameObject.FindGameObjectsWithTag("Player").Length > 1) {
            Destroy(gameObject);
        }
        playerPossession = GetComponent<PlayerPossession>();
	}

    void FixedUpdate() 
    {
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
        else if (timeDownX < timeDownY) {
            movement = Vector2.up * movement.y;
        }
        // Debug.Log(movement);
        UpdateAnimationAndMove();
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

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            SaySmt.Line("", "GAME OVER!", true);
            playerPossession.reset();
        }
    }

    public bool IsStill()
    {
        return movement.x == 0 && movement.y == 0;
    }
}
