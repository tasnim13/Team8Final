using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for now this is just hand ot hand combat damange. we will later use this template to set up a swtich between weapons.
public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;      // Empty GameObject for the attack origin
    public float attackRange = 1f;     // Radius of attack range
    public int attackDamage = 10;      // How much damage to deal
    public LayerMask enemyLayers;      // Set this in inspector to "Enemy" layer

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        // Detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
        }
    }

    // Draw the attack range in editor
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
