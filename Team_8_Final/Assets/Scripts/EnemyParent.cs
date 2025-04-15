using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyParent : MonoBehaviour
{
    [Header ("Don't Touch")]
    public Animator anim;
    private CircleCollider2D enemyCollider;
    public Transform target;
    public bool isAttacking = false;
    public float lastAttackTime = 0f;
    public Vector3 lastPosition;
    public Rigidbody2D rb;

    [Header ("Enemy Stats")]
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
    
    public GameHandler gameHandler;
    public PlayerHealthBar playerHealthBar;

    public virtual void Start () {
        //Obtain circle collider for attack range purposes
        enemyCollider = GetComponent<CircleCollider2D>();
        enemyCollider.radius = attackRange;
        enemyCollider.offset = new Vector2(sightOffsetX, sightOffsetY);

        //Obtain position of this object
        scaleX = gameObject.transform.localScale.x;

        //Identify player for position tracking
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform> ();

        //Identify GameHandler
        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();

        //Set health to max
        currHealth = health;
        healthBar.Initialize();

        //Obtain animator for walking purposes
        anim = GetComponent<Animator>();

        //Get rigidbody
        rb = GetComponent<Rigidbody2D>();

        //Get health bar
        playerHealthBar = GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<PlayerHealthBar>();
    }

    public virtual void Update () {
        //Deal damage when player is within attack range
        if (isAttacking && Time.time >= lastAttackTime + attackCooldown) {
            // GetComponent<AudioSource>().Play();
            lastAttackTime = Time.time;
            gameHandler.playerGetHit(damage);
            playerHealthBar.UpdateHealthBar();
        }
    }

    public virtual void FixedUpdate() {
        //Track player position and lurk when in sight range
        float distToPlayer = Vector3.Distance(transform.position, target.position);

        //Detect if player is within sight range
        Vector2 moveDirection = Vector2.zero;

        if (distToPlayer <= sightRange && target != null) {
            moveDirection = (target.position - transform.position).normalized;

            Vector2 newPosition = rb.position + moveDirection * movementSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);

            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

        //Animation: Movement direction
        anim.SetFloat("inputX", moveDirection.x);
        anim.SetFloat("inputY", moveDirection.y);

        //Animation: Store last direction if there's movement
        if (moveDirection != Vector2.zero) {
            anim.SetFloat("lastInputX", moveDirection.x);
            anim.SetFloat("lastInputY", moveDirection.y);
        }

        //Check movement after applying Rigidbody movement
        bool isMoving = (transform.position != lastPosition);
        anim.SetBool("isMoving", isMoving);
        lastPosition = transform.position;
    }


    //Testing Health Bar
    /* public void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            TakeDamage(4);
        }
    } */

    //When player is in range and being attacked
    public virtual void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player")) {
                isAttacking = true;
        }
    }

    //When player is out of range and no longer being attacked
    public virtual void OnTriggerExit2D(Collider2D collision){
        if (collision.CompareTag("Player")) {
                isAttacking = false;
        }
    }

    public void Die()
    {
        Debug.Log($"{gameObject.name} is dead.");

        GameHandler handler = FindObjectOfType<GameHandler>();
        if (handler != null)
        {
            // Set this enemy to null in the enemiesToDefeat array
            for (int i = 0; i < handler.enemiesToDefeat.Length; i++)
            {
                if (handler.enemiesToDefeat[i] == gameObject)
                {
                    handler.enemiesToDefeat[i] = null;
                    break;
                }
            }

            handler.CheckEnemiesStatus();
        }

        Destroy(gameObject);
    }

    //Take damage, update health bar and destroy enemy when dead 
    public void TakeDamage(int damage) {
        currHealth -= damage;
        currHealth = Mathf.Max(0, currHealth);
        float percent = (float)currHealth / health;
        healthBar.UpdateBar(percent);

        //Destoy enemy if health reaches zero
        if (currHealth <= 0) {
            Die();
        }
    }

    

    //DISPLAY attack range and sight range
    public virtual void OnDrawGizmosSelected()
    {
        Vector3 offset = new Vector3(sightOffsetX, sightOffsetY, 0f);
        //Sight range - Blue
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + offset, sightRange);
        //Attack range - Red
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offset, attackRange);
    }
}
