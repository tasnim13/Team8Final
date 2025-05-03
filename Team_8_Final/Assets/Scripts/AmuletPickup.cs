using UnityEngine;

public class AmuletPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AmuletManager.Instance.CollectAmulet();
            Destroy(gameObject); // Or gameObject.SetActive(false);
        }
    }
}
