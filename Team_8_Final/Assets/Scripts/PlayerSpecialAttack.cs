using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialAttack : MonoBehaviour
{

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;

    // Update is called once per frame
    // TODO: restrict to form
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            roarAttack();
        }
    }


    void roarAttack() {
        // number of projectiles
        int num = 16;
        // TODO: ??
        // Vector2 fwd = (firePoint.position - this.transform.position).normalized; 
        // Debug.Log("roar attack!!!");
        Vector2 fwd = Vector2.up;

        // GameObject projectile;
        
        for (int i = 0; i < num; i++) {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().AddForce(fwd * projectileSpeed, ForceMode2D.Impulse); 
            projectile.GetComponentInChildren<SpriteRenderer>().transform.Rotate(new Vector3(0, 0, i * (360f / num)));
            fwd = Quaternion.AngleAxis(360f / num, Vector3.forward) * fwd;
        }
    }
}
