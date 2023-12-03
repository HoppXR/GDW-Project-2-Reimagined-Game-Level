using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Phase2Enemy : MonoBehaviour
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
    public AudioClip damageSound;

    private static int defeatedEnemiesCount = 0;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (defeatedEnemiesCount >= 2)
        {
            SceneManager.LoadScene("Phase2Cutscene");
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

                if (damageSound != null)
                {
                    AudioSource.PlayClipAtPoint(damageSound, transform.position);
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

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
        isDefeated = true;

        defeatedEnemiesCount++;

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

        if (defeatedEnemiesCount >= 2)
        {
            SceneManager.LoadScene("Phase2Cutscene");
        }
    }
}
