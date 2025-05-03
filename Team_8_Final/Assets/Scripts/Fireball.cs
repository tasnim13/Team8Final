using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 3f;
    private GameHandler gameHandler;
    private PlayerHealthBar playerHealthBar;

    private void Start()
    {
        Destroy(gameObject, lifetime);

        //Identify GameHandler
        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        playerHealthBar = GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<PlayerHealthBar>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameHandler.playerGetHit(damage);
            playerHealthBar.UpdateHealthBar();
            Destroy(gameObject);
        }
        else if (!other.isTrigger && !other.CompareTag("Enemy")) {
            Destroy(gameObject); // Hit something solid that isn't the phoenix
        }
    }
}