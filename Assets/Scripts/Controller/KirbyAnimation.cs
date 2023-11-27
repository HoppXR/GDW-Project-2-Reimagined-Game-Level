using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyAnimation : MonoBehaviour
{
    public Animator animator;
    private bool isGrounded;

    void Update()
    {
        CheckGrounded();

        float horizontalMove = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump") && !isGrounded)
        {
            animator.SetBool("IsJumping", true);
        }

        if(isGrounded)
        {
            animator.SetBool("IsJumping", false);
            Debug.Log("ON GROUND");
        }

        bool isCrouch = Input.GetButton("Crouch");
        animator.SetBool("IsCrouching", isCrouch);
    }

    void CheckGrounded()
    {
        
    }
}
