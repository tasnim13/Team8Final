using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScorpion : EnemyParent
{
    [Header ("Scorpion-Specific Stats")]
    public Material poisonMat;
    private SpriteRenderer playerSpriteRend;

    //FixedUpdate is same as parent except for poison function in attack range statement
    public override void Update() {
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
                StartCoroutine(PoisonPlayer());
                GameHandler.playerHealth -= damage;
                //gameHandler.playerGetHit(damage);
                lastAttackTime = Time.time;
        }
    }

    //TODO Affect movement speed when player is poisoned
    private IEnumerator PoisonPlayer() {
        Debug.Log("Player poisoned!");
        playerSpriteRend = target.GetComponentInChildren<SpriteRenderer>();
        Material originalMat = playerSpriteRend.material;
        playerSpriteRend.material = poisonMat;
        yield return new WaitForSeconds(5);
        Debug.Log("Player unpoisoned!");
        playerSpriteRend.material = originalMat;
    }
}