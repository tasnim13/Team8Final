using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialMove : MonoBehaviour
{

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // TODO: restrict to form
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            roarAttack();
        }
    }


    void roarAttack() {
        // TODO: ??
        // Vector2 fwd = (firePoint.position - this.transform.position).normalized; 
        Vector2 fwd = Vector2.up;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().AddForce(fwd * projectileSpeed, ForceMode2D.Impulse); 
    }
}
