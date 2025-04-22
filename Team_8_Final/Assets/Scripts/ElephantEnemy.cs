using UnityEngine;

public class ElephantEnemy : EnemyParent
{
    [Header("Elephant-Specific Vision Stats")]
    public float visionWidth = 3f;
    public float visionHeight = 10f;

    public override void Update()
    {
        if (target != null && IsPlayerInSight())
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);

            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            HandleAnimation(direction);
        }
        else
        {
            anim.Play("EleIdle");
        }

        if (isAttacking && Time.time >= lastAttackTime + attackCooldown)
        {
            GameHandler.playerHealth -= damage;
            lastAttackTime = Time.time;
            TriggerAttackEffect();
            playerHealthBar.UpdateHealthBar();
        }

        bool isMoving = (transform.position != lastPosition);
        anim.SetBool("isMoving", isMoving);
        lastPosition = transform.position;
    }

    private void HandleAnimation(Vector2 direction)
    {
        bool isMoving = direction.magnitude > 0.01f;
        anim.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            anim.SetFloat("directionY", direction.y);

            // Flip sideways when moving horizontally
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x < 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private bool IsPlayerInSight()
    {
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
        Vector2 origin = transform.position + transform.up * (visionHeight / 2);
        Vector2 size = new Vector2(visionWidth, visionHeight);
        Quaternion rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z);

        Gizmos.matrix = Matrix4x4.TRS(origin, rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);
        Gizmos.matrix = Matrix4x4.identity;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
