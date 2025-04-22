using UnityEngine;

public class FireDamage : MonoBehaviour {
    public int damage = 10;

    private GameHandler gameHandler;
    private PlayerHealthBar playerHealthBar;

    void Start() {
        gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        playerHealthBar = GameObject.FindWithTag("PlayerHealthBar").GetComponent<PlayerHealthBar>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            gameHandler.playerGetHit(damage);
            playerHealthBar.UpdateHealthBar();
        }
    }
}