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
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyParent>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
