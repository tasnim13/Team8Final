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
        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();;

        //Set health to max
        currHealth = health;
        healthBar.Initialize();

        //Obtain animator and position for walking purposes
        anim = GetComponent<Animator>();
        lastPosition = transform.position;
        
    }

    public virtual void Update () {
        //Track player position and lurk when in sight range
        float DistToPlayer = Vector3.Distance(transform.position, target.position);

        //Detect if player is within sight range
        if ((target != null) && (DistToPlayer <= sightRange)){
                //Actively lurk the player
                transform.position = Vector2.MoveTowards (transform.position, target.position, movementSpeed * Time.deltaTime);
                
                //Calculate direction to player
                Vector2 direction = target.position - transform.position;
                //Calculate angle in degrees
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                //Make sure the top of the sprite faces the player
                transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

        //Simple check to see if the enemy is moving
        bool isMoving = (transform.position != lastPosition);
        anim.SetBool("isMoving", isMoving);
        lastPosition = transform.position;

        //Deal damage when player is within attack range
        if (isAttacking && Time.time >= lastAttackTime + attackCooldown) {
                // GetComponent<AudioSource>().Play();
                lastAttackTime = Time.time;
                gameHandler.playerGetHit(damage);
                playerHealthBar.UpdateHealthBar();
        }
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
