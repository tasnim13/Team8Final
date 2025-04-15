using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeetle : EnemyParent
{
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

    public override void FixedUpdate()
    {
        if (isJumping || target == null) return;

        Vector2 moveDirection = Vector2.zero;
        float distToPlayer = Vector3.Distance(transform.position, target.position);

        if (distToPlayer <= sightRange)
        {
            moveDirection = (target.position - transform.position).normalized;

            Vector2 newPosition = rb.position + moveDirection * movementSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);

            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

        anim.SetFloat("inputX", moveDirection.x);
        anim.SetFloat("inputY", moveDirection.y);

        if (moveDirection != Vector2.zero)
        {
            anim.SetFloat("lastInputX", moveDirection.x);
            anim.SetFloat("lastInputY", moveDirection.y);
        }

        bool isMoving = (transform.position != lastPosition);
        anim.SetBool("isMoving", isMoving);
        lastPosition = transform.position;
    }

    public override void Update()
    {
        base.Update(); // Handles attack logic

        // Execute jump behavior
        if (!isJumping && Time.time >= lastJumpTime + jumpCooldown)
        {
            JumpRandomDirection();
            anim.SetBool("isFlying", true);
            lastJumpTime = Time.time;
            jumpCooldown = Random.Range(jumpTimeMin, jumpTimeMax);
            Debug.Log("Jump cooldown is " + jumpCooldown);
        }

        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            float t = jumpTimer / jumpDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            if (t >= 1f)
            {
                isJumping = false;
                anim.SetBool("isFlying", false);
            }
        }
    }

    public void JumpRandomDirection()
    {
        if (isJumping) return;

        Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        startPosition = transform.position;
        targetPosition = startPosition + (Vector3)(randomDir * jumpDistance);
        jumpTimer = 0f;
        isJumping = true;
    }
}
