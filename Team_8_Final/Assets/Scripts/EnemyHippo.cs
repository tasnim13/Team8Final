using UnityEngine;

public class EnemyHippo : EnemyParent
{
    [Header("Hippo-Specific Stats")]
    public float visionWidth = 3f;
    public float visionHeight = 10f;

    public override void Update() {

        if (target != null && IsPlayerInSight())
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);

            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        } 

        if (isAttacking && Time.time >= lastAttackTime + attackCooldown)
        {
            // GetComponent<AudioSource>().Play();
            GameHandler.playerHealth -= damage;
            //gameHandler.playerGetHit(damage);
            lastAttackTime = Time.time;
        }

        //Simple check to see if the enemy is moving
        bool isMoving = (transform.position != lastPosition);
        anim.SetBool("isMoving", isMoving);
        lastPosition = transform.position;
    }

    private bool IsPlayerInSight() {
        //Create a rectangle from the top of the enemy
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

    public override void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;

        //Show vision box in Scene view
        Vector2 origin = transform.position + transform.up * (visionHeight / 2);
        Vector2 size = new Vector2(visionWidth, visionHeight);
        Quaternion rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z);

        Gizmos.matrix = Matrix4x4.TRS(origin, rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);
        Gizmos.matrix = Matrix4x4.identity;
        //Show attack hitbox in Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}