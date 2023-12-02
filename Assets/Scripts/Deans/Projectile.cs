using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 500f; 
    public float damageAmount = 10f;

    private Vector2 direction;

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized; 
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);

                Destroy(gameObject);
            }
        }
    }
}
