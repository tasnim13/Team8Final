using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCroc : EnemyParent {
    [Header("Croc-Specific Stats")]
    public int hitsBeforeSpin = 3;
    private int hits = 0;
    public int spinDamage = 12;
    public float spinDuration = 0.5f;

    public override void FixedUpdate() {
        if (isDead || target == null) return;

        Vector2 moveDirection = Vector2.zero;
        float distToPlayer = Vector3.Distance(transform.position, target.position);

        if (distToPlayer <= sightRange) {
            moveDirection = (target.position - transform.position).normalized;

            Vector2 newPosition = rb.position + moveDirection * movementSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);

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

    public override void Update() {
        if (isDead) return;

        if (isAttacking && Time.time >= lastAttackTime + attackCooldown) {
            lastAttackTime = Time.time;

            if (hits >= hitsBeforeSpin) {
                GameHandler.playerHealth -= spinDamage;
                StartCoroutine(SpinAttack(spinDuration));
                hits = 0;
            } else {
                GameHandler.playerHealth -= damage;
                hits++;
            }

            playerHealthBar.UpdateHealthBar();
        }
    }

    private IEnumerator SpinAttack(float duration) {
        anim.SetBool("isSpinning", true);
        float elapsed = 0f;
        float startRotation = transform.eulerAngles.z;
        float endRotation = startRotation + 360f;

        while (elapsed < duration) {
            if (isDead) yield break;

            float zRotation = Mathf.Lerp(startRotation, endRotation, elapsed / duration);
            transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, endRotation % 360f);
        anim.SetBool("isSpinning", false);
    }
}