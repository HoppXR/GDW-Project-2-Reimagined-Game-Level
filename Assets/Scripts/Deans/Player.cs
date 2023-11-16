using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] private float speed;
    private Vector2 _moveDirection;

    public float crouch = 5f;
    public float jumpForce = 5f;
    public float sprintSpeedMultiplier = 10f;

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
            
            // Reduce the height of the player model
            transform.localScale = new Vector3(1f, 0.5f, 1f);
            Debug.Log("Crouching");
            // Adjust the collider size if you're using a BoxCollider2D
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.size = new Vector2(collider.size.x, collider.size.y * 1f);
            }

            isCrouch = true;
            Debug.Log("Crouch");
        }
        else
        {
            // Restore the original height of the player model
            transform.localScale = new Vector3(1f, 1f, 1f);
            Debug.Log("Standing");

            // Restore the original collider size if you're using a BoxCollider2D
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.size = new Vector2(collider.size.x, collider.size.y * 2f);
            }

            isCrouch = false;
        }
    }
}
