using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float damageAmount = 10f;
    [SerializeField] public float maxHealth = 50f;
    public float currentHealth;
    public string enemyID;
    private bool isDefeated = false;

    private SpriteRenderer spriteRenderer;

    public Sprite defeatedSprite;

    public ParticleSystem defeatParticles;

    public AudioClip defeatSound;

    private static HashSet<string> remainingEnemies = new HashSet<string>();

    void Start()
    {
        currentHealth = maxHealth;

        remainingEnemies.Add(enemyID);

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (remainingEnemies.Count <= 0)
        {
            Debug.Log("Both enemies defeated!");
            SceneManager.LoadScene("Phase2Scene");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);

                Debug.Log($"Enemy {enemyID} took {damageAmount} damage. Remaining health: {currentHealth}");
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log($"Enemy {enemyID} took {damage} damage. Remaining health: {currentHealth}");

        if (isDefeated)
        {
            return;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    private void Die()
    {
        Debug.Log($"Enemy {enemyID} defeated!");

        isDefeated=true;

        if (defeatParticles != null)
        {
            Instantiate(defeatParticles, transform.position, Quaternion.identity);
        }

        if (defeatSound != null)
        {
            AudioSource.PlayClipAtPoint(defeatSound, transform.position);
        }

        if (defeatedSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = defeatedSprite;
        }

        remainingEnemies.Remove(enemyID);

        if (remainingEnemies.Count <= 0)
        {
            SceneManager.LoadScene("Phase2Scene");
        }
    }
}
