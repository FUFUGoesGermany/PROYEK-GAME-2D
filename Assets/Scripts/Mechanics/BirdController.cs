using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
public class BirdController : MonoBehaviour
{
    [Header("Flight Model v0.6")]
    [SerializeField] private float flapImpulse = 4f;
    [SerializeField] private float maxRiseSpeed = 6f;
    [SerializeField] private float diveSpeed = 7f;
    [SerializeField] private float diveAccel = 30f;
    [SerializeField] private float hoverDamp = 10f;
    private bool flapQueued;
    
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    public int currentHealth;

    [Header("Boundaries")]
    [Tooltip("Batas atas layar (mentok, tidak mati)")]
    [SerializeField] private float topBoundary = 4.5f;

    [Tooltip("Batas bawah layar (mati)")]
    [SerializeField] private float bottomBoundary = -4.5f;


    [Header("Invincibility")]
    [Tooltip("Durasi kebal setelah spawn (detik)")]
    [SerializeField] private float invincibilityDuration = 1.5f;


    [Header("Debug")]
    [Tooltip("Aktifkan restart dengan tombol Space (untuk testing)")]
    [SerializeField] private bool enableDebugRestart = true;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject[] heartIcons;
    


    // Simpan posisi spawn awal
    private Vector2 initialSpawnPosition;

    // Private 
    private Rigidbody2D rb;
    private float invincibilityTimer;
    private bool isInvincible;
    private bool isDead;
    public int CurrentHearts => currentHealth;

    //  Properties untuk diakses sistem lain 
    public bool IsDead => isDead;
    public bool IsInvincible => isInvincible;
    public float CurrentVelocityY => rb != null ? rb.linearVelocity.y : 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        
        rb.bodyType = RigidbodyType2D.Kinematic; // Mulai kinematic, nanti diubah saat game start
    }

    void Start()
    {
        initialSpawnPosition = transform.position;
        currentHealth = maxHealth;
        isDead = false;

        // update UI health
        if (healthText != null)
        {
            healthText.text = $"Hp: {currentHealth}";
        }

        StartInvincibility();
    }

    void FixedUpdate()
    {
        if (isDead) return;
        if (rb.bodyType == RigidbodyType2D.Kinematic)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        } 

        Vector2 vel = rb.linearVelocity;

        if (flapQueued)
        { 
            vel.y += flapImpulse;
            flapQueued = false;  
        }

        if (Input.GetKey(KeyCode.S))
        {
            vel.y = Mathf.MoveTowards(vel.y, -diveSpeed, diveAccel * Time.fixedDeltaTime);
        }
        else
        {
            vel.y = Mathf.MoveTowards(vel.y, 0f, hoverDamp  * Time.fixedDeltaTime);
        }

        rb.linearVelocity = vel;
        ClampVelocity();
        HandleBoundaries();
    }

    void Update()
    {
        UpdateInvincibility();
        HandleDebugRestart();

        if (Input.GetKeyDown(KeyCode.W))
        {
            flapQueued = true;
        }
    }
   
    // Debug Restart
    private void HandleDebugRestart()
    {
        if (!enableDebugRestart) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetBird(initialSpawnPosition);
            Debug.Log("[BirdController] Restarting bird...");
        }
    }
    
    private void ClampVelocity()
    {
        Vector2 vel = rb.linearVelocity;
        vel.y = Mathf.Clamp(vel.y, -diveSpeed, maxRiseSpeed);
        rb.linearVelocity = vel;
    }

    
    //  BOUNDARIES
    private void HandleBoundaries()
    {
        Vector2 pos = transform.position;

        // Batas atas mentok
        if (pos.y > topBoundary)
        {
            pos.y = topBoundary;
            transform.position = pos;

            // Hentikan velocity naik
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            }
        }

        // Batas bawah
        if (pos.y < bottomBoundary)
        {
            Die();
        }
    }

    
    //  INVINCIBILITY
    
    public void StartInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
        
    }

    private void UpdateInvincibility()
    {
        if (!isInvincible) return;

        invincibilityTimer -= Time.deltaTime;

        // Efek blink selama invincible (visual feedback)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.enabled = Mathf.PingPong(Time.unscaledTime * 8f, 1f) > 0.3f;
        }

        if (invincibilityTimer <= 0f)
        {
            isInvincible = false;
            if (sr != null) sr.enabled = true;
        }
    }

    
    //  DEATH dan COLLISION
    public void Die()
    {
        if (isDead) return;

        isDead = true;

        // Hentikan semua gerakan
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        Debug.Log("[BirdController] Bird died!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead || isInvincible) return;

        string tag = collision.gameObject.tag;

        Debug.Log($"[Bird] Ditabrak oleh: {collision.gameObject.name} (tag: {tag})");
        if (tag == "Obstacle" || tag == "Enemy")
        {
            TakeDamage();
        }   
    }

    public void TakeDamage()
    {
        if (isInvincible || isDead) return;

        currentHealth--;
        if (healthText != null)
        {
            healthText.text = $"Hp: {currentHealth}";
        }
        Debug.Log($"[BirdController] Si Fufu kena damage: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // jika masih ada nyawa, mulai invincible lagi
            StartInvincibility();
        }
    }

    public void SetBirdKinematic(bool isKinematic)
    {
        rb.bodyType = isKinematic ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
    }


    public void ResetBird(Vector2 spawnPosition)
    {
        currentHealth = maxHealth;
        isDead = false;
        isInvincible = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;
        transform.position = spawnPosition;
        SetBirdKinematic(false);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = true;
    
        if (healthText != null)
        {
            healthText.text = $"Hp: {currentHealth}";
        }

        StartInvincibility();
    }
}