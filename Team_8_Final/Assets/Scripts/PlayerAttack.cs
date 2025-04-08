using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for now we are manually setting the weapons. we need to decide if thats how we want to keep it or if we want a do a random weapon kind of situation.
public class PlayerAttack : MonoBehaviour
{
    public AudioClip weaponSwitchClip;           // Sound to play on weapon switch
    private AudioSource audioSource;     
    public Transform attackPoint;              // point of origin of attack... move aorund in scene to adjust
    public float attackRange = 1f;             // attack radius... can be editied in inspector
    public LayerMask enemyLayers;              // designate enemy layer

    private int currentWeaponIndex = 0;

    public int[] weaponDamages = { 10, 20, 35 };       // damage array for each sprite... can be editied in inspector
    public Sprite[] weaponSprites;                     // sprite array for different weapons
    public SpriteRenderer weaponRenderer;              

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
void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

        if (weaponRenderer != null)
        {
            if (weaponSprites != null && weaponSprites.Length > currentWeaponIndex)
            {
                weaponRenderer.sprite = weaponSprites[currentWeaponIndex];
            }
            else
            {
                weaponRenderer.sprite = null;
            }
        }

        // Play weapon switch sound
        if (weaponSwitchClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(weaponSwitchClip);
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
            enemy.GetComponent<EnemyParent>()?.TakeDamage(attackDamage);
        }
    }

}
