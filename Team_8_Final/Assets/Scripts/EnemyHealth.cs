using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 20;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} is dead.");

        GameHandler handler = FindObjectOfType<GameHandler>();
        if (handler != null)
        {
            // Set this enemy to null in the enemiesToDefeat array
            for (int i = 0; i < handler.enemiesToDefeat.Length; i++)
            {
                if (handler.enemiesToDefeat[i] == gameObject)
                {
                    handler.enemiesToDefeat[i] = null;
                    break;
                }
            }

            handler.CheckEnemiesStatus();
        }

        Destroy(gameObject);
    }
}
