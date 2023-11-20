using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float damageAmount = 10f;
    [SerializeField] public float maxHealth = 50f;
    public float currentHealth;


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

    public float GetHealthPercentage()
    {
        // Return the health percentage (between 0 and 1)
        return currentHealth / maxHealth;
    }

    private void Die()
    {
        Debug.Log("Enemy defeated!");
        // need to add animation
        //Destroy(gameObject);
    }
}
