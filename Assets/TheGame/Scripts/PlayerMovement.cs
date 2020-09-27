using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    private Rigidbody2D m_Rigidbody2D;
    float velocity = 0f;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    private void Start() {
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

    }
    void Update()
    {
        velocity =  m_Rigidbody2D.velocity.y;

        Debug.Log("Velocity=" + velocity);
        animator.SetFloat("isFalling", velocity);
        
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("isJumping", true);
        }
        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("isCrouching", isCrouching);
    }
    void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch ,jump);
        jump = false;
    }
}
