using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyParent : MonoBehaviour {
    [Header("Don't Touch")]
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
    private SpriteRenderer spriteRenderer;
    protected bool isDead = false;

    public GameHandler gameHandler;
    public PlayerHealthBar playerHealthBar;

    [Header("Death Art")]
    public Sprite deathSprite;

    public virtual void Start() {
        enemyCollider = GetComponent<CircleCollider2D>();
        enemyCollider.radius = attackRange;
        enemyCollider.offset = new Vector2(sightOffsetX, sightOffsetY);

        scaleX = transform.localScale.x;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();

        currHealth = health;
        healthBar.Initialize();

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerHealthBar = GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<PlayerHealthBar>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public virtual void Update() {
        if (isDead) return;

        if (isAttacking && Time.time >= lastAttackTime + attackCooldown) {
            lastAttackTime = Time.time;
            gameHandler.playerGetHit(damage);
            playerHealthBar.UpdateHealthBar();
        }
    }

    public virtual void FixedUpdate() {
        if (isDead) return;

        float distToPlayer = Vector3.Distance(transform.position, target.position);
        Vector2 moveDirection = Vector2.zero;

        if (distToPlayer <= sightRange && target != null) {
            moveDirection = (target.position - transform.position).normalized;
            Vector2 moveDelta = moveDirection * movementSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveDelta);

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

        isAttacking = false;
        rb.velocity = Vector2.zero;
        rb.simulated = false;
        enemyCollider.enabled = false;

        anim.enabled = false;

        if (deathSprite != null) {
            spriteRenderer.sprite = deathSprite;
        }

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
}