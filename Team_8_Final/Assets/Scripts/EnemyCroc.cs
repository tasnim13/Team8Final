using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCroc : EnemyParent
{
    [Header ("Croc-Specific Stats")]
    public int hitsBeforeSpin = 3;
    private int hits = 0;
    public int spinDamage = 12;
    public float spinDuration = 0.5f;

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
            if (hits >= hitsBeforeSpin) {
                GameHandler.playerHealth -= spinDamage;
                StartCoroutine(SpinAttack(spinDuration));
            } else {
                // GetComponent<AudioSource>().Play();
                GameHandler.playerHealth -= damage;
                //gameHandler.playerGetHit(damage);
                hits++;
            }

            lastAttackTime = Time.time;
        }
    }

    private IEnumerator SpinAttack(float duration) {
        anim.SetBool("isSpinning", true);
        float elapsed = 0f;
        float startRotation = transform.eulerAngles.z;
        float endRotation = startRotation + 360f;

        while (elapsed < duration)
        {
            float zRotation = Mathf.Lerp(startRotation, endRotation, elapsed / duration);
            transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
            elapsed += Time.deltaTime;
            yield return null;
        }

        //Snap to final rotation to avoid overshoot
        transform.rotation = Quaternion.Euler(0f, 0f, endRotation % 360f);
        anim.SetBool("isSpinning", false);
    }
}
