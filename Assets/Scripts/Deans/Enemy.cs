using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float damageAmount = 10f;
    public float maxHealth = 50f;
    private float currentHealth;


    void Start()
    {
        currentHealth = maxHealth;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);

                Debug.Log($"Enemy took {damageAmount} damage. Remaining health: {currentHealth}");
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log($"Enemy took {damage} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy defeated!");
        // need to add animation
        Destroy(gameObject);
    }
}
