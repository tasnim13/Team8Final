using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeetle : EnemyParent {
    [Header("Beetle-Specific Stats")]
    public float jumpDistance = 1f;
    public float jumpDuration = 0.1f;
    public float jumpTimeMin = 0f;
    public float jumpTimeMax = 10f;

    private float lastJumpTime = 0f;
    private float jumpCooldown = 3f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float jumpTimer = 0f;
    private bool isJumping = false;

    [Header("Jump Obstacle Check")]
    public LayerMask obstacleMask;

    public override void FixedUpdate() {
        if (isDead || isJumping || target == null) return;

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
        base.Update(); //Handles attack logic

        if (!isJumping && Time.time >= lastJumpTime + jumpCooldown) {
            TryJump();
            anim.SetBool("isFlying", true);
            lastJumpTime = Time.time;
            jumpCooldown = Random.Range(jumpTimeMin, jumpTimeMax);
            Debug.Log("Jump cooldown is " + jumpCooldown);
        }

        if (isJumping) {
            jumpTimer += Time.deltaTime;
            float t = jumpTimer / jumpDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            if (t >= 1f) {
                isJumping = false;
                anim.SetBool("isFlying", false);
            }
        }
    }

    private void TryJump() {
        if (isDead || isJumping) return;

        for (int i = 0; i < 10; i++) {
            Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Vector3 candidateTarget = transform.position + (Vector3)(randomDir * jumpDistance);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, randomDir, jumpDistance, obstacleMask);
            bool blockedLanding = Physics2D.OverlapCircle(candidateTarget, 0.2f, obstacleMask);

            if (!hit && !blockedLanding) {
                startPosition = transform.position;
                targetPosition = candidateTarget;
                jumpTimer = 0f;
                isJumping = true;
                return;
            }
        }

        Debug.Log("Beetle jump blocked on all sides.");
    }
}
