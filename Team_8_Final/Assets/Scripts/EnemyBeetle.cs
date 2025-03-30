using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeetle : EnemyParent
{
    [Header ("Beetle-Specific Stats")]
    public float jumpDistance = 1f;
    public float jumpDuration = 0.1f;
    //This are the upper and lower bounds of the randomizer for jump time
    public float jumpTimeMin = 0f;
    public float jumpTimeMax = 10f;
    private float lastJumpTime = 0f;
    //This float will be randomized by the script
    private float jumpCooldown = 3f;
    //Used for executing the jump
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float jumpTimer = 0f;
    private bool isJumping = false;

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        //Jump in a random direction at a random time
        if (Time.time >= lastJumpTime + jumpCooldown) {
            JumpRandomDirection();
            //Reset timer
            lastJumpTime = Time.time;
            //Randomize time until next jump
            jumpCooldown = Random.Range(jumpTimeMin, jumpTimeMax);
            Debug.Log("Jump cooldown is " + jumpCooldown);
        }

        if (isJumping) {
            jumpTimer += Time.deltaTime;
            float t = jumpTimer / jumpDuration;

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            if (t >= 1f)
            {
                isJumping = false;
            }
        }
    }

    public void JumpRandomDirection()
    {
        if (isJumping) return;

        //Generate a random normalized direction, aimed a bit upward
        Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        //Setup jump
        startPosition = transform.position;
        targetPosition = startPosition + (Vector3)(randomDir * jumpDistance);
        jumpTimer = 0f;
        isJumping = true;
    }
}
