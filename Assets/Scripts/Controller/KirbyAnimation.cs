using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyAnimation : MonoBehaviour
{
    public LayerMask groundLayer;
    
    public Animator animator;

    void Update()
    {
        jumpAni();
        movingAni();

        bool isCrouch = Input.GetButton("Crouch");
        animator.SetBool("IsCrouching", isCrouch);
    }

    private bool IsGrounded()
    {
        Vector2 raycastOrigin = transform.position - new Vector3(0f,1f,0f);

        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, 0.2f, groundLayer);

        Debug.DrawRay(raycastOrigin,Vector2.down * 0.2f, Color.red);

        return hit.collider != null;
    }

    void movingAni()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }

    void jumpAni()
    {
        if (IsGrounded())
        {
            animator.SetBool("IsJumping", false);
            Debug.Log("ON GROUND");
        }
        else if(!IsGrounded())
        {
            animator.SetBool("IsJumping", true);
        }
    }
}
