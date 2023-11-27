using System.Collections;
using UnityEngine;

public class ObjectBounceLeft : MonoBehaviour
{
    private Rigidbody2D rb;
    private int bounceCount = 0;
    private int maxBounces = 3;

    [SerializeField] private float bounceSpeed = 25f;
    [SerializeField] private float bounceDistance = 6f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            BounceToLeft();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private void BounceToLeft()
    {
        if (rb != null)
        {
            rb.velocity = new Vector2(-bounceDistance, rb.velocity.y);
            bounceCount++;

            if (bounceCount >= maxBounces)
            {
                Destroy(gameObject);
            }
        }
    }
}