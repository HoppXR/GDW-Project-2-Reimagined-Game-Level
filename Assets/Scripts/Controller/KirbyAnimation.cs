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
        crouchAni();
        movingAni();
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
        }
        else if(!IsGrounded())
        {
            animator.SetBool("IsJumping", true);
        }
    }

    void crouchAni()
    {
        if (IsGrounded() && Input.GetKeyDown(KeyCode.C))
        {
            animator.SetBool("IsCrouching", true);
        }
        else if(Input.GetKeyUp(KeyCode.C))
        {
            animator.SetBool("IsCrouching", false);
        }
    }
}
