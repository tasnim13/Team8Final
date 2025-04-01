using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for now this is just hand ot hand combat damange. we will later use this template to set up a swtich between weapons.
public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;              // Empty GameObject where attack originates
    public float attackRange = 1f;             // Radius for detecting enemies
    public LayerMask enemyLayers;              // Layer(s) that count as enemies

    private int currentWeaponIndex = 0;

    public int[] weaponDamages = { 10, 20, 35 };       // Damage for hand, weapon1, weapon2
    public Sprite[] weaponSprites;                     // Sprites for each weapon (set in Inspector)
    public SpriteRenderer weaponRenderer;              // SpriteRenderer to show current weapon

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SwitchWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void SwitchWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % weaponDamages.Length;

        string weaponName = currentWeaponIndex switch
        {
            0 => "Hand-to-Hand",
            1 => "axe1",
            2 => "bbat1",
            _ => "Unknown Weapon"
        };

        Debug.Log($"Switched to {weaponName} (Index {currentWeaponIndex})");

        // Set the sprite if it's not hand-to-hand, otherwise clear it
        if (weaponRenderer != null)
        {
            if (currentWeaponIndex == 0)
            {
                weaponRenderer.sprite = null; // Clear the weapon sprite
            }
            else if (weaponSprites.Length > currentWeaponIndex)
            {
                weaponRenderer.sprite = weaponSprites[currentWeaponIndex];
            }
        }
    }


    void Attack()
    {
        int attackDamage = weaponDamages[currentWeaponIndex];
        Debug.Log($"Attacking with weapon {currentWeaponIndex}, Damage: {attackDamage}");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        Debug.Log($"Enemies in range: {hitEnemies.Length}");

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"Hit enemy: {enemy.name}");
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
        }
    }

    // Draw attack range in Scene view
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
