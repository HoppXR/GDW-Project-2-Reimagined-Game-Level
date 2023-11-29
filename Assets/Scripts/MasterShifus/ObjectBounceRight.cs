using System.Collections;
using UnityEngine;

public class ObjectBounceRight : MonoBehaviour
{
    private Rigidbody2D rb;
    private int bounceCount = 0;
    private int maxBounces = 4;

    [SerializeField] private float bounceSpeed = 25f;
    [SerializeField] private float bounceDistance = 6f;
    [SerializeField] private float spinSpeed = 1000f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 7f);
    }

    private void Update()
    {
        if (rb != null && rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, -bounceSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            BounceToRight();
        }
    }

    private void BounceToRight()
    {
        if (rb != null)
        {
            rb.velocity = new Vector2(bounceDistance, rb.velocity.y);
            rb.angularVelocity = spinSpeed;
            bounceCount++;

            if (bounceCount >= maxBounces)
            {
                Destroy(gameObject);
            }
        }
    }
}