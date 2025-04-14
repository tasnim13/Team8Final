using UnityEngine;

public class EnemyPhoenix : EnemyParent
{
    [Header("Phoenix-Specific")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;
    public float rotationSpeed = 180f; // degrees per second
    [Header("Phoenix Spiral Attack")]
    public int shotsBeforeSpiral = 3;
    public int spiralFireballCount = 8;
    private int shotsFiredSinceLastSpiral = 0;


    public override void FixedUpdate()
    {
        if (target == null) return;

        float distToPlayer = Vector3.Distance(transform.position, target.position);

        if (distToPlayer <= sightRange)
        {
            // Movement (keep from parent)
            Vector2 direction = (target.position - transform.position).normalized;
            Vector2 newPosition = rb.position + direction * movementSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);

            // 🔄 Custom: Rotate gradually toward player
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion desiredRotation = Quaternion.Euler(0f, 0f, targetAngle - 90f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        // Movement animation
        bool isMoving = (transform.position != lastPosition);
        anim.SetBool("isMoving", isMoving);
        lastPosition = transform.position;
    }

    public override void Update()
    {
        if (isAttacking && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            ShootFireball();
        }
    }

    private void ShootFireball() {
        if (target == null || fireballPrefab == null || firePoint == null) return;

        shotsFiredSinceLastSpiral++;

        // 🔁 Regular fireball shot (in facing direction)
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = firePoint.up;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fireball.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

        // Sprial every few shots
        if (shotsFiredSinceLastSpiral >= shotsBeforeSpiral)
        {
            FireSpiral();
            shotsFiredSinceLastSpiral = 0;
        }
    }

    private void FireSpiral() {
        float angleStep = 360f / spiralFireballCount;

        for (int i = 0; i < spiralFireballCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            // Calculate outward direction
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            fireball.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }
        }
    }
}
