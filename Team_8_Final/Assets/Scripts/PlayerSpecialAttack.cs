using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialAttack : MonoBehaviour
{

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;
    // public float projectileSpeed = 0f;

    // Update is called once per frame
    // TODO: restrict to form
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.F)) {
        //     roarAttack();
        // }
    }


    public void roarAttack() {
        basicAttack(8);
    }


    public void ramAttack() {
        basicAttack(4);
    }

    public void snakeAttack() {
        basicAttack(3);
    }

    public void falconAttack() {
        basicAttack(16);
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
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            projectile.GetComponent<Rigidbody2D>().AddForce(fwd * projectileSpeed, ForceMode2D.Impulse); 
            projectile.GetComponentInChildren<SpriteRenderer>().transform.Rotate(orb_placement);
            fwd = Quaternion.AngleAxis(360f / num, Vector3.forward) * fwd;
        }
    }

}
