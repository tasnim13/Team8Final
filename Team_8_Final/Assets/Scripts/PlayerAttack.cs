using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for now we are manually setting the weapons. we need to decide if thats how we want to keep it or if we want a do a random weapon kind of situation.
public class PlayerAttack : MonoBehaviour
{
    private AudioSource audioSource;   

    [Header("Attack Stats")]
    public float attackRange = 1f;          
    public int attackDamage = 10;
    public Transform attackPoint; 
    public LayerMask enemyLayers;  
    public AudioClip attackSoundHit;
    public AudioClip attackSoundMiss;

    [Header("Falcon Attack")]
    public GameObject falconProjectile;
    public float projectileSpd = 10f;
    private float falconCooldown = 1f;
    private float lastFalconShotTime = -Mathf.Infinity;

    //private GameHandler gh;

    //private int currentWeaponIndex = 0;

    //public int[] weaponDamages = { 10, 20, 35 };       // damage array for each sprite... can be editied in inspector
    //public Sprite[] weaponSprites;                     // sprite array for different weapons
    //public SpriteRenderer weaponRenderer;              

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        /* if (Input.GetKeyDown(KeyCode.L))
        {
            SwitchWeapon();
        } */

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (GameHandler.currForm == 3) {
            FalconAttack();
            return;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        Debug.Log($"Enemies in range: {hitEnemies.Length}");

        if (hitEnemies.Length == 0) {
            audioSource.PlayOneShot(attackSoundMiss);
        } else {
            audioSource.PlayOneShot(attackSoundHit);
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"Hit enemy: {enemy.name}");
            enemy.GetComponent<EnemyParent>()?.TakeDamage(attackDamage);
        }
    }

    void FalconAttack() {
        //Check cooldown
        if (Time.time < lastFalconShotTime + falconCooldown) return;

        //Get last movement direction
        Vector2 direction = GetComponent<PlayerMove>().LastDirection;

        //Don't fire if no valid direction
        if (direction == Vector2.zero) {
            Vector2 direction = new Vector2(1, 0);
        }

        //Spawn projectile at attack point
        GameObject proj = Instantiate(falconProjectile, attackPoint.position, Quaternion.identity);

        //Set direction of projectile
        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        if (projRb != null) {
            projRb.velocity = direction.normalized * projectileSpd;
        }

        lastFalconShotTime = Time.time;
    }

    /* void SwitchWeapon()
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
    } */
}
