using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalconAttack : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PhoenixDamageZone")) {
            // Phoenix uses a special damage zone
            EnemyPhoenix phoenix = other.GetComponentInParent<EnemyPhoenix>();
            if (phoenix != null) {
                phoenix.TakeDamage(damage);
                Destroy(gameObject);
            }
        } else if (other.CompareTag("Enemy")) {
            EnemyPhoenix phoenix = other.GetComponentInParent<EnemyPhoenix>();
            if (phoenix != null) return;

            other.GetComponent<EnemyParent>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
