using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyParent : MonoBehaviour
{
    [Header ("Don't Touch")]
    private CircleCollider2D enemyCollider;
    public Transform target;
    public bool isAttacking = false;
    public float lastAttackTime = 0f;

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

    public virtual void Start () {
        //Obtain circle collider for attack range purposes
        enemyCollider = GetComponent<CircleCollider2D>();
        enemyCollider.radius = attackRange;
        enemyCollider.offset = new Vector2(sightOffsetX, sightOffsetY);

        //Obtain position of this object
        scaleX = gameObject.transform.localScale.x;

        //Identify player for position tracking
        if (GameObject.FindGameObjectWithTag ("Player") != null) {
                target = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
        }

        //Set health to max
        currHealth = health;
        healthBar.Initialize();
    }

    public virtual void FixedUpdate () {
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

        //Deal damage when player is within attack range
        if (isAttacking && Time.time >= lastAttackTime + attackCooldown) {
                GameHandler.playerHealth -= damage;
                lastAttackTime = Time.time;
        }
    }

    public virtual void Update() {
        //Test health bar & damage
        if (Input.GetKeyDown(KeyCode.Space)) {
            TakeDamage(4);
        }
    }

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

    public void TakeDamage(int damage) {
        currHealth -= damage;
        currHealth = Mathf.Max(0, currHealth);
        float percent = (float)currHealth / health;
        healthBar.UpdateBar(percent);

        //Destoy enemy if health reaches zero
        if (currHealth <= 0)
        {
            Destroy(gameObject);
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
