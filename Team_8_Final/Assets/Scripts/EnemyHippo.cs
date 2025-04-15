using UnityEngine;

public class EnemyHippo : EnemyParent
{
    [Header("Elephant-Specific Stats")]
    public float visionWidth = 3f;
    public float visionHeight = 10f;
    public Transform visualTransform;

    public override void Start() {
        base.Start();
        anim = GetComponentInChildren<Animator>();
    }

    public override void FixedUpdate()
    {
        if (target == null) return;

        Vector2 moveDirection = Vector2.zero;

        if (IsPlayerInSight())
        {
            moveDirection = (target.position - transform.position).normalized;

            // Movement
            Vector2 newPosition = rb.position + moveDirection * movementSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);

            // Rotate
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

        // Animator blend tree inputs
        anim.SetFloat("inputX", moveDirection.x);
        anim.SetFloat("inputY", moveDirection.y);

        if (moveDirection != Vector2.zero)
        {
            anim.SetFloat("lastInputX", moveDirection.x);
            anim.SetFloat("lastInputY", moveDirection.y);
        }

        // Walk toggle
        bool isMoving = (transform.position != lastPosition);
        anim.SetBool("isMoving", isMoving);
        lastPosition = transform.position;

        // Keep visual upright (no rotation)
        if (visualTransform != null) {
            visualTransform.rotation = Quaternion.identity;
        }
    }

    public override void Update()
    {
        // Use parent's attack logic
        if (isAttacking && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            gameHandler.playerGetHit(damage);
            playerHealthBar.UpdateHealthBar();
        }
    }

    private bool IsPlayerInSight()
    {
        // Create a rectangle from the top of the enemy
        Vector2 origin = transform.position + transform.up * (visionHeight / 2);
        Vector2 size = new Vector2(visionWidth, visionHeight);
        Collider2D[] hits = Physics2D.OverlapBoxAll(origin, size, transform.eulerAngles.z);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        // Show vision box in Scene view
        Vector2 origin = transform.position + transform.up * (visionHeight / 2);
        Vector2 size = new Vector2(visionWidth, visionHeight);
        Quaternion rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z);

        Gizmos.matrix = Matrix4x4.TRS(origin, rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);
        Gizmos.matrix = Matrix4x4.identity;

        // Show attack hitbox in Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
