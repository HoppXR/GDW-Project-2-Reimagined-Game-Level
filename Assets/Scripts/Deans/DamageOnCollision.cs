using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float bounceForce = 5f; // Adjust the force to control the bounce
    [SerializeField] private float playerJumpForce = 5f; // Adjust the force to control the jump
    [SerializeField] private float playerInvulnerabilityTime = 1f; // Adjust the time the player is invulnerable

    private Rigidbody2D rb;
    private bool isPlayerInvulnerable = false;

    private void Start()
    {
        // Ensure there is a Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        if (isPlayerInvulnerable)
        {
            // Reduce the invulnerability timer
            playerInvulnerabilityTime -= Time.deltaTime;

            // Check if the invulnerability time has expired
            if (playerInvulnerabilityTime <= 0f)
            {
                isPlayerInvulnerable = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Air") && !isPlayerInvulnerable)
        {
            if (collision.collider.CompareTag("Player"))
            {
                Player player = collision.collider.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(damageAmount);
                    Debug.Log($"{gameObject.name} dealt {damageAmount} damage to the player.");

                    // Bounce off the player, push the player back, and make the player jump
                    BounceOffPlayer();
                    player.PerformJump(playerJumpForce);

                    // Set the player to be invulnerable for a certain time
                    isPlayerInvulnerable = true;
                    playerInvulnerabilityTime = 1f; // Adjust the time the player is invulnerable

                    // Destroy the projectile after bouncing
                    Destroy(gameObject, 0.2f); // Adjust the time to control how long it stays after bouncing
                }
            }
        }
    }

    private void BounceOffPlayer()
    {
        // Reflect the current velocity based on a random direction
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        Vector2 reflectedVelocity = Vector2.Reflect(rb.velocity, randomDirection.normalized);

        // Apply the reflected velocity with an additional force for bouncing effect
        rb.velocity = reflectedVelocity.normalized * (bounceForce + rb.velocity.magnitude);
    }
}
