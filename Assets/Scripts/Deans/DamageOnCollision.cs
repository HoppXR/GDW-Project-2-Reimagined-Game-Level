using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float bounceForce = 5f;
    [SerializeField] private float playerJumpForce = 5f;
    [SerializeField] private float playerInvulnerabilityTime = 1f;

    private Rigidbody2D rb;
    private bool isPlayerInvulnerable = false;

    private void Start()
    {
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
            playerInvulnerabilityTime -= Time.deltaTime;

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

                    BounceOffPlayer();
                    player.PerformJump(playerJumpForce);

                    isPlayerInvulnerable = true;
                    playerInvulnerabilityTime = 1f;

                    if (gameObject.CompareTag("Trunk"))
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        Destroy(gameObject, 0.2f);
                    }
                }
            }
        }
        if (gameObject.CompareTag("Air"))
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                Destroy(gameObject);
            }
        }
    }

    private void BounceOffPlayer()
    {
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        Vector2 reflectedVelocity = Vector2.Reflect(rb.velocity, randomDirection.normalized);

        rb.velocity = reflectedVelocity.normalized * (bounceForce + rb.velocity.magnitude);
    }
}
