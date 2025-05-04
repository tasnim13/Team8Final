using UnityEngine;

public class KeyPickup : MonoBehaviour
{

    private GameHandler gh;

    void Start() {
        gh = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gh.hasKey = true;
            Destroy(gameObject); 
        }
    }
}
