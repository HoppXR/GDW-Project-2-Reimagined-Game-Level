using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 500f; // Adjust the rotation speed as needed
    public float damageAmount = 10f;

    private Vector2 direction;

    //set the direction of the projectile
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized; // Ensure the direction is normalized
    }

    void Update()
    {
        // Move the projectile in the set direction
        transform.Translate(direction * speed * Time.deltaTime);

        // Rotate the projectile clockwise
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Deal damage to the enemy
                enemy.TakeDamage(damageAmount);

                Destroy(gameObject);
            }
        }
    }
}
