using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    KirbyAnimation kirbyAnimation;
    
    Rigidbody2D rb;

    [SerializeField] private float speed;
    private Vector2 _moveDirection;
    public float horizontalMove;

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
    private bool canDamage = true;
    private bool isBlocking = false;
    private float damageReductionMultiplier = 0.5f;

    private bool isInhaling = false;
    private bool hasInhaled = false;
    private bool isDying = false;
    private bool isJumping = false;

    [SerializeField] private Image healthBar;

    public static bool isRunning;

    bool isGrounded;
    bool isCrouch;
    bool hasDoubleJumped;

    public AudioSource audioPlayer;

    public AudioSource trunkSound;

    private AudioClip lastPlayedHurtSound;

    private SpriteRenderer spriteRenderer;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip jumpSoundClip;
    [SerializeField] private AudioClip inhaleSoundClip;
    [SerializeField] private AudioClip[] hurtSounds;
    [SerializeField] private AudioClip exhaleSoundClip;
    [SerializeField] private AudioClip crouchSoundClip;
    [SerializeField] private AudioClip deathSoundClip;
    [SerializeField] private AudioClip blockSoundClip;
    [SerializeField] private AudioClip moveSoundClip;
    [SerializeField] private AudioClip sprintSoundClip;



    private AudioSource inhaleAudioSource;
    private AudioSource deathAudioSource;


    void Start()
    {
        InputManager.Init(this);
        InputManager.SetGameControls();
        rb = GetComponent<Rigidbody2D>();
        kirbyAnimation = GetComponent<KirbyAnimation>();
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

    }

    private void FixedUpdate()
    {
        spriteRenderer.flipX = rb.velocity.x < 0f;
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
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

        if (_moveDirection.x != 0f && isGrounded)
        {
            PlayMoveSound();
        }
        else
        {
            StopMoveSound();
        }

        if (isRunning && isGrounded)
        {
            PlaySprintSound();
        }
        else
        {
            StopSprintSound();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            audioPlayer.Play();
        }
    }

    public void TrunkSound(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trunk")
        {
            audioPlayer.Play();
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
        if (!isJumping)
        {
            isJumping = true;

            if (!hasDoubleJumped)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.5f);
                hasDoubleJumped = true;
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            if (jumpSoundClip != null)
            {
                AudioSource.PlayClipAtPoint(jumpSoundClip, transform.position);
            }

            StartCoroutine(ResetJumpFlag());
        }
    }

    private IEnumerator ResetJumpFlag()
    {
        yield return new WaitForSeconds(0.1f);
        isJumping = false;
    }

    private IEnumerator DamageAnimation()
    {
        yield return new WaitForSeconds(0.75f);
        kirbyAnimation.Recovery();
    }

    private IEnumerator SpittingAnimation()
    {
        yield return new WaitForSeconds(0.33f);
        kirbyAnimation.EndSpitting();
    }

    public void PlayerCrouch()
    {
        if (!isCrouch)
        {

            isCrouch = true;
        }
        else
        {
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
        kirbyAnimation.TakeDamage();

        isTakingDamage = true;

        float blockDamage;
        
        if (canDamage)
        {
            if (!isBlocking && isTakingDamage)
            {
                currentHealth -= damage;
            
                //rb.velocity = new Vector2(-Mathf.Sign(_moveDirection.x) * pushForce, rb.velocity.y);
                //PerformJump(jumpForce);
            }
            else if (isBlocking && isTakingDamage)
            {
                blockDamage = damage * damageReductionMultiplier;

                currentHealth -= blockDamage;
            }

            kirbyAnimation.Damaged();
            kirbyAnimation.BigDamaged();

            StartCoroutine(DamageAnimation());
            StartCoroutine(EnableMovementAfterDelay());

            currentHealth = Mathf.Max(currentHealth, 0f);

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

    private IEnumerator EnableMovementAfterDelay()
    {
        canDamage = false;

        isTakingDamage = false;
        _moveDirection = Vector2.zero;

        yield return new WaitForSeconds(0.75f);

        canDamage = true;
    }

    public void PerformJump(float jumpForce)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void ToggleBlock()
    {
        isBlocking = true;

        if (isBlocking)
        {
            if (blockSoundClip != null) AudioSource.PlayClipAtPoint(blockSoundClip, transform.position);
        }
    }


    public void StopBlocking()
    {
        isBlocking = false;
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


        SceneManager.LoadScene("DeathScreen");

        StartCoroutine(LoadTitleScreenAfterDelay());
    }

    private IEnumerator LoadTitleScreenAfterDelay()
    {

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

                    inhaledObject.GetComponent<Renderer>().enabled = false;

                    inhaledRb.isKinematic = true;

                    hasInhaled = true;

                    kirbyAnimation.Inhaled();
                }
            }

            isInhaling = true;
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
    }

    public void SpitOut()
    {
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

            hasInhaled = false;

            isMovementEnabled = true;

            kirbyAnimation.Spitting();
            StartCoroutine(SpittingAnimation());
        }
    }

    private void PlayMoveSound()
    {
        if (!audioPlayer.isPlaying && audioPlayer.clip != moveSoundClip)
        {
            audioPlayer.Stop();
            audioPlayer.clip = moveSoundClip;
            audioPlayer.Play();
        }
    }

    private void StopMoveSound()
    {
        if (audioPlayer.clip == moveSoundClip)
        {
            audioPlayer.Stop();
            audioPlayer.clip = null;
        }
    }

    private void PlaySprintSound()
    {
        if (!audioPlayer.isPlaying && audioPlayer.clip != sprintSoundClip)
        {
            audioPlayer.Stop();
            audioPlayer.clip = sprintSoundClip;
            audioPlayer.Play();
        }
    }

    private void StopSprintSound()
    {
        if (audioPlayer.clip == sprintSoundClip)
        {
            audioPlayer.Stop();
            audioPlayer.clip = null;
        }
    }
}