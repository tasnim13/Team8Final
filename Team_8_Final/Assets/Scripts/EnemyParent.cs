using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class EnemyParent : MonoBehaviour {
    [Header("Hide In Inspector")]
    public Animator anim;
    private CircleCollider2D enemyCollider;
    public Transform target;
    public bool isAttacking = false;
    public float lastAttackTime = Mathf.NegativeInfinity;
    public Vector3 lastPosition;
    public Rigidbody2D rb;

    [Header("Enemy Stats")]
    public float movementSpeed = 4f;
    public float sightRange = 10f;
    public float sightOffsetX = 0f;
    public float sightOffsetY = 0f;

    public int damage = 10;
    public float attackRange = 1f;
    public float attackCooldown = 1f;

    public int health = 100;
    private int currHealth = 0;
    public HealthBar healthBar;

    private float scaleX;
    private Color originalColor;
    private SpriteRenderer spriteRenderer; // ignore this
    public GameObject enemyShadow;
    protected bool isDead = false;

    public GameHandler gameHandler;
    public PlayerHealthBar playerHealthBar;

    [Header("Death Art")]
    public Sprite deathSprite;
    public Vector3 deathArtScale = Vector3.one;
    public GameObject deathParticlesPrefab;

    [Header("Attack Visual Effect")]
    public GameObject attackEffect;
    public float attackEffectDistance = 0.5f;
    private SpriteRenderer attackEffectRenderer;

    // for music layering
    private bool isCombat = false;

    public virtual void Start() {
        enemyCollider = GetComponent<CircleCollider2D>();
        enemyCollider.radius = attackRange;
        enemyCollider.offset = new Vector2(sightOffsetX, sightOffsetY);

        scaleX = transform.localScale.x;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();

        currHealth = health;
        healthBar.Initialize();

        enemyShadow.SetActive(true);


        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerHealthBar = GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<PlayerHealthBar>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        //Cache attack effect
        if (attackEffect != null) {
            attackEffectRenderer = attackEffect.GetComponent<SpriteRenderer>();
            Color c = attackEffectRenderer.color;
            c.a = 0f;
            attackEffectRenderer.color = c;
        }
    }

    public virtual void Update() {
        if (isDead) return;

        if (isAttacking && Time.time >= lastAttackTime + attackCooldown) {
            lastAttackTime = Time.time;
            gameHandler.playerGetHit(damage);
            playerHealthBar.UpdateHealthBar();
            TriggerAttackEffect();
        }
    }

    public virtual void FixedUpdate() {
        if (isDead || isAttacking) return;

        float distToPlayer = Vector3.Distance(transform.position, target.position);
        Vector2 moveDirection = Vector2.zero;

        if (distToPlayer <= sightRange && target != null) {
            moveDirection = (target.position - transform.position).normalized;
            Vector2 moveDelta = moveDirection * movementSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveDelta);

            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("isCombat", 1f, false); // music: fades in intense percussion when an enemy sees you. included as a default in case some enemies don't override FixedUpdate

            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

        anim.SetFloat("inputX", moveDirection.x);
        anim.SetFloat("inputY", moveDirection.y);

        if (moveDirection != Vector2.zero) {
            anim.SetFloat("lastInputX", moveDirection.x);
            anim.SetFloat("lastInputY", moveDirection.y);
        }

        bool isMoving = (transform.position != lastPosition);
        anim.SetBool("isMoving", isMoving);
        lastPosition = transform.position;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision) {
        if (isDead) return;

        if (collision.CompareTag("Player")) {
            isAttacking = true;
            lastAttackTime = Time.time;
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            isAttacking = false;
        }
    }

    public void Die() {
        if (isDead) return;
        isDead = true;

        Debug.Log($"{gameObject.name} is dead.");

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("isCombat", 0f, false); // music: fades out intense percussion when enemy dies. if multiple enemies are aggro'd at the same time, this shouldn't happen due to FixedUpdate

        isAttacking = false;
        rb.velocity = Vector2.zero;
        rb.simulated = false;
        enemyCollider.enabled = false;

        anim.enabled = false;

        if(deathSprite != null){
            spriteRenderer.sprite = deathSprite;
            transform.localScale = deathArtScale;
        }
        enemyShadow.SetActive(false);


        transform.rotation = Quaternion.identity;

        if (healthBar != null) {
            healthBar.gameObject.SetActive(false);
        }

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        LeanTween.value(gameObject, 1f, 0f, 3f)
            .setOnUpdate((float alpha) => {
                Color c = spriteRenderer.color;
                c.a = alpha;
                spriteRenderer.color = c;
            })
            .setOnComplete(() => Destroy(gameObject))
            .setEase(LeanTweenType.easeInOutQuad);

        if (deathParticlesPrefab != null) {
            Vector3 spawnPos = transform.position + new Vector3(0f, -0.2f, 0f);
            GameObject particles = Instantiate(deathParticlesPrefab, spawnPos, deathParticlesPrefab.transform.rotation);

            ParticleSystem ps = particles.GetComponent<ParticleSystem>();
            if (ps != null) {
                ps.Play();
                Destroy(particles, ps.main.duration + ps.main.startLifetime.constantMax);
            } else {
                Destroy(particles, 3f); //fallback
            }
        }

        GameHandler handler = FindObjectOfType<GameHandler>();
        if (handler != null) {
            for (int i = 0; i < handler.enemiesToDefeat.Length; i++) {
                if (handler.enemiesToDefeat[i] == gameObject) {
                    handler.enemiesToDefeat[i] = null;
                    break;
                }
            }
            handler.CheckEnemiesStatus();
        }
    }

    public void TakeDamage(int damage) {
        if (isDead) return;

        currHealth -= damage;
        currHealth = Mathf.Max(0, currHealth);
        float percent = (float)currHealth / health;
        healthBar.UpdateBar(percent);
        DamageFlash();
        if (currHealth <= 0) {
            Die();
        }
    }

    public void DamageFlash() {
        spriteRenderer.color = Color.red;
        LeanTween.value(gameObject, 0f, 1f, 0.2f)
            .setOnUpdate((float t) => {
                spriteRenderer.color = Color.Lerp(Color.red, originalColor, t);
            })
            .setEase(LeanTweenType.linear);
    }

    public virtual void OnDrawGizmosSelected() {
        Vector3 offset = new Vector3(sightOffsetX, sightOffsetY, 0f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + offset, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offset, attackRange);
    }

    //Show visual attack effect and fade it out
    public void TriggerAttackEffect() {
        if (attackEffectRenderer == null) return;

        //Face direction and position offset
        if (target != null) {
            Vector2 dir = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            attackEffect.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
            attackEffect.transform.position = transform.position + (Vector3)(dir * attackEffectDistance);
        }

        //Make fully visible
        Color c = attackEffectRenderer.color;
        c.a = 1f;
        attackEffectRenderer.color = c;

        //Fade out
        LeanTween.value(attackEffect, 1f, 0f, 1f).setOnUpdate((float a) => {
            Color fade = attackEffectRenderer.color;
            fade.a = a;
            attackEffectRenderer.color = fade;
        }).setEase(LeanTweenType.linear);
    }

    public bool IsDead() {
            return isDead;
        }
}