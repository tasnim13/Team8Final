using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialAttack : MonoBehaviour
{

    public GameObject roarProjectile;
    public GameObject falconProjectile;
    public Transform firePoint;
    public float projectileSpeed = 10f;
    public float flaconProjSpd = 10f;
    // public float projectileSpeed = 0f;
    public bool canSpecial = true;
    public bool useSpecial;
    private float lastFalconShotTime = -Mathf.Infinity;

    // Update is called once per frame
    // TODO: restrict to form
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.F)) {
        //     roarAttack();
        // }
    }

    public void defaultSpatk() {
        if (canSpecial) {
            //Lets the cat interaction know it can take place
            useSpecial = true;
        }
    }

    public void lionessSpatk() {
        if (canSpecial) {
            StartCoroutine(multiRoar());
        }
    }

    public void falconSpatk() {
        if (canSpecial) {
            falconSpecial();
        }
    }

    private void basicAttack(int num) {
        // number of projectiles
        // TODO: ??
        // Vector2 fwd = (firePoint.position - this.transform.position).normalized; 
        // Debug.Log("roar attack!!!");
        Vector2 fwd = Vector2.up;

        // GameObject projectile;
        
        for (int i = 0; i < num; i++) {
            Vector3 orb_placement = new Vector3(0, 0, i * (360f / num));
            Vector3 fwd3 = fwd;
            // GameObject projectile = Instantiate(projectilePrefab, firePoint.position + fwd3, Quaternion.identity);
            // Start at center?
            GameObject projectile = Instantiate(roarProjectile, firePoint.position, Quaternion.identity);

            projectile.GetComponent<Rigidbody2D>().AddForce(fwd * projectileSpeed, ForceMode2D.Impulse); 
            projectile.GetComponentInChildren<SpriteRenderer>().transform.Rotate(orb_placement);
            fwd = Quaternion.AngleAxis(360f / num, Vector3.forward) * fwd;
        }
    }

    IEnumerator multiRoar() {
        basicAttack(8);
        yield return new WaitForSeconds(0.15f);
        basicAttack(8);
        yield return new WaitForSeconds(0.15f);
        basicAttack(8);
    }

    void falconSpecial() {
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
        GameObject proj = Instantiate(falconProjectile, firePoint.position, rot);

        //Set direction of projectile
        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        if (projRb != null) {
            projRb.velocity = direction.normalized * flaconProjSpd;
        }

        lastFalconShotTime = Time.time;
    }
}
