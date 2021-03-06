﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public PhysicsMaterial2D slide;
    public PhysicsMaterial2D notSlide;
    public BoxCollider2D bc;


    public float fallMultiplier = 3.0f;
    public float lowJumpMultiplier = 5f;


    public bool isGrounded = true;
    public bool isMoving = false;

    public float movementSpeed;
    public float sprintSpeed;
    public float jumpSpeed;
    float defaultMovementSpeed;

    public Animator playerAnimator;
    public RuntimeAnimatorController idle;
    public RuntimeAnimatorController run;
    public RuntimeAnimatorController jump;
    


	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = gameObject.GetComponent<BoxCollider2D>();
        playerAnimator = gameObject.GetComponent<Animator>();
        playerAnimator.runtimeAnimatorController = idle;
        defaultMovementSpeed = movementSpeed;


    }

    // Update is called once per frame
    void Update ()
    {
        Movement();
        Jump();
        Sprint();

        if(rb.velocity.x != 0 || rb.velocity.y != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

    }

    void Movement()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            rb.velocity = new Vector2(movementSpeed * Time.fixedDeltaTime * 10.0f, rb.velocity.y);
            playerAnimator.runtimeAnimatorController = run;
            transform.rotation = new Quaternion(transform.rotation.x, 0.0f, transform.rotation.z, transform.rotation.w);


        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            rb.velocity = new Vector2(-movementSpeed * Time.fixedDeltaTime * 10.0f, rb.velocity.y);
            transform.rotation = new Quaternion(transform.rotation.x, -180.0f, transform.rotation.z, transform.rotation.w);
            playerAnimator.runtimeAnimatorController = run;

        }
        else if(GetComponent<PlayerRewindScript>().isRewinding == false)
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
            playerAnimator.runtimeAnimatorController = idle;

        }

    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") & isGrounded)
        {
            isGrounded = false;
            rb.velocity = Vector2.up * jumpSpeed * Time.fixedDeltaTime * 50.0f;
        }

        if(!isGrounded)
        {
            playerAnimator.runtimeAnimatorController = jump;

        }


        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton ("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

    }

    void Sprint()
    {
        


        if (Input.GetKey(KeyCode.LeftShift))
        {

            if(movementSpeed < sprintSpeed)
            {
                movementSpeed += 0.5f;
            }
        }
        else
        {
            if(movementSpeed > defaultMovementSpeed)
            {
                movementSpeed -= 0.5f;

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;

        }
    }
}
