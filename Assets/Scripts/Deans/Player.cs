using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] private float speed;
    private Vector2 _moveDirection;

    public float crouch = 5f;
    public float jumpForce = 5f;
    public float sprintSpeedMultiplier = 10f;
    public float pushForce = 5f;
    public float maxHealth = 100f;

    public float inhaleRadius = 2f;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float inhaleSpeed = 5f;

    private GameObject inhaledObject;

    private float currentHealth;
    private bool isTakingDamage = false;
    private bool isBlocking = false;
    private float damageReductionMultiplier = 0.5f;

    private bool isInhaling = false;

    [SerializeField] private Image healthBar;
    
    public static bool isRunning;

    bool isGrounded;
    bool isCrouch;
    bool hasDoubleJumped;


    // Start is called before the first frame update
    void Start()
    {
        InputManager.Init(this);
        InputManager.SetGameControls();
        rb = GetComponent<Rigidbody2D>();
        _moveDirection = Vector2.zero;

        //Get the starting health
        currentHealth = maxHealth;

        //updates the hp bar UI
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _moveDirection, 0.1f, LayerMask.GetMask("Walls"));

        // If a wall is detected, adjust the movement
        if (hit.collider != null)
        {
            _moveDirection = Vector2.zero; // Stop movement
        }

        // Check if Shift key is pressed to increase speed
        float currentSpeed = isRunning ? speed * sprintSpeedMultiplier : speed;

        rb.velocity = new Vector2(currentSpeed * _moveDirection.x, rb.velocity.y);

        isGrounded = Physics2D.Raycast(transform.position, -Vector2.up, GetComponent<Collider2D>().bounds.extents.y + 0.1f);

        // Reset double jump flag when grounded
        if (isGrounded)
        {
            hasDoubleJumped = false;
        }
    }

    public void SetMovementDirection(Vector2 currentDirection)
    {
        _moveDirection = currentDirection;
    }

    public void PlayerJump()
    {
        if (isGrounded)
        {
            // Regular jump
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            Debug.Log("jump");
        }
        else if (!hasDoubleJumped)
        {
            // Double jump with reduced effectiveness
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.5f);
            hasDoubleJumped = true;
            Debug.Log("double jump");
        }
    }

    public void PlayerCrouch()
    {
        if (!isCrouch)
        {
            // Reduce the height of the upper and lower halves of kirby
            transform.localScale = new Vector3(1f, 0.5f, 1f);
            Debug.Log("Crouching");

            isCrouch = true;
            Debug.Log("Crouch");
        }
        else
        {
            // Restore the original height of the upper and lower halves of kirby
            transform.localScale = new Vector3(1f, 1f, 1f);
            Debug.Log("Standing");

            isCrouch = false;
            Debug.Log("Standing");

            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.size = new Vector2(collider.size.x, collider.size.y * 2f);
            }

            isCrouch = false;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isTakingDamage || isBlocking)
        {
            // If taking damage or blocking, reduce the damage
            damage *= damageReductionMultiplier;
        }

        // Disable movement during damage
        isTakingDamage = true;

        // Subtract damage from current health
        currentHealth -= damage;

        // Ensure health doesn't go below zero
        currentHealth = Mathf.Max(currentHealth, 0f);

        //pushes the player a little bit when getting hurt
        rb.velocity = new Vector2(-Mathf.Sign(_moveDirection.x) * pushForce, rb.velocity.y);

        // Update health bar UI
        UpdateHealthBar();

        Debug.Log($"Player took {damage} damage. Remaining health: {currentHealth}");

        // Check if the player is dead
        if (currentHealth <= 0f)
        {
            Die();
        }

        // Enable movement after a short delay
        StartCoroutine(EnableMovementAfterDelay(1.0f));
    }

    // enable movement after a delay (being hurt)
    private IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reset movement
        isTakingDamage = false;
        _moveDirection = Vector2.zero;
    }

    public void ToggleBlock()
    {
        isBlocking = !isBlocking;

        if (isBlocking)
        {
            Debug.Log("Blocking enabled");
        }
        else
        {
            Debug.Log("Blocking disabled");
        }
    }

    public void StopBlocking()
    {
        isBlocking = false;
        Debug.Log("Blocking stopped");
    }

    private void UpdateHealthBar()
    {

    }

    private void Die()
    {

    }

    public void Inhale()
    {
        // Check if there's an inhaled object
        if (!isInhaling && inhaledObject == null)
        {
            // Detect nearby objects (you might want to use physics layers for better filtering)
            Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, inhaleRadius);

            // Choose the first object to inhale
            foreach (Collider2D objCollider in nearbyObjects)
            {
                if (objCollider.CompareTag("Inhalable"))
                {
                    inhaledObject = objCollider.gameObject;
                    Rigidbody2D inhaledRb = inhaledObject.GetComponent<Rigidbody2D>();

                    Debug.Log("Inhaled object: " + inhaledObject.name);

                    inhaledRb.isKinematic = true;

                    Destroy(inhaledObject);

                    break;
                }
            }

            isInhaling = true;
            Debug.Log("Inhaling");
        }
    }
    public void StopInhaling()
    {
        isInhaling = false;
        Debug.Log("Stopped Inhaling");
    }

    public void SpitOut()
    {
        // Spit out the inhaled object
        if (isInhaling && inhaledObject != null)
        {
            Rigidbody2D rb = inhaledObject.GetComponent<Rigidbody2D>();
            rb.isKinematic = false;
            rb.velocity = transform.right * inhaleSpeed;

            // Destroy the inhaled object
            Destroy(inhaledObject);

            inhaledObject = null;

            isInhaling = false;
            Debug.Log("Spit Out and Destroyed");
        }
    }
}
