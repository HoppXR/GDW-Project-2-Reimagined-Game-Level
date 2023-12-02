using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] private float speed;
    private Vector2 _moveDirection;

    public float crouch = 5f;
    public float jumpForce = 5f;
    public float sprintSpeedMultiplier = 10f;
    public float pushForce = 5f;
    public float maxHealth = 100f;

    public float inhaleRadius = 2f;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float inhaleSpeed = 5f;
    public float spitOutSpeed = 10f;
    private float invulnerabilityDuration = 1.0f;

    private GameObject inhaledObject;

    private bool isMovementEnabled = true;
    private float currentHealth;
    private bool isTakingDamage = false;
    private bool isBlocking = false;
    private float damageReductionMultiplier = 0.5f;

    private bool isInhaling = false;
    private bool hasInhaled = false;
    private bool isDying = false;


    [SerializeField] private Image healthBar;

    public static bool isRunning;

    bool isGrounded;
    bool isCrouch;
    bool hasDoubleJumped;


    private AudioClip lastPlayedHurtSound;

    private SpriteRenderer spriteRenderer;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip jumpSoundClip;
    [SerializeField] private AudioClip inhaleSoundClip;
    [SerializeField] private AudioClip[] hurtSounds;
    [SerializeField] private AudioClip exhaleSoundClip;
    [SerializeField] private AudioClip crouchSoundClip;
    [SerializeField] private AudioClip deathSoundClip;

    private AudioSource inhaleAudioSource;
    private AudioSource deathAudioSource;


    void Start()
    {
        InputManager.Init(this);
        InputManager.SetGameControls();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _moveDirection = Vector2.zero;

        currentHealth = maxHealth;

        inhaleAudioSource = gameObject.AddComponent<AudioSource>();
        inhaleAudioSource.clip = inhaleSoundClip;
        inhaleAudioSource.loop = true; 
        inhaleAudioSource.playOnAwake = false;

        deathAudioSource = gameObject.AddComponent<AudioSource>();
        deathAudioSource.clip = deathSoundClip;
        deathAudioSource.playOnAwake = false;

        UpdateHealthBar();
    }

    private void FixedUpdate()
    {
        spriteRenderer.flipX = rb.velocity.x < 0f;
    }

    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));

        if (isMovementEnabled)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _moveDirection, 0.1f, LayerMask.GetMask("Walls"));

            if (hit.collider != null)
            {
                _moveDirection = Vector2.zero;
            }

            float currentSpeed = isRunning ? speed * sprintSpeedMultiplier : speed;

            rb.velocity = new Vector2(currentSpeed * _moveDirection.x, rb.velocity.y);

            if (isGrounded)
            {
                hasDoubleJumped = false;
            }
        }
    }

    public void TogglePlayerInput(bool enable)
    {
        isMovementEnabled = enable;
    }

    public void SetMovementDirection(Vector2 currentDirection)
    {
        _moveDirection = currentDirection;
    }

    public void PlayerJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        Debug.Log("jump");

        if (!hasDoubleJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.5f);
            hasDoubleJumped = true;
            Debug.Log("double jump");
        }

        if (jumpSoundClip != null) AudioSource.PlayClipAtPoint(jumpSoundClip, transform.position);
    }

    public void PlayerCrouch()
    {
        if (!isCrouch)
        {
            Debug.Log("Crouching");

            isCrouch = true;
            Debug.Log("Crouch");
        }
        else
        {
            Debug.Log("Standing");

            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.size = new Vector2(collider.size.x, collider.size.y * 2f);
            }

            isCrouch = false;
        }

        if (crouchSoundClip != null) AudioSource.PlayClipAtPoint(crouchSoundClip, transform.position);
    }

    public void TakeDamage(float damage)
    {
        if (isTakingDamage || isBlocking)
        {
            damage *= damageReductionMultiplier;
        }

        isTakingDamage = true;

        currentHealth -= damage;

        currentHealth = Mathf.Max(currentHealth, 0f);

        rb.velocity = new Vector2(-Mathf.Sign(_moveDirection.x) * pushForce, rb.velocity.y);

        PerformJump(jumpForce);

        UpdateHealthBar();

        Debug.Log($"Player took {damage} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }

        if (hurtSounds.Length > 0)
        {
            AudioClip randomHurtSound = GetRandomUniqueHurtSound();
            if (randomHurtSound != null) AudioSource.PlayClipAtPoint(randomHurtSound, transform.position);
        }
    }

    private AudioClip GetRandomUniqueHurtSound()
    {
        if (hurtSounds.Length == 1)
        {
            return hurtSounds[0];
        }

        AudioClip randomHurtSound;
        do
        {
            int randomIndex = Random.Range(0, hurtSounds.Length);
            randomHurtSound = hurtSounds[randomIndex];
        } while (randomHurtSound == lastPlayedHurtSound);

        lastPlayedHurtSound = randomHurtSound;

        return randomHurtSound;
    }

    private IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        isTakingDamage = false;
        _moveDirection = Vector2.zero;
    }

    public void PerformJump(float jumpForce)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        Debug.Log("Jump");
    }

    public void ToggleBlock()
    {
        isBlocking = !isBlocking;

        if (isBlocking)
        {
            Debug.Log("Blocking enabled");
        }
        else
        {
            Debug.Log("Blocking disabled");
        }
    }

    public void StopBlocking()
    {
        isBlocking = false;
        Debug.Log("Blocking stopped");
    }

    private void UpdateHealthBar()
    {

    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    private void Die()
    {
        if (deathSoundClip != null)
        {
            deathAudioSource.Play();
        }

        Debug.Log("Die method called");

        SceneManager.LoadScene("DeathScreen");

        StartCoroutine(LoadTitleScreenAfterDelay());
    }

    private IEnumerator LoadTitleScreenAfterDelay()
    {
        Debug.Log("LoadTitleScreenAfterDelay coroutine started");

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("ContinueScreen");
    }

    public void Inhale()
    {
        if (!isInhaling)
        {
            isMovementEnabled = false;
            rb.velocity = Vector2.zero;

            if (inhaleAudioSource != null)
            {
                inhaleAudioSource.Play();
            }

            Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, inhaleRadius);

            foreach (Collider2D objCollider in nearbyObjects)
            {
                if (objCollider.CompareTag("Inhalable"))
                {
                    inhaledObject = objCollider.gameObject;
                    Rigidbody2D inhaledRb = inhaledObject.GetComponent<Rigidbody2D>();

                    Debug.Log("Inhaled object: " + inhaledObject.name);

                    inhaledObject.GetComponent<Renderer>().enabled = false;

                    inhaledRb.isKinematic = true;

                    hasInhaled = true;

                    break;
                }
            }

            isInhaling = true;
            Debug.Log("Inhaling");
        }
    }

    public void StopInhaling()
    {
        isInhaling = false;
        isMovementEnabled = true;

        if (inhaleAudioSource != null)
        {
            inhaleAudioSource.Stop();
        }

        Debug.Log("Stopped Inhaling");
    }

    public void SpitOut()
    {
        Debug.Log("SpitOut method called");

        if (inhaledObject != null && hasInhaled)
        {
            if (exhaleSoundClip != null) AudioSource.PlayClipAtPoint(exhaleSoundClip, transform.position);

            GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

            Rigidbody2D projectileRb = newProjectile.GetComponent<Rigidbody2D>();

            SpriteRenderer spriteRenderer = newProjectile.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
            }

            Vector2 spitDirection = transform.right * (transform.localScale.x > 0 ? 1 : -1);

            projectileRb.velocity = spitDirection * spitOutSpeed;

            Destroy(inhaledObject);

            inhaledObject = null;

            Debug.Log("Spit out an object");

            hasInhaled = false;

            isMovementEnabled = true;
        }
    }
}
