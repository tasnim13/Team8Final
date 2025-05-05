using UnityEngine;

public class EnemyPhoenix : EnemyParent {
    [Header("Phoenix-Specific")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;
    public float rotationSpeed = 180f;

    //For movement pause
    private float idleTimer = 0f;
    private bool isInIdlePhase = false;
    private float idleDuration = 2f;
    private float idleCycle = 4f;
    private Quaternion savedRotation;

    [Header("Phoenix Spiral Attack")]
    public int shotsBeforeSpiral = 3;
    public int spiralFireballCount = 8;
    private int shotsFiredSinceLastSpiral = 0;

    public override void FixedUpdate() {
        if (isDead || target == null) return;

        //Update idle timer
        idleTimer += Time.fixedDeltaTime;

        //Start idle phase every 5 seconds
        if (!isInIdlePhase && idleTimer >= idleCycle) {
            isInIdlePhase = true;
            savedRotation = transform.rotation; //Save current rotation
            transform.rotation = Quaternion.identity;
            idleTimer = 0f;
        }

        //If idle phase is active, remain idle
        if (isInIdlePhase) {
            anim.SetBool("isMoving", false);
            anim.SetFloat("inputX", 0f);
            anim.SetFloat("inputY", 0f);
            rb.velocity = Vector2.zero;

            if (idleTimer >= idleDuration) {
                isInIdlePhase = false;
                transform.rotation = savedRotation; //Restore rotation
                idleTimer = 0f;
            }

            return;
        }

        float distToPlayer = Vector3.Distance(transform.position, target.position);
        Vector2 moveDirection = Vector2.zero;

        if (distToPlayer <= sightRange) {
            moveDirection = (target.position - transform.position).normalized;

            Vector2 newPosition = rb.position + moveDirection * movementSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);

            float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            Quaternion desiredRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, targetAngle - 90f), rotationSpeed * Time.fixedDeltaTime);
            transform.rotation = desiredRotation;
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
            Debug.Log("[Phoenix] Shooting fireball!");
            lastAttackTime = Time.time;
            ShootFireball();
        }
    }

    private void ShootFireball() {
        if (target == null || fireballPrefab == null || firePoint == null) {
            Debug.Log("fireball null reference");
            return;
        }

        shotsFiredSinceLastSpiral++;

        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        Debug.Log("Active: " + fireball.activeInHierarchy);
        Debug.Log("Fireball made at" + firePoint.position);

        Vector2 direction = firePoint.up;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fireball.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.velocity = direction * projectileSpeed;
        }

        if (shotsFiredSinceLastSpiral >= shotsBeforeSpiral) {
            FireSpiral();
            shotsFiredSinceLastSpiral = 0;
        }
    }

    private void FireSpiral() {
        if (fireballPrefab == null || firePoint == null) return;

        float angleStep = 360f / spiralFireballCount;

        for (int i = 0; i < spiralFireballCount; i++) {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            fireball.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            if (rb != null) {
                rb.velocity = direction * projectileSpeed;
            }
        }
    }
}