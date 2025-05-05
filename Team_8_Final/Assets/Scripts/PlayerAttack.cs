using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for now we are manually setting the weapons. we need to decide if thats how we want to keep it or if we want a do a random weapon kind of situation.
public class PlayerAttack : MonoBehaviour
{
    private AudioSource audioSource;
    private PlayerSpecialAttack spatk;

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
        spatk = GetComponent<PlayerSpecialAttack>();
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

        if (Input.GetKeyDown(KeyCode.E)) {
            SpecialAttack();
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

        //Fire right if no valid direction
        if (direction == Vector2.zero) {
            direction = new Vector2(1, 0);
        }

        //Calculate rotation to match direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, 0, angle);

        //Spawn projectile at attack point with rotation
        GameObject proj = Instantiate(falconProjectile, attackPoint.position, rot);

        //Set direction of projectile
        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        if (projRb != null) {
            projRb.velocity = direction.normalized * projectileSpd;
        }

        lastFalconShotTime = Time.time;
    }

    void SpecialAttack() {
        if (GameHandler.currForm == 0) spatk.defaultSpatk();
        else if (GameHandler.currForm == 3) spatk.falconSpatk();
        else if (GameHandler.currForm == 4) spatk.lionessSpatk();
        else return;
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
