using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class KirbyAnimation : MonoBehaviour
{
    public LayerMask groundLayer;
    
    public Animator animator;

    private float horizontalMove;

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");

        InhaleAni();
        BlockAni();

        MovingAni();
        JumpAni();
        CrouchAni();
    }

    private bool IsGrounded()
    {
        Vector2 raycastOrigin = transform.position - new Vector3(0f,1f,0f);

        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, 0.2f, groundLayer);

        return hit.collider != null;
    }

    void MovingAni()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetBool("IsSprinting", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("IsSprinting", false);
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }

    void JumpAni()
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

    void CrouchAni()
    {
        if (IsGrounded() && Input.GetKeyDown(KeyCode.C) && horizontalMove == 0)
        {
            animator.SetBool("IsCrouching", true);
        }
        else if(Input.GetKeyUp(KeyCode.C))
        {
            animator.SetBool("IsCrouching", false);
        }
    }

    void BlockAni()
    {  
        if (IsGrounded() && Input.GetMouseButtonDown(2) && horizontalMove == 0)
        {
            animator.SetBool("IsBlocking", true);
        }
        else if(Input.GetMouseButtonUp(2))
        {
            animator.SetBool("IsBlocking", false);
        }
    }
    void InhaleAni()
    {
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("IsInhaling", true);

            Invoke("Inhaling", 0.5f);
        }
        else if(Input.GetMouseButtonUp(1))
        {
            animator.SetBool("IsInhaling", false);
            animator.SetBool("Inhaling", false);
        }
    }

    void Inhaling()
    {
        animator.SetBool("Inhaling", true);
    }

    public void Inhaled()
    {
        animator.SetBool("Inhaled", true);
    }

    public void Spitting()
    {
        animator.SetBool("IsSpitting", true);
        
        animator.SetBool("Inhaled", false);
    }

    public void EndSpitting()
    {
        animator.SetBool("IsSpitting", false);
    }

    public void Damaged()
    {
        animator.SetTrigger("Damaged");
    }
    
    public void BigDamaged()
    {
        animator.SetTrigger("BigDamaged");
    }

    public void TakeDamage()
    {
        animator.SetBool("IsDamaged", true);
    }

    public void Recovery()
    {
        animator.SetBool("IsDamaged", false);
    }
}
