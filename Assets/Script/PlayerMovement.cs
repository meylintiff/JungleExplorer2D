using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private PlayerController playerController;

    private Vector2 moveInput;
    private float mobileInputX = 0f;
    private bool isJumping = false;

    private enum MovementState { idle, walk, run, jump, landing }

    [Header("Jump Settings")]
    [SerializeField] private LayerMask jumpableGround;
    private BoxCollider2D coll;

    [Header("Health System")]
    public int maxHealth = 100;
    private int currentHealth;
    public TextMeshProUGUI healthText;

    [Header("Knockback Settings")]
    [SerializeField] private float knockBackTime = 0.2f;
    [SerializeField] private float knockBackThrust = 10f;

    private bool isKnockedBack = false;

    private Vector3 startPosition;

    [Header("Sound Effects")]
    public AudioClip jumpSound;
    public AudioClip hitSound;
    public AudioClip fallSound;
    public AudioClip coinSound;
    public AudioClip winSound;
    private AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();

        playerController = new PlayerController();

        currentHealth = maxHealth;
        UpdateHealthUI();

        startPosition = transform.position; // Simpan posisi awal player

        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        playerController.Enable();

        playerController.Movement.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerController.Movement.Move.canceled += ctx => moveInput = Vector2.zero;

        playerController.Movement.Jump.performed += ctx => Jump();
    }

    private void OnDisable()
    {
        playerController.Disable();
    }

    private void Update()
    {
        if (Application.isMobilePlatform)
        {
            moveInput = new Vector2(mobileInputX, 0f);
        }
        else
        {
            moveInput = playerController.Movement.Move.ReadValue<Vector2>();
        }

        // Tambahan: cek kalau player jatuh di bawah batas tertentu
        if (transform.position.y < -10f)
        {
            if (fallSound != null)
                audioSource.PlayOneShot(fallSound);

            TakeDamage(currentHealth, Vector2.up); // langsung kurangi semua health biar respawn
        }
    }

    private void FixedUpdate()
    {
        if (isKnockedBack) return;

        float totalInputX = moveInput.x + mobileInputX;

        // Hanya update velocity jika benar-benar ada input ke kanan/kiri
        if (Mathf.Abs(totalInputX) > 0.01f)
        {
            Vector2 targetVelocity = new Vector2(totalInputX * moveSpeed, rb.velocity.y);
            rb.velocity = targetVelocity;
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y); // Tidak gerak horizontal
        }

        UpdateAnimation(totalInputX);

        if (isGrounded() && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            isJumping = false;
        }
    }

    private void UpdateAnimation(float inputX)
    {
        MovementState state;

        // Cek apakah player benar-benar sedang bergerak horizontal
        if (Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            state = MovementState.walk;
            sprite.flipX = rb.velocity.x < 0;
        }
        else
        {
            state = MovementState.idle;
        }

        // Kondisi vertikal (lompat ke atas)
        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jump;
        }
        // Kondisi jatuh ke bawah dan belum menyentuh tanah
        else if (rb.velocity.y < -0.1f && !isGrounded())
        {
            state = MovementState.landing;
        }
        // Tambahan: Jika sedang di udara dan bergerak (misal tekan D atau A), tetap animasi jump
        else if (!isGrounded() && Mathf.Abs(inputX) > 0.1f)
        {
            state = MovementState.jump;
        }

        anim.SetInteger("state", (int)state);
    }

    private void Jump()
    {
        if (isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;

            if (jumpSound != null)
                audioSource.PlayOneShot(jumpSound);
        }
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    public void MoveRight(bool isPressed)
    {
        if (isPressed)
            mobileInputX = 1f;
        else if (mobileInputX == 1f)
            mobileInputX = 0f;
    }

    public void MoveLeft(bool isPressed)
    {
        if (isPressed)
            mobileInputX = -1f;
        else if (mobileInputX == -1f)
            mobileInputX = 0f;
    }

    public void MobileJump()
    {
        if (isGrounded())
        {
            Jump();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = currentHealth.ToString();
    }

    public void TakeDamage(int damage, Vector2 direction)
    {
        if (isKnockedBack) return;

        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = maxHealth; // Reset nyawa
            transform.position = startPosition; // Respawn ke posisi awal
            rb.velocity = Vector2.zero; // Hentikan gerakan
            UpdateHealthUI();
            return;
        }

        StartCoroutine(HandleKnockback(direction.normalized));
        UpdateHealthUI();
    }

    private IEnumerator HandleKnockback(Vector2 direction)
    {
        isKnockedBack = true;
        rb.velocity = Vector2.zero;

        Vector2 force = direction * knockBackThrust * rb.mass;
        rb.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockBackTime);
        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }

    public void SetPlayerInput(bool aktif)
    {
        if (aktif)
            playerController.Enable();
        else
            playerController.Disable();
    }

    // Tambahan fungsi pemanggil suara lain (opsional)
    public void PlayCoinSound()
    {
        if (coinSound != null)
            audioSource.PlayOneShot(coinSound);
    }

    public void PlayVictorySound()
    {
        if (winSound != null)
            audioSource.PlayOneShot(winSound);
    }
}
