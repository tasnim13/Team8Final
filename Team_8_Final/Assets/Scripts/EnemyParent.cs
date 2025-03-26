using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyParent : MonoBehaviour
{
    private CircleCollider2D enemyCollider;
    private Transform target;

    [Header ("Enemy Stats")]
    public float movementSpeed = 4f;
    public float sightRange = 10;

    public int damage = 10;
    public float attackRange = 1f;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    public float attackCooldown = 1f;

    public int health = 100;

    private float scaleX;

    void Start () {
            //Obtain circle collider for attack range purposes
            enemyCollider = GetComponent<CircleCollider2D>();
            enemyCollider.radius = attackRange;

            //Obtain position of this object
            scaleX = gameObject.transform.localScale.x;

            //Identify player for position tracking
            if (GameObject.FindGameObjectWithTag ("Player") != null) {
                    target = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
            }
    }

    void FixedUpdate () {
            //Track player position and lurk when in sight range
            float DistToPlayer = Vector3.Distance(transform.position, target.position);

            if ((target != null) && (DistToPlayer <= sightRange)){
                    transform.position = Vector2.MoveTowards (transform.position, target.position, movementSpeed * Time.deltaTime);
                //flip enemy to face player direction. Wrong direction? Swap the * -1.
                if (target.position.x > gameObject.transform.position.x){
                                gameObject.transform.localScale = new Vector2(scaleX, gameObject.transform.localScale.y);
                } else {
                                gameObject.transform.localScale = new Vector2(scaleX * -1, gameObject.transform.localScale.y);
                }
            }

            //Deal damage when player is within attack range
            if (isAttacking && Time.time >= lastAttackTime + attackCooldown) {
                GameHandler.playerHealth -= damage;
                lastAttackTime = Time.time;
            }
    }

    //When player is in range and being attacked
    public void OnTriggerEnter2D(Collider2D collision){
            if (collision.CompareTag("Player")) {
                    isAttacking = true;
                    Debug.Log("DAMAGE!");
                    //anim.SetBool("Attack", true);
            }
    }

    //When player is out of range and no longer being attacked
    public void OnTriggerExit2D(Collider2D collision){
            if (collision.CompareTag("Player")) {
                    Debug.Log("NO DAMAGE.");
                    isAttacking = false;
                    //anim.SetBool("Attack", false);
            }
    }

    //DISPLAY attack range and sight range
    void OnDrawGizmosSelected(){
            Gizmos.DrawWireSphere(transform.position, sightRange);
            Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
