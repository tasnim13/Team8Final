using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameHandler.hasKey = true;
            Destroy(gameObject); 
        }
    }
}
